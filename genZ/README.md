# Matias Go Edition

Tämä kansio sisältää Matias-sovelluksen Go-toteutuksen, joka korvaa alkuperäisen
C#-version. Konsolisovellus lukee EW-tietokannan, synkronoi laulut Seppo-palveluun
WebSocket-rajapinnan yli ja päivittää paikallisen tietokannan palvelimen
vastauksen perusteella.

## Kehitys

1. Asenna Go 1.21 ja varmista, että `CGO_ENABLED=1` on käytössä, koska
   tietokanta käyttää `github.com/mattn/go-sqlite3` -ajuria.
2. Lataa riippuvuudet: `go mod tidy` (vaatii verkko-oikeudet).
3. Käännä konsoli: `go build ./cmd/matias`.
4. Aja: `./matias --config ../config`.

Kun `--watch` on päällä, sovellus seuraa EW:n lukitustiedostoa ja käynnistää
synkronoinnin automaattisesti tietokannan vapautuessa.

Lisätietoa WebSocket-protokollasta löytyy tiedostosta
[PROTOCOL.md](PROTOCOL.md).
