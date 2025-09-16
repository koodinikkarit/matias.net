# Matias WebSocket Protocol

Matias-console connects to Seppo with a single WebSocket session. Unless otherwise
specified, payloads are JSON objects encoded in UTF-8.

## Connection

- URL: `ws://<host>:<port>/matias`
- Client must include the EW database key in every message.
- Server may send unsolicited `keepalive` frames with `{ "type": "ping" }`.

## Messages from Client

### Sync Request

```json
{
  "type": "sync_request",
  "ewDatabaseKey": "string",
  "songs": [
    {
      "id": 123,
      "variationId": 0,
      "title": "string",
      "author": "string",
      "copyright": "string",
      "administrator": "string",
      "description": "string",
      "tags": "string",
      "text": "plain lyric text"
    }
  ]
}
```

The array lists every song currently stored locally (only `id > 0` are sent).

### Mapping Update

```json
{
  "type": "mapping_update",
  "ewDatabaseKey": "string",
  "mappings": [
    { "variationId": 42, "ewSongId": 999 }
  ],
  "rekeyedSongs": [
    { "oldId": 1, "newId": 15 }
  ]
}
```

- `mappings` communicates EW identifiers allocated for new variations that the
  server asked to insert during the last sync.
- `rekeyedSongs` is optional and lists old/new ids generated when the EW
  database was rebuilt locally (e.g. after fixing corruption).

## Messages from Server

### Sync Result

```json
{
  "type": "sync_result",
  "songs": [
    {
      "id": 0,
      "variationId": 42,
      "title": "string",
      "author": "string",
      "text": "plain lyric text"
    }
  ],
  "removeSongIds": [13, 99]
}
```

- `songs` contains new or updated items that must be persisted to EW. The
  client may receive `id = 0` for brand new songs; in that case the client will
  insert a fresh row and return the allocated id in a subsequent
  `mapping_update`.
- `removeSongIds` enumerates local EW ids that must be deleted.

### Error

```json
{
  "type": "error",
  "code": "string",
  "message": "human readable"
}
```

Receiving an `error` frame terminates the current sync attempt; the client may
retry later.
