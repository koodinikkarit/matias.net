package ewdb

import (
	"context"
	"crypto/rand"
	"database/sql"
	"encoding/hex"
	"errors"
	"fmt"
	"os"
	"path/filepath"
	"strings"
	"sync"
	"time"

	"github.com/fsnotify/fsnotify"
	_ "github.com/mattn/go-sqlite3"

	"genz/matias/internal/model"
)

// Database encapsulates access to the EW SQLite files and lock signals.
type Database struct {
	rootDir  string
	songs    *sql.DB
	words    *sql.DB
	watcher  *fsnotify.Watcher
	mu       sync.RWMutex
	locked   bool
	lockSubs []chan bool
}

// Open initialises connections to Songs.db and SongWords.db located under the
// EW export directory.
func Open(root string) (*Database, error) {
	songsPath := filepath.Join(root, "Data", "Songs.db")
	wordsPath := filepath.Join(root, "Data", "SongWords.db")

	songs, err := sql.Open("sqlite3", fmt.Sprintf("file:%s?mode=rw", songsPath))
	if err != nil {
		return nil, fmt.Errorf("open Songs.db: %w", err)
	}

	words, err := sql.Open("sqlite3", fmt.Sprintf("file:%s?mode=rw", wordsPath))
	if err != nil {
		songs.Close()
		return nil, fmt.Errorf("open SongWords.db: %w", err)
	}

	if err := songs.Ping(); err != nil {
		songs.Close()
		words.Close()
		return nil, fmt.Errorf("ping Songs.db: %w", err)
	}
	if err := words.Ping(); err != nil {
		songs.Close()
		words.Close()
		return nil, fmt.Errorf("ping SongWords.db: %w", err)
	}

    var watcher *fsnotify.Watcher
    locksDir := filepath.Join(root, "Locks")
    if info, err := os.Stat(locksDir); err == nil && info.IsDir() {
        watcher, err = fsnotify.NewWatcher()
        if err != nil {
            songs.Close()
            words.Close()
            return nil, fmt.Errorf("create watcher: %w", err)
        }
        if err := watcher.Add(locksDir); err != nil {
            watcher.Close()
            watcher = nil
        }
    }

    db := &Database{
        rootDir: root,
        songs:   songs,
        words:   words,
        watcher: watcher,
        locked:  isLocked(locksDir),
    }

	return db, nil
}

// Close releases database connections and watchers.
func (db *Database) Close() error {
	db.mu.Lock()
	defer db.mu.Unlock()

	if db.watcher != nil {
		db.watcher.Close()
	}
	if db.songs != nil {
		db.songs.Close()
	}
	if db.words != nil {
		db.words.Close()
	}
	for _, ch := range db.lockSubs {
		close(ch)
	}
	db.lockSubs = nil
	return nil
}

// SubscribeLockStates provides a channel that emits lock status changes.
func (db *Database) SubscribeLockStates() <-chan bool {
	db.mu.Lock()
	defer db.mu.Unlock()

	ch := make(chan bool, 1)
	ch <- db.locked
	db.lockSubs = append(db.lockSubs, ch)
	return ch
}

// RunLockObserver pumps fsnotify events and notifies subscribers.
func (db *Database) RunLockObserver(ctx context.Context) error {
    if db.watcher == nil {
        <-ctx.Done()
        return ctx.Err()
    }
    for {
		select {
		case <-ctx.Done():
			return ctx.Err()
		case evt, ok := <-db.watcher.Events:
			if !ok {
				return errors.New("lock watcher closed")
			}
			if !strings.HasSuffix(evt.Name, "Client (1).ulck") {
				continue
			}

			locked := evt.Op&fsnotify.Create == fsnotify.Create || evt.Op&fsnotify.Write == fsnotify.Write
			if evt.Op&fsnotify.Remove == fsnotify.Remove || evt.Op&fsnotify.Rename == fsnotify.Rename {
				locked = false
			}
			db.setLock(locked)
		case err, ok := <-db.watcher.Errors:
			if !ok {
				return errors.New("lock watcher error channel closed")
			}
			return err
		}
	}
}

func (db *Database) setLock(state bool) {
	db.mu.Lock()
	defer db.mu.Unlock()

	if db.locked == state {
		return
	}
	db.locked = state
	for _, ch := range db.lockSubs {
		select {
		case ch <- state:
		default:
		}
	}
}

// Locked reports the last observed lock state.
func (db *Database) Locked() bool {
	db.mu.RLock()
	defer db.mu.RUnlock()
	return db.locked
}

