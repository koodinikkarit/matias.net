package seppo

import (
	"context"
	"encoding/json"
	"fmt"
	"net/url"
	"sync"
	"time"

	"github.com/gorilla/websocket"

	"genz/matias/internal/model"
)

const (
	messageTypeSyncRequest  = "sync_request"
	messageTypeSyncResponse = "sync_result"
	messageTypeMappings     = "mapping_update"
	messageTypeError        = "error"
)

// Client communicates with the Seppo backend over websocket.
type Client struct {
	endpoint string
	mu       sync.Mutex
	conn     *websocket.Conn
}

// New constructs a websocket client targeting the provided endpoint.
func New(endpoint string) (*Client, error) {
	u, err := url.Parse(endpoint)
	if err != nil {
		return nil, fmt.Errorf("invalid websocket endpoint: %w", err)
	}
	if u.Scheme == "" {
		u.Scheme = "ws"
	}
	if u.Path == "" || u.Path == "/" {
		u.Path = "/matias"
	}
	return &Client{endpoint: u.String()}, nil
}

func (c *Client) dial(ctx context.Context) error {
	c.mu.Lock()
	defer c.mu.Unlock()

	if c.conn != nil {
		return nil
	}

	dialer := &websocket.Dialer{HandshakeTimeout: 10 * time.Second}
	conn, _, err := dialer.DialContext(ctx, c.endpoint, nil)
	if err != nil {
		return fmt.Errorf("connect websocket: %w", err)
	}
	c.conn = conn
	return nil
}

// SyncDatabase sends the local song catalogue and waits for server decisions.
func (c *Client) SyncDatabase(ctx context.Context, key string, songs []model.Song) (model.SyncResponse, error) {
	if err := c.dial(ctx); err != nil {
		return model.SyncResponse{}, err
	}

	req := model.SyncRequest{
		Type:          messageTypeSyncRequest,
		EWDatabaseKey: key,
		Songs:         songs,
	}

	if err := c.writeJSON(ctx, req); err != nil {
		return model.SyncResponse{}, err
	}

	return c.readSyncResponse(ctx)
}

// PublishMappings informs Seppo about created EW song ids.
func (c *Client) PublishMappings(ctx context.Context, key string, links []model.VariationLink, rekey []model.NewSongID) error {
	if len(links) == 0 && len(rekey) == 0 {
		return nil
	}
	if err := c.dial(ctx); err != nil {
		return err
	}

	msg := model.MappingUpdate{
		Type:          messageTypeMappings,
		EWDatabaseKey: key,
		Mappings:      links,
		RekeyedSongs:  rekey,
	}
	return c.writeJSON(ctx, msg)
}

func (c *Client) writeJSON(ctx context.Context, payload any) error {
	c.mu.Lock()
	conn := c.conn
	c.mu.Unlock()

	if conn == nil {
		return fmt.Errorf("websocket not connected")
	}

	conn.SetWriteDeadline(time.Now().Add(10 * time.Second))
	if err := conn.WriteJSON(payload); err != nil {
		return fmt.Errorf("write websocket payload: %w", err)
	}
	return nil
}

func (c *Client) readSyncResponse(ctx context.Context) (model.SyncResponse, error) {
	c.mu.Lock()
	conn := c.conn
	c.mu.Unlock()

	if conn == nil {
		return model.SyncResponse{}, fmt.Errorf("websocket not connected")
	}

	deadline := time.Now().Add(30 * time.Second)
	conn.SetReadDeadline(deadline)

	for {
		type envelope struct {
			Type string `json:"type"`
		}
		_, data, err := conn.ReadMessage()
		if err != nil {
			return model.SyncResponse{}, fmt.Errorf("read websocket response: %w", err)
		}
		var env envelope
		if err := json.Unmarshal(data, &env); err != nil {
			return model.SyncResponse{}, fmt.Errorf("decode websocket envelope: %w", err)
		}

		switch env.Type {
		case messageTypeSyncResponse:
			var resp model.SyncResponse
			if err := json.Unmarshal(data, &resp); err != nil {
				return model.SyncResponse{}, fmt.Errorf("decode sync response: %w", err)
			}
			return resp, nil
		case messageTypeError:
			var msg model.ErrorMessage
			if err := json.Unmarshal(data, &msg); err != nil {
				return model.SyncResponse{}, fmt.Errorf("decode error message: %w", err)
			}
			return model.SyncResponse{}, fmt.Errorf("server error %s: %s", msg.Code, msg.Message)
		default:
			// ignore keepalive/unknown messages
		}
	}
}

// Close terminates the underlying websocket connection.
func (c *Client) Close() error {
	c.mu.Lock()
	defer c.mu.Unlock()

	if c.conn != nil {
		err := c.conn.Close()
		c.conn = nil
		return err
	}
	return nil
}
