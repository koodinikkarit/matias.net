package main

import (
	"context"
	"flag"
	"log"
	"os"
	"os/signal"
	"path/filepath"
	"syscall"
	"time"

	"genz/matias/internal/app"
	"genz/matias/internal/config"
)

func main() {
	var (
		configFile = flag.String("config", "config", "Polku asetustiedostoon")
		watch      = flag.Bool("watch", false, "Seuraa EW-lukitusta ja synkkaa automaattisesti")
		interval   = flag.Duration("interval", 5*time.Minute, "Synkronointiväli automaattimoodissa")
	)
	flag.Parse()

	cfg, err := config.Load(*configFile)
	if err != nil {
		log.Fatalf("Asetusten lukeminen epäonnistui: %v", err)
	}

	if !filepath.IsAbs(cfg.EWDatabaseDir) {
		cwd, _ := os.Getwd()
		cfg.EWDatabaseDir = filepath.Join(cwd, cfg.EWDatabaseDir)
	}

	ctx, stop := signal.NotifyContext(context.Background(), syscall.SIGINT, syscall.SIGTERM)
	defer stop()

	service, err := app.New(cfg)
	if err != nil {
		log.Fatalf("Sovelluksen alustaminen epäonnistui: %v", err)
	}
	defer func() {
		if cerr := service.Close(); cerr != nil {
			log.Printf("Virhe vapauttaessa resursseja: %v", cerr)
		}
	}()

	if *watch {
		log.Println("Automaattinen tila päällä – odotetaan lukituksen vapautumista")
		if err := service.AutoSync(ctx, *interval); err != nil && err != context.Canceled {
			log.Fatalf("Synkronointi epäonnistui: %v", err)
		}
		return
	}

	if err := service.Sync(ctx); err != nil {
		log.Fatalf("Synkronointi epäonnistui: %v", err)
	}

	log.Println("Valmis")
}
