using DTECH.IIoT.Viewer.Models;

namespace DTECH.IIoT.Viewer.Services;

/// <summary>
/// Cały odczyt danych z SQL Server w jednym miejscu.
///
/// ZADANIE PRAKTYKANTA. Metody są celowo puste - implementujesz je kolejno
/// według planu praktyk (docs/plan-praktyk.md).
///
/// Zasady:
///   - zapytania piszemy ręcznie jako SELECT ... FROM ... WHERE ... ORDER BY,
///     bez ORM-a; chodzi o to, żebyś widział, co dokładnie jedzie do bazy;
///   - wartości wstawiamy WYŁĄCZNIE przez parametry (cmd.Parameters.AddWithValue),
///     nigdy przez sklejanie stringów - inaczej otwierasz SQL injection;
///   - SqlConnection i SqlCommand trzymamy w using, żeby połączenia się zwalniały;
///   - program tylko czyta; żadnych INSERT, UPDATE ani DELETE.
///
/// Nazwy tabel i kolumn dla tej bazy podaje opiekun - patrz docs/srodowisko.md.
/// </summary>
public class SqlDataService
{
    private readonly AppState _appState;

    public SqlDataService(AppState appState)
    {
        _appState = appState;
    }

    /// <summary>
    /// Connection string aktywnego połączenia, ustawionego na ekranie 1.
    /// Rzuca wyjątek, gdy operator nie przeszedł jeszcze testu połączenia -
    /// to świadoma decyzja: lepiej wyraźny błąd niż ciche puste wyniki.
    /// </summary>
    private string CurrentConnectionString =>
        _appState.Connection?.BuildConnectionString()
        ?? throw new InvalidOperationException(
            "Brak aktywnego połączenia. Najpierw wykonaj test połączenia na ekranie 1.");

    /// <summary>
    /// Sprawdza, czy z podanymi ustawieniami da się połączyć z bazą.
    /// Nie rzuca wyjątków - zwraca opis wyniku do pokazania operatorowi.
    /// </summary>
    /// <remarks>
    /// TODO (dzień 3):
    ///   1. Zbuduj connection string: settings.BuildConnectionString().
    ///   2. Otwórz SqlConnection w bloku using i wywołaj OpenAsync().
    ///   3. Zwróć wynik pozytywny z nazwą serwera i bazy.
    ///   4. Złap SqlException i rozróżnij komunikaty: błędne dane logowania
    ///      (Number 18456) od serwera niedostępnego (Number 53 albo timeout).
    ///   5. Złap też ogólny Exception - inaczej literówka w nazwie serwera
    ///      wywróci całą aplikację.
    /// Komunikat musi być zrozumiały dla operatora, nie surowy ex.ToString().
    /// </remarks>
    public Task<(bool Success, string Message)> TestConnectionAsync(
        SqlConnectionSettings settings,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Dzień 3: test połączenia z SQL Server.");
    }

    /// <summary>
    /// Pobiera listę kolektorów wraz z czasem ostatniego pomiaru.
    /// </summary>
    /// <remarks>
    /// TODO (dzień 4):
    ///   1. Napisz SELECT zwracający id, nazwę, SN, IP i czas ostatniego pomiaru.
    ///   2. Uporządkuj wynik po nazwie (ORDER BY).
    ///   3. Przeczytaj rekordy przez SqlDataReader i zmapuj na obiekty Collector.
    ///   4. Pamiętaj o kolumnach, które mogą być NULL - użyj reader.IsDBNull.
    /// Wyszukiwanie po nazwie, SN i IP (dzień 5) możesz zrobić po stronie C#
    /// na tej liście albo dopisać WHERE - omów z opiekunem, co jest tu sensowniejsze.
    /// </remarks>
    public Task<IReadOnlyList<Collector>> GetCollectorsAsync(
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Dzień 4: pobranie listy kolektorów z bazy.");
    }

    /// <summary>
    /// Pobiera przebiegi wszystkich wejść GIP wybranego kolektora w zadanym zakresie czasu.
    /// Każde wejście to osobna seria rysowana na wspólnej osi czasu.
    /// </summary>
    /// <remarks>
    /// TODO (tydzień 2): SELECT z filtrem po kolektorze i zakresie czasu,
    /// ORDER BY po czasie, a następnie pogrupowanie próbek po numerze wejścia.
    /// Zakres czasu przekazuj jako parametry zapytania, nie jako tekst.
    /// </remarks>
    public Task<IReadOnlyList<MeasurementSeries>> GetGipSeriesAsync(
        int collectorId,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Tydzień 2: przebiegi wejść GIP.");
    }

    /// <summary>
    /// Pobiera przebieg energii: moc w kW albo licznik w kWh.
    /// </summary>
    /// <remarks>
    /// TODO (tydzień 2): jeden zakres czasu wspólny z wykresem GIP.
    /// Jednostki i znaczenie kolumn potwierdź z opiekunem - licznik kWh rośnie
    /// narastająco, moc kW jest wartością chwilową i wymaga innej skali.
    /// </remarks>
    public Task<MeasurementSeries> GetEnergySeriesAsync(
        int collectorId,
        DateTime from,
        DateTime to,
        bool asPower,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Tydzień 2: przebieg energii.");
    }
}
