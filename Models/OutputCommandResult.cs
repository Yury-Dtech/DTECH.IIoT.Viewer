namespace DTECH.IIoT.Viewer.Models;

/// <summary>
/// Jak zakończyło się wysłanie polecenia do wyjścia kolektora (ekran 4).
/// </summary>
public enum OutputCommandStatus
{
    /// <summary>Urządzenie potwierdziło polecenie i zgłosiło oczekiwany stan wyjścia.</summary>
    Confirmed,

    /// <summary>Urządzenie przyjęło polecenie, ale nie potwierdziło stanu wyjścia.</summary>
    Accepted,

    /// <summary>Brak odpowiedzi w zadanym czasie.</summary>
    Timeout,

    /// <summary>API zwróciło błąd.</summary>
    Error
}

/// <summary>
/// Wynik jednego polecenia ON/OFF. Jedno polecenie naraz, bez automatycznych ponowień.
/// </summary>
/// <param name="Status">Status wykonania.</param>
/// <param name="Message">Komunikat do pokazania operatorowi.</param>
/// <param name="ReportedState">Stan wyjścia zgłoszony przez urządzenie, jeśli znany.</param>
/// <param name="CompletedAt">Czas zakończenia operacji.</param>
public record OutputCommandResult(
    OutputCommandStatus Status,
    string Message,
    bool? ReportedState,
    DateTime CompletedAt);
