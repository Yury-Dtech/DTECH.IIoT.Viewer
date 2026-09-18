# Plan praktyk — D-TECH IIoT Viewer

Czas: 4 tygodnie, 20 dni roboczych.
Tygodnie 1–3: budowanie aplikacji. Tydzień 4: poprawki, weryfikacja i przekazanie.

Praktykant uczy się C#, SQL i Blazora, wykonując kolejne części jednego programu.
Podstawą jest zaakceptowany mock „4 ekrany — GIP razem”
(`D-TECH-IIoT-Viewer-mock-4-ekrany-GIP-razem.html`), a nie wcześniejsza,
rozbudowana wersja analizatora.

Ustalona technologia: C#, .NET 10, Blazor Hybrid w oknie WPF z BlazorWebView,
SQL Server oraz Telerik UI for Blazor. WPF pozostaje obudową programu —
ekrany powstają w Blazorze.

## Co ma powstać

| Ekran | Zakres do wykonania |
|---|---|
| 1. Połączenie SQL | Osobny ekran: serwer, baza, uwierzytelnianie Windows lub SQL, login i hasło. Test połączenia, komunikat o wyniku, zapamiętanie ustawień bez hasła. |
| 2. Wybór kolektora | Lista kolektorów, wyszukiwanie po nazwie, SN lub IP, informacja o aktualności danych. Wybrany kolektor jest wspólny dla wykresów i sterowania. |
| 3. Wykresy GIP i energii | Wszystkie wejścia GIP jednocześnie, jedno pod drugim w jednym panelu, ze wspólną osią czasu i kursorem. Poniżej wykres energii: moc lub licznik kWh. Wspólny zakres czasu i ręczne odświeżanie. |
| 4. Sterowanie wyjściami | Ustawianie pojedynczych wyjść ON/OFF przez API. Potwierdzenie urządzenia i wyjścia przed wysłaniem, pokazanie wyniku, obsługa błędów. |

Sterowanie ma działać pojedynczymi poleceniami, bez automatycznych ponowień,
z potwierdzeniem przed wysłaniem.

**Nie dokładamy:** analizy czasu pracy, liczników produkcji, KPI, drgań,
raportów, SSH ani obsługi kilku modeli bazy.

## Zasada nadrzędna

HTML jest wzorem interfejsu, nie gotowym programem. Mock symuluje SQL, API
i pomiary; praktykant ma zastąpić te symulacje rzeczywistym odczytem
i komunikacją w C#.

Numeracja wyjść i sposób potwierdzania stanu muszą pochodzić z dokumentacji
urządzenia, nie z demonstracyjnych oznaczeń mocka.

## Przygotowanie przed pierwszym dniem — opiekun

- [x] Uruchamialny szkielet Blazor/WebView z Telerik.
- [x] Repozytorium Git.
- [ ] Dostęp do testowej bazy SQL, tylko do odczytu.
- [ ] Wskazane tabele i kolumny: lista kolektorów, GIP2, energia — wraz ze
      znaczeniem czasu i jednostek.
- [ ] Dokumentacja API sterowania wyjściami i przykład poprawnego wywołania.
- [ ] Kolektor testowy, niepodłączony do maszyn.

Pozycje niezaznaczone uzupełnia opiekun w pliku `docs/srodowisko.md`.

## Tydzień 1 — uruchomienie projektu, SQL i wybór kolektora

Cel tygodnia: działają ekrany 1 i 2. Program łączy się z bazą i pozwala wybrać
kolektor.

### Dzień 1 — projekt i nawigacja

Nauka: przejść z opiekunem przez mock, uruchomić szkielet aplikacji, poznać
komponent Razor, prostą metodę C#, obsługę przycisku i zapis zmian w Git.

Zadanie: przygotować cztery ekrany oraz przełączanie między nimi.

Efekt: program uruchamia się jako aplikacja Windows, można przejść między
czterema ekranami.

### Dzień 2 — podstawy C# i danych SQL

Nauka: klasa, właściwości, lista i metoda na przykładzie kolektora.

Zadanie: napisać zapytania `SELECT`, `WHERE`, `ORDER BY` pobierające kolektory
i przykładowe rekordy pomiarów.

Efekt: zapisane zapytania kontrolne. Praktykant potrafi wskazać identyfikator
kolektora, czas i wartość pomiaru.

### Dzień 3 — ekran połączenia

Zadanie: zbudować formularz zgodny z mockiem. Podłączyć przycisk
„Testuj połączenie” do rzeczywistego SQL. Obsłużyć poprawne połączenie,
błędny login i niedostępny serwer.

Efekt: test połączenia działa i pokazuje zrozumiały wynik.

### Dzień 4 — ustawienia i lista urządzeń

Zadanie: zapamiętać ustawienia połączenia bez hasła. Wydzielić pobieranie
danych do osobnej klasy C#. Pobrać kolektory z SQL i wyświetlić w tabeli Telerik.

Efekt: po ponownym uruchomieniu ustawienia są zachowane, a lista pochodzi z bazy.

### Dzień 5 — wybór kolektora

Zadanie: dodać wyszukiwanie po nazwie, SN i IP, informację o aktualności danych
oraz wybór urządzenia. Zachować wybór przy przechodzeniu do wykresów i wyjść.

Efekt: można znaleźć i wybrać kolektor. Na kolejnych ekranach widoczna jest jego
właściwa nazwa, SN i IP.

### Checkout po tygodniu 1

Pokaz: praktykant uruchamia aplikację, testuje poprawne i błędne połączenie,
wyszukuje kolektor i pokazuje odpowiadający mu rekord w SQL.

Powinien już umieć: utworzyć prostą klasę i listę obiektów, napisać podstawowe
zapytanie SQL, obsłużyć przycisk w Blazorze, zatrzymać kod w debuggerze
i zapisać zmiany w Git.

**Warunek odbioru:** oba ekrany pracują na rzeczywistych danych testowych.
Nie wystarcza lista wpisana na stałe w kodzie.

## Tygodnie 2–4

Zakres kolejnych tygodni: ekran 3 (wykresy GIP i energii), ekran 4 (sterowanie
wyjściami), a następnie poprawki, weryfikacja i przekazanie. Szczegółowy podział
na dni zostanie dopisany po checkoucie tygodnia 1.