// GetSongs pulls song metadata and text into a single slice.
func (db *Database) GetSongs() ([]model.Song, error) {
	rows, err := db.songs.Query(`SELECT rowid, title, author, copyright, administrator, description, tags FROM song`)
	if err != nil {
		return nil, fmt.Errorf("query songs: %w", err)
	}
	defer rows.Close()

	titles := map[int]*model.Song{}

    for rows.Next() {
        song := &model.Song{}
        if err := rows.Scan(&song.ID, &song.Title, &song.Author, &song.Copyright, &song.Administrator, &song.Description, &song.Tags); err != nil {
            return nil, fmt.Errorf("scan song: %w", err)
        }
        titles[song.ID] = song
    }

	if err := rows.Err(); err != nil {
		return nil, fmt.Errorf("iterate songs: %w", err)
	}

	textRows, err := db.words.Query(`SELECT song_id, words FROM word`)
	if err != nil {
		return nil, fmt.Errorf("query words: %w", err)
	}
	defer textRows.Close()

	for textRows.Next() {
		var (
			id   int
			text string
		)
		if err := textRows.Scan(&id, &text); err != nil {
			return nil, fmt.Errorf("scan word row: %w", err)
		}

		song, ok := titles[id]
		if !ok {
			// Create placeholder entry so server can request deletion.
			titles[id] = &model.Song{ID: id}
			song = titles[id]
		}

		song.Text = stripRTF(text)
	}

	if err := textRows.Err(); err != nil {
		return nil, fmt.Errorf("iterate words: %w", err)
	}

	songs := make([]model.Song, 0, len(titles))
	for _, s := range titles {
		songs = append(songs, *s)
	}
	return songs, nil
}

// UpdateOrCreateSongs upserts the provided songs and returns mappings between
// variation ids and newly generated EW song ids.
func (db *Database) UpdateOrCreateSongs(ctx context.Context, songs []model.Song) ([]model.VariationLink, error) {
	if len(songs) == 0 {
		return nil, nil
	}

	tx, err := db.songs.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin songs tx: %w", err)
	}
	wtx, err := db.words.BeginTx(ctx, nil)
	if err != nil {
		tx.Rollback()
		return nil, fmt.Errorf("begin words tx: %w", err)
	}

	var links []model.VariationLink

	for _, song := range songs {
		if song.ID > 0 {
			exists, err := db.songExists(ctx, tx, song.ID)
			if err != nil {
				wtx.Rollback()
				tx.Rollback()
				return nil, err
			}
			if exists {
				if err := updateSong(tx, wtx, song); err != nil {
					wtx.Rollback()
					tx.Rollback()
					return nil, err
				}
				continue
			}
		}

		newID, err := insertSong(tx, wtx, song)
		if err != nil {
			wtx.Rollback()
			tx.Rollback()
			return nil, err
		}
		links = append(links, model.VariationLink{
			VariationID: song.VariationID,
			EWSongID:    uint32(newID),
		})
	}

	if err := wtx.Commit(); err != nil {
		tx.Rollback()
		return nil, fmt.Errorf("commit word tx: %w", err)
	}
	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit song tx: %w", err)
	}

	return links, nil
}

// RemoveSongs deletes rows by EW song id.
func (db *Database) RemoveSongs(ctx context.Context, ids []uint32) error {
	if len(ids) == 0 {
		return nil
	}

	tx, err := db.songs.BeginTx(ctx, nil)
	if err != nil {
		return fmt.Errorf("begin song delete tx: %w", err)
	}
	wtx, err := db.words.BeginTx(ctx, nil)
	if err != nil {
		tx.Rollback()
		return fmt.Errorf("begin word delete tx: %w", err)
	}

	for _, id := range ids {
		if _, err := tx.ExecContext(ctx, `DELETE FROM song WHERE rowid = ?`, id); err != nil {
			wtx.Rollback()
			tx.Rollback()
			return fmt.Errorf("delete song %d: %w", id, err)
		}
		if _, err := wtx.ExecContext(ctx, `DELETE FROM word WHERE song_id = ?`, id); err != nil {
			wtx.Rollback()
			tx.Rollback()
			return fmt.Errorf("delete words %d: %w", id, err)
		}
	}

	if err := wtx.Commit(); err != nil {
		tx.Rollback()
		return fmt.Errorf("commit word delete: %w", err)
	}
	if err := tx.Commit(); err != nil {
		return fmt.Errorf("commit song delete: %w", err)
	}
	return nil
}

