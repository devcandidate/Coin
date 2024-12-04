# Coin
To projekt demonstracyjny zawierający API do pobierania kursów walut (cena zakupy i sprzedaży) 🪙 na podstawie danych z Narodowego Banku Polskiego. <br />

## Stack
* NET 8, webApi
* Entity Framework
* MediatR - implementacja wzorca CQRS
* Fluent Validation - do walidacji zapytań i komend
* Hangfire - do obsługi zadań w tle
* Xunit - testy
* Mssql - baza danych
* Docker - do konteneryzacji aplikacji

## Jak uruchomić projekt?

```
git clone https://github.com/devcandidate/Coin.git
cd Coin
```

```
docker-compose build
```

```
docker-compose up
```

Po uruchomieniu aplikacja będzie dostępna pod adresem: ```http://localhost:8080/swagger```

## Pobieranie kursu dla danego dnia i waluty
```GET http://localhost:8080/api/ExchangeRates/2024-12-03/EUR```

Endpoint zwróci kurs jeżeli jest już on zsynchronizowany w bazie danych.

Parametry:

* date - Data w formacie yyyy-MM-dd (np. 2024-12-01).
* currency - Kod waluty w formacie ISO 4217 (np. EUR, USD).

## Endpoint administracyjny (wewnętrzny) do synchronizacji kursów dla zakresu dat z NBP
``` GET http://localhost:8080/api/System/SyncExchangeRates?startDate=2024-12-03&endDate=2024-12-03```

* startDate - Data początkowa w formacie yyyy-MM-dd.
* endDate - Data końcowa w formacie yyyy-MM-dd.
* token - Token autoryzacyjny. W dockerze jest skonfigurowany jako pusty, więc dla testów nie trzeba go podawać.


To endpoint synchronizacyjny dzięki któremu możemy pobrać i wypełnić lub przeładować bazę danych kursami z NBP dla danego zakresu dat.
Niezależnie od manualnych działań przy jego użyciu o 12:05 każdego dnia wywoła się zadanie ```ExchangeRatesSyncJob```, które przez Hangfire pobierze nowe kursy po ich udostępnieniu przez NBP.
