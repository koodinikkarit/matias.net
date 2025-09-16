package config

import (
	"bufio"
	"errors"
	"fmt"
	"os"
	"path/filepath"
	"strings"
)

// Config mirrors the legacy config file used by the C# implementation.
type Config struct {
	SeppoURL      string
	EWDatabaseDir string
	EWDatabaseKey string
}

// Load reads a simple key=value config file. Keys are case-insensitive and
// whitespace around values is trimmed.
func Load(path string) (Config, error) {
	file, err := os.Open(path)
	if err != nil {
		return Config{}, fmt.Errorf("open config: %w", err)
	}
	defer file.Close()

	cfg := Config{}
	var (
		host string
		port string
	)

	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		line := strings.TrimSpace(scanner.Text())
		if line == "" || strings.HasPrefix(line, "#") {
			continue
		}

		parts := strings.SplitN(line, "=", 2)
		if len(parts) != 2 {
			return Config{}, fmt.Errorf("invalid config row: %q", line)
		}

		key := strings.TrimSpace(strings.ToLower(parts[0]))
		value := strings.TrimSpace(parts[1])

		switch key {
		case "seppoip":
			host = value
		case "seppoport":
			port = value
		case "seppourl":
			cfg.SeppoURL = value
		case "ewdatabasepath":
			cfg.EWDatabaseDir = filepath.Clean(value)
		case "ewdatabasekey":
			cfg.EWDatabaseKey = value
		default:
			return Config{}, fmt.Errorf("tuntematon avain %q", key)
		}
	}

	if err := scanner.Err(); err != nil {
		return Config{}, fmt.Errorf("read config: %w", err)
	}

	if cfg.SeppoURL == "" {
		if host == "" {
			host = "localhost"
		}
		if port == "" {
			port = "80"
		}
		cfg.SeppoURL = fmt.Sprintf("ws://%s:%s", host, port)
	}
	if cfg.EWDatabaseDir == "" {
		return Config{}, errors.New("config: ewdatabasepath puuttuu")
	}
	if cfg.EWDatabaseKey == "" {
		return Config{}, errors.New("config: ewdatabasekey puuttuu")
	}

	return cfg, nil
}
