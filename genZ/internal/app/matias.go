package app

import (
	"context"
	"errors"
	"fmt"
	"log"
	"sort"
	"time"

	"genz/matias/internal/config"
	"genz/matias/internal/ewdb"
	"genz/matias/internal/model"
	"genz/matias/internal/seppo"
)

// App coordinates local EW database access and communication with Seppo.
type App struct {
    cfg   config.Config
    db    *ewdb.Database
    ws    *seppo.Client
}

// New creates an App using the provided configuration.
func New(cfg config.Config) (*App, error) {
	db, err := ewdb.Open(cfg.EWDatabaseDir)
	if err != nil {
		return nil, err
	}
	client, err := seppo.New(cfg.SeppoURL)
	if err != nil {
		db.Close()
		return nil, err
	}
	return &App{cfg: cfg, db: db, ws: client}, nil
}

// Close releases database and websocket resources.
func (a *App) Close() error {
	var first error
	if a.ws != nil {
		if err := a.ws.Close(); err != nil && !errors.Is(err, context.Canceled) {
			first = err
		}
	}
	if a.db != nil {
		if err := a.db.Close(); err != nil && first == nil {
			first = err
		}
	}
	return first
}

// Sync pushes local state to Seppo and reconciles remote updates.
func (a *App) Sync(ctx context.Context) error {
	if a.db.Locked() {
		return fmt.Errorf("database locked: odota EW:n vapautumista")
	}

	log.Println("Synkronoidaan tietokantoja…")
	songs, err := a.db.GetSongs()
	if err != nil {
		return fmt.Errorf("hae laulut: %w", err)
	}

	send := make([]model.Song, 0, len(songs))
	for _, song := range songs {
		if song.ID > 0 {
			send = append(send, song)
		}
	}

	resp, err := a.ws.SyncDatabase(ctx, a.cfg.EWDatabaseKey, send)
	if err != nil {
		return fmt.Errorf("sync remote: %w", err)
	}

	log.Printf("Palvelin palautti %d lisättävää tai päivitettävää laulua ja %d poistettavaa.\n", len(resp.Songs), len(resp.RemoveSongIDs))
	if len(resp.Songs) == 0 && len(resp.RemoveSongIDs) == 0 {
		return nil
	}

	links, idmap, err := a.applyUpdates(ctx, resp)
	if err != nil {
		return err
	}

	if err := a.ws.PublishMappings(ctx, a.cfg.EWDatabaseKey, links, idmap); err != nil {
		return fmt.Errorf("lähetä id-mäppäykset: %w", err)
	}

	return nil
}

func (a *App) applyUpdates(ctx context.Context, resp model.SyncResponse) ([]model.VariationLink, []model.NewSongID, error) {
	links, err := a.db.UpdateOrCreateSongs(ctx, resp.Songs)
	if err == nil {
		if err := a.db.RemoveSongs(ctx, resp.RemoveSongIDs); err != nil {
			return nil, nil, fmt.Errorf("poista lauluja: %w", err)
		}
		return links, nil, nil
	}

	log.Printf("Synkronointi epäonnistui (%v), rakennetaan EW uudestaan…", err)
	mapping, fixErr := a.db.FixDatabase(ctx)
	if fixErr != nil {
		return nil, nil, fmt.Errorf("uudelleenrakennus epäonnistui: %w", fixErr)
	}

	rebuilt := remapSongs(resp.Songs, mapping)
	links, err = a.db.UpdateOrCreateSongs(ctx, rebuilt)
	if err != nil {
		return nil, nil, fmt.Errorf("päivitä laulut uudelleenrakennuksen jälkeen: %w", err)
	}

	mappedRemovals := remapIDs(resp.RemoveSongIDs, mapping)
	if err := a.db.RemoveSongs(ctx, mappedRemovals); err != nil {
		return nil, nil, fmt.Errorf("poista lauluja uudelleenrakennuksen jälkeen: %w", err)
	}

	rekey := make([]model.NewSongID, 0, len(mapping))
	for oldID, newID := range mapping {
		if oldID == newID {
			continue
		}
		rekey = append(rekey, model.NewSongID{OldID: uint32(oldID), NewID: uint32(newID)})
	}
	sort.Slice(rekey, func(i, j int) bool { return rekey[i].OldID < rekey[j].OldID })

	return links, rekey, nil
}

func remapSongs(songs []model.Song, mapping map[int]int) []model.Song {
	if len(mapping) == 0 {
		return songs
	}

	cloned := make([]model.Song, len(songs))
	for i, song := range songs {
		if newID, ok := mapping[song.ID]; ok {
			song.ID = newID
		}
		cloned[i] = song
	}
	return cloned
}

func remapIDs(ids []uint32, mapping map[int]int) []uint32 {
	if len(mapping) == 0 {
		return ids
	}

	out := make([]uint32, len(ids))
	for i, id := range ids {
		if newID, ok := mapping[int(id)]; ok {
			out[i] = uint32(newID)
		} else {
			out[i] = id
		}
	}
	return out
}

// Watch unlock events and run sync once the database becomes available.
func (a *App) AutoSync(ctx context.Context, interval time.Duration) error {
    go func() {
        if err := a.db.RunLockObserver(ctx); err != nil && !errors.Is(err, context.Canceled) {
            log.Printf("Lukituksen seuranta päättyi: %v", err)
        }
    }()

    lockCh := a.db.SubscribeLockStates()
    lastLocked := true
    var (
        ticker *time.Ticker
        tickC <-chan time.Time
    )
    if interval > 0 {
        ticker = time.NewTicker(interval)
        tickC = ticker.C
        defer ticker.Stop()
    }

    for {
        select {
        case <-ctx.Done():
            return ctx.Err()
        case locked, ok := <-lockCh:
            if !ok {
                return errors.New("lock subscription closed")
            }
            lastLocked = locked
            if locked {
                log.Println("EW lukossa – odotetaan vapautusta")
                continue
            }
            log.Println("EW vapautui – käynnistetään synkronointi")
            if err := a.Sync(ctx); err != nil {
                log.Printf("Synkronointi epäonnistui: %v", err)
            }
            if ticker == nil {
                return nil
            }
        case <-tickC:
            if lastLocked {
                log.Println("EW yhä lukossa – ohitetaan ajastettu ajo")
                continue
            }
            if err := a.Sync(ctx); err != nil {
                log.Printf("Synkronointi epäonnistui: %v", err)
            }
        }
    }
}
