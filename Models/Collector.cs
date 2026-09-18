namespace DTECH.IIoT.Viewer.Models;

/// <summary>
/// Kolektor danych - urządzenie zbierające pomiary z maszyny (ekran 2).
///
/// TODO (dzień 2): dopasuj właściwości do rzeczywistych kolumn tabeli kolektorów
/// w testowej bazie. Nazwy kolumn i ich znaczenie wskazuje opiekun -
/// patrz docs/srodowisko.md.
/// </summary>
public class Collector
{
    /// <summary>Identyfikator kolektora w bazie - klucz używany w zapytaniach o pomiary.</summary>
    public int Id { get; set; }

    /// <summary>Nazwa opisowa, widoczna dla operatora.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Numer seryjny urządzenia.</summary>
    public string SerialNumber { get; set; } = string.Empty;

    /// <summary>Adres IP kolektora - używany także przez API sterowania wyjściami.</summary>
    public string IpAddress { get; set; } = string.Empty;

    /// <summary>Czas ostatniego pomiaru zapisanego w bazie. Null, gdy brak danych.</summary>
    public DateTime? LastSampleAt { get; set; }

    /// <summary>
    /// Wiek najnowszych danych. Służy do oceny, czy kolektor jest aktualny.
    /// </summary>
    public TimeSpan? DataAge => LastSampleAt is null ? null : DateTime.Now - LastSampleAt.Value;

    /// <summary>
    /// Dane uznajemy za aktualne, gdy ostatni pomiar jest nie starszy niż 5 minut.
    ///
    /// TODO (dzień 5): ustal z opiekunem właściwy próg dla testowej bazy -
    /// zależy od częstotliwości zapisu pomiarów.
    /// </summary>
    public bool IsDataFresh => DataAge is not null && DataAge.Value < TimeSpan.FromMinutes(5);
}