// FixDatabase rebuilds the song tables to obtain a clean set of rowids. It
// returns a mapping from old ids to new ids used by the caller when notifying
// Seppo.
func (db *Database) FixDatabase(ctx context.Context) (map[int]int, error) {
	songs, err := db.GetSongs()
	if err != nil {
		return nil, err
	}

	tx, err := db.songs.BeginTx(ctx, nil)
	if err != nil {
		return nil, fmt.Errorf("begin songs tx: %w", err)
	}
	wtx, err := db.words.BeginTx(ctx, nil)
	if err != nil {
		tx.Rollback()
		return nil, fmt.Errorf("begin words tx: %w", err)
	}

	if _, err := wtx.ExecContext(ctx, `DELETE FROM word`); err != nil {
		wtx.Rollback()
		tx.Rollback()
		return nil, fmt.Errorf("clear word table: %w", err)
	}

	if _, err := tx.ExecContext(ctx, `DROP TABLE IF EXISTS song`); err != nil {
		wtx.Rollback()
		tx.Rollback()
		return nil, fmt.Errorf("drop song table: %w", err)
	}

	if _, err := tx.ExecContext(ctx, `CREATE TABLE song (rowid integer PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE, song_item_uid text UNIQUE, song_rev_uid text, song_uid text, title text NOT NULL, author text, copyright text, administrator text, description text, tags text, reference_number text, vendor_id integer, presentation_id integer, layout_revision integer DEFAULT 1, revision integer DEFAULT 1 )`); err != nil {
		wtx.Rollback()
		tx.Rollback()
		return nil, fmt.Errorf("create song table: %w", err)
	}

	mapping := make(map[int]int, len(songs))

	for _, song := range songs {
		newID, err := insertSong(tx, wtx, song)
		if err != nil {
			wtx.Rollback()
			tx.Rollback()
			return nil, err
		}
		mapping[song.ID] = newID
	}

	if err := wtx.Commit(); err != nil {
		tx.Rollback()
		return nil, fmt.Errorf("commit words rebuild: %w", err)
	}
	if err := tx.Commit(); err != nil {
		return nil, fmt.Errorf("commit songs rebuild: %w", err)
	}

	return mapping, nil
}

func (db *Database) songExists(ctx context.Context, tx *sql.Tx, id int) (bool, error) {
	row := tx.QueryRowContext(ctx, `SELECT 1 FROM song WHERE rowid = ?`, id)
	var dummy int
	switch err := row.Scan(&dummy); err {
	case sql.ErrNoRows:
		return false, nil
	case nil:
		return true, nil
	default:
		return false, fmt.Errorf("lookup song %d: %w", id, err)
	}
}

func insertSong(tx *sql.Tx, wtx *sql.Tx, song model.Song) (int, error) {
	songUID := newUUID()
	res, err := tx.Exec(`INSERT INTO song (song_item_uid, song_uid, title, author, copyright, administrator, description, tags) VALUES (?, ?, ?, ?, ?, ?, ?, ?)`, songUID, songUID, song.Title, song.Author, song.Copyright, song.Administrator, song.Description, song.Tags)
	if err != nil {
		return 0, fmt.Errorf("insert song: %w", err)
	}

	rowID64, err := res.LastInsertId()
	if err != nil {
		return 0, fmt.Errorf("fetch song id: %w", err)
	}
	rowID := int(rowID64)

	if _, err := wtx.Exec(`INSERT INTO word (song_id, words) VALUES(?, ?)`, rowID, wrapRTF(song.Text)); err != nil {
		return 0, fmt.Errorf("insert word: %w", err)
	}

	return rowID, nil
}

func updateSong(tx *sql.Tx, wtx *sql.Tx, song model.Song) error {
	if _, err := tx.Exec(`UPDATE song SET title = ?, author = ?, copyright = ?, administrator = ?, description = ?, tags = ? WHERE rowid = ?`, song.Title, song.Author, song.Copyright, song.Administrator, song.Description, song.Tags, song.ID); err != nil {
		return fmt.Errorf("update song %d: %w", song.ID, err)
	}
	if _, err := wtx.Exec(`UPDATE word SET words = ? WHERE song_id = ?`, wrapRTF(song.Text), song.ID); err != nil {
		return fmt.Errorf("update words %d: %w", song.ID, err)
	}
	return nil
}

func isLocked(locksDir string) bool {
	lockFile := filepath.Join(locksDir, "Client (1).ulck")
	if _, err := os.Stat(lockFile); err == nil {
		return true
	}
	return false
}

func wrapRTF(text string) string {
	text = strings.ReplaceAll(text, "\\", "\\\\")
	text = strings.ReplaceAll(text, "\n", `\par `)
	return fmt.Sprintf("{\\rtf1{\\pard %s}}", text)
}

func stripRTF(rtf string) string {
	if rtf == "" {
		return ""
	}

	rtf = strings.TrimPrefix(rtf, "{\\rtf1")
	rtf = strings.TrimSuffix(rtf, "}")
	rtf = strings.ReplaceAll(rtf, "\\par ", "\n")
	rtf = strings.ReplaceAll(rtf, "\\pard", "")
	rtf = strings.ReplaceAll(rtf, "\\", "")
	return strings.TrimSpace(rtf)
}

func newUUID() string {
	b := make([]byte, 16)
	if _, err := rand.Read(b); err != nil {
		return fmt.Sprintf("%d", time.Now().UnixNano())
	}
	b[6] = (b[6] & 0x0f) | 0x40
	b[8] = (b[8] & 0x3f) | 0x80
	return fmt.Sprintf("%s-%s-%s-%s-%s",
		hex.EncodeToString(b[0:4]),
		hex.EncodeToString(b[4:6]),
		hex.EncodeToString(b[6:8]),
		hex.EncodeToString(b[8:10]),
		hex.EncodeToString(b[10:16]))
}
