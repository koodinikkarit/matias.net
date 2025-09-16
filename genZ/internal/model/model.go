package model

import "time"

// Song represents a lyric row stored in EW databases.
type Song struct {
	ID            int       `json:"id"`
	VariationID   uint32    `json:"variationId,omitempty"`
	Title         string    `json:"title"`
	Author        string    `json:"author"`
	Copyright     string    `json:"copyright"`
	Administrator string    `json:"administrator"`
	Description   string    `json:"description"`
	Tags          string    `json:"tags"`
	Text          string    `json:"text"`
	UpdatedAt     time.Time `json:"updatedAt,omitempty"`
}

// SyncRequest describes the payload sent to Seppo over the websocket.
type SyncRequest struct {
	Type          string `json:"type"`
	EWDatabaseKey string `json:"ewDatabaseKey"`
	Songs         []Song `json:"songs"`
}

// SyncResponse mirrors the server reply for sync requests.
type SyncResponse struct {
	Type          string `json:"type"`
	Songs         []Song `json:"songs"`
	RemoveSongIDs []uint32 `json:"removeSongIds"`
}

// MappingUpdate informs the server about newly allocated EW song ids.
type MappingUpdate struct {
	Type          string          `json:"type"`
	EWDatabaseKey string          `json:"ewDatabaseKey"`
	Mappings      []VariationLink `json:"mappings"`
	RekeyedSongs  []NewSongID     `json:"rekeyedSongs,omitempty"`
}

// VariationLink couples the remote variation id with the new EW song id.
type VariationLink struct {
	VariationID uint32 `json:"variationId"`
	EWSongID    uint32 `json:"ewSongId"`
}

// NewSongID links old and new EW identifiers generated when the database is rebuilt.
type NewSongID struct {
	OldID uint32 `json:"oldId"`
	NewID uint32 `json:"newId"`
}

// ErrorMessage captures protocol-level error responses.
type ErrorMessage struct {
	Type    string `json:"type"`
	Code    string `json:"code"`
	Message string `json:"message"`
}
