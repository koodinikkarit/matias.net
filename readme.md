# Matias .NET
Matias on .NET-sovellus, joka lukee EW-tietokannan laulut ja synkronoi ne Seppo-palveluun gRPC:n kautta. Konsolisovellus (`matias-console.net`) vastaa synkronoinnin ajamisesta paikallisilla asetuksilla, ja `common-matias.net` sisältää yhteiset rajapinnat sekä MatiasService-protosta generoidut asiakaskoodit.

### Asetukset
Asetukset laitetaan `config`-nimiseen tiedostoon samaan kansioon kuin Matiasin suoritettava tiedosto.

```
seppoip = Seppo servicen ip
seppoport = Seppo service port
ewdatabasepath = Ewtietokannan tiedostopolku
ewdatabasekey = Ewtietokannan avain joka lähetetään seppo servicelle sync viestissä
```

### Käyttö
1. Asenna riippuvuudet: `nuget restore matias.net.sln`
2. Käännä konsolisovellus: `msbuild matias-console.net/matias-console.net.csproj`
3. Aseta `config`-tiedosto ajohakemistoon yllä kuvatulla muodolla.
4. Suorita synkronointi: `matias-console.net/bin/Debug/matias-console.net.exe`

Ohjelma yhdistää Seppo-palveluun, lukee EW-tietokannan tiedot ja lähettää mahdolliset muutokset MatiasService-rajapinnan kautta, jotta Seppo pysyy ajan tasalla laulutietojen osalta.
