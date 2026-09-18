# D-TECH IIoT Viewer — szkielet projektu praktyk

Aplikacja desktopowa Windows do podglądu danych z kolektorów IIoT i sterowania
ich wyjściami. To **szkielet startowy**: uruchamia się, ma gotową nawigację i
konfigurację, ale logika czterech ekranów jest zadaniem praktykanta.

Wzorem interfejsu jest `D-TECH-IIoT-Viewer-mock-4-ekrany-GIP-razem.html`.
Mock symuluje SQL, API i pomiary — twoim zadaniem jest zastąpić te symulacje
rzeczywistym odczytem i komunikacją w C#.

## Technologia

| Element | Wybór |
|---|---|
| Język / platforma | C#, .NET 10 |
| Obudowa programu | WPF (okno główne) |
| Interfejs | Blazor Hybrid w `BlazorWebView` |
| Komponenty UI | Telerik UI for Blazor 15.0.1 |
| Baza danych | SQL Server, dostęp przez `Microsoft.Data.SqlClient` |

Zapytania SQL piszemy ręcznie. Świadomie nie używamy ORM-a — chodzi o to, żeby
było widać, co dokładnie jedzie do bazy.

## Wymagania przed pierwszym uruchomieniem

1. **.NET 10 SDK** — sprawdź: `dotnet --version` (oczekiwane 10.x).
2. **Visual Studio 2022/2026** z obciążeniem „.NET desktop development”,
   albo VS Code z rozszerzeniem C# Dev Kit.
3. **WebView2 Runtime** — na Windows 11 jest wbudowany.
4. **Dostęp do feedu NuGet Telerik.** Pakiety Telerik nie są publiczne.
   Wykonaj jednorazowo, wpisując dane konta Telerik:

   ```
   dotnet nuget update source TelerikNuGetV3 --username <login> --password <haslo> --store-password-in-clear-text
   ```

   Jeśli źródła jeszcze nie ma:

   ```
   dotnet nuget add source https://nuget.telerik.com/v3/index.json --name TelerikNuGetV3 --username <login> --password <haslo> --store-password-in-clear-text
   ```

   Dane logowania trzymamy w konfiguracji NuGet użytkownika, **nigdy w repozytorium**.
5. **Klucz licencyjny Telerik** — plik `telerik-license.txt` pobrany z konta
   Telerik, umieszczony w `%AppData%\Telerik\`. Bez niego komponenty działają,
   ale pokazują znak wodny.

## Uruchomienie

```
dotnet restore
dotnet build
dotnet run
```

W Visual Studio: otwórz `DTECH.IIoT.Viewer.slnx` i naciśnij F5.

## Struktura projektu

```
App.xaml.cs                  start aplikacji, rejestracja serwisów w kontenerze DI
MainWindow.xaml              okno WPF, w nim jedna kontrolka: BlazorWebView
wwwroot/
  index.html                 strona hosta Blazora, wczytuje motyw Telerik
  css/app.css                paleta i klasy powłoki, przeniesione z mocku
Components/
  Routes.razor               router + TelerikRootComponent
  Layout/MainLayout.razor    pasek górny, cztery zakładki, stopka
  Pages/                     cztery ekrany aplikacji
Models/                      klasy danych: kolektor, próbka pomiaru, wynik polecenia
Services/
  AppState.cs                stan wspólny: połączenie i wybrany kolektor
  ConnectionSettingsStore.cs zapis ustawień połączenia (bez hasła)
  SqlDataService.cs          odczyt danych z SQL   <- do zaimplementowania
  CollectorApiClient.cs      sterowanie wyjściami  <- do zaimplementowania
docs/
  plan-praktyk.md            plan czterech tygodni
  srodowisko.md              dane testowej bazy i API (wypełnia opiekun)
```

## Co już działa

- Okno aplikacji z czterema ekranami i przełączaniem między nimi.
- Kontener DI: serwisy wstrzykiwane do komponentów przez `@inject`.
- Komponenty Telerik (przykład na ekranie 1).
- Zapis i odczyt ustawień połączenia w `%AppData%\DTECH\IIoT.Viewer\connection.json`.
- Log nieobsłużonych błędów w `%LocalAppData%\DTECH\IIoT.Viewer\crash.log`.

## Co jest do zrobienia

Wszystko, co oznaczono komentarzem `TODO` oraz blokami „do zrobienia” widocznymi
na ekranach. Kolejność i zakres: `docs/plan-praktyk.md`.

## Zasady pracy

- **Nigdy nie commituj haseł ani rzeczywistych connection stringów.**
  Plik `.gitignore` blokuje typowe przypadki, ale odpowiedzialność jest twoja.
- **Program tylko czyta z bazy.** Żadnych `INSERT`, `UPDATE`, `DELETE`.
- **Sterowanie wyjściami testuj wyłącznie na kolektorze testowym**,
  niepodłączonym do maszyn. Polecenia przełączają fizyczne wyjścia.
- Wartości w zapytaniach przekazuj przez parametry, nigdy przez sklejanie stringów.
- Commituj małymi krokami, na koniec każdego dnia praktyk minimum jeden commit.

## Uwagi techniczne

- `TargetFramework` to `net10.0-windows10.0.19041.0`. Wersja Windows SDK jest
  wymagana: kontrolka WebView2 używa API kompozycji WinRT i bez niej aplikacja
  wywala się przy starcie.
- Pliki źródłowe zapisujemy w **UTF-8 z BOM**. Bez BOM kompilator Razor czyta
  polskie znaki jako inną stronę kodową i w interfejsie pojawiają się krzaki.
