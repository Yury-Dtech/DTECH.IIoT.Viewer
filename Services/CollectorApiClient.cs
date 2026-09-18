using System.Net.Http;
using DTECH.IIoT.Viewer.Models;

namespace DTECH.IIoT.Viewer.Services;

/// <summary>
/// Komunikacja z API kolektora - ustawianie pojedynczych wyjść (ekran 4).
///
/// ZADANIE PRAKTYKANTA. To osobny kanał niż SQL: baza służy tylko do czytania
/// pomiarów, a polecenia ON/OFF idą prosto do urządzenia po HTTP.
///
/// Zasady bezpieczeństwa - obowiązują bez wyjątków:
///   - jedno polecenie naraz, bez automatycznych ponowień; powtórzone wysłanie
///     to decyzja operatora, nie programu;
///   - przed wysłaniem operator potwierdza urządzenie i numer wyjścia w dialogu;
///   - numeracja wyjść pochodzi z dokumentacji urządzenia, NIE z oznaczeń w mocku;
///   - testujemy wyłącznie na kolektorze testowym, niepodłączonym do maszyn.
///
/// Adres API, format polecenia i sposób potwierdzania stanu podaje opiekun -
/// patrz docs/srodowisko.md.
/// </summary>
public class CollectorApiClient
{
    private readonly HttpClient _httpClient;

    public CollectorApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Ustawia jedno wyjście kolektora w stan ON albo OFF.
    /// </summary>
    /// <param name="collector">Urządzenie docelowe - jego IP wskazuje adres API.</param>
    /// <param name="outputNumber">Numer wyjścia zgodny z dokumentacją urządzenia.</param>
    /// <param name="turnOn">true = ON, false = OFF.</param>
    /// <remarks>
    /// TODO (tydzień 3):
    ///   1. Zbuduj adres żądania na podstawie collector.IpAddress i dokumentacji API.
    ///   2. Wyślij żądanie przez _httpClient z rozsądnym timeoutem.
    ///   3. Sprawdź kod odpowiedzi HTTP oraz treść odpowiedzi urządzenia.
    ///   4. Zmapuj wynik na OutputCommandResult:
    ///        Confirmed - urządzenie potwierdziło oczekiwany stan wyjścia,
    ///        Accepted  - przyjęło polecenie, ale stanu nie potwierdziło,
    ///        Timeout   - brak odpowiedzi (TaskCanceledException),
    ///        Error     - kod błędu HTTP albo błąd zgłoszony przez API.
    ///   5. Złap HttpRequestException i TaskCanceledException osobno - operator
    ///      musi wiedzieć, czy urządzenie odrzuciło polecenie, czy nie odpowiedziało.
    ///      Brak odpowiedzi NIE oznacza, że wyjście się nie przełączyło - tak to opisz.
    /// </remarks>
    public Task<OutputCommandResult> SetOutputAsync(
        Collector collector,
        int outputNumber,
        bool turnOn,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Tydzień 3: ustawianie wyjścia przez API kolektora.");
    }

    /// <summary>
    /// Odczytuje aktualny stan wyjść kolektora, jeśli API to udostępnia.
    /// </summary>
    /// <remarks>
    /// TODO (tydzień 3): sprawdź w dokumentacji API, czy istnieje endpoint
    /// odczytu stanu wyjść. Jeśli nie ma - zostaw tę metodę niezaimplementowaną
    /// i pokaż na ekranie, że stan jest nieznany. Nie wymyślaj stanu, którego
    /// urządzenie nie potwierdza.
    /// </remarks>
    public Task<IReadOnlyDictionary<int, bool>> GetOutputStatesAsync(
        Collector collector,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Tydzień 3: odczyt stanu wyjść (jeśli API to wspiera).");
    }
}
