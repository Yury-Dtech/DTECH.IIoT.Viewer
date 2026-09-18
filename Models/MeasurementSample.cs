namespace DTECH.IIoT.Viewer.Models;

/// <summary>
/// Jedna próbka pomiarowa: czas i wartość (ekran 3).
///
/// Używana zarówno dla wejść GIP (wartość 0 lub 1), jak i dla energii
/// (moc w kW albo licznik w kWh).
///
/// TODO (dzień 2): potwierdź z opiekunem, w jakiej strefie czasowej baza
/// przechowuje znacznik czasu (czas lokalny czy UTC) - od tego zależy,
/// czy wykresy pokażą właściwe godziny.
/// </summary>
/// <param name="Timestamp">Czas pomiaru.</param>
/// <param name="Value">Wartość pomiaru.</param>
public record MeasurementSample(DateTime Timestamp, double Value);

/// <summary>
/// Kompletny przebieg jednego kanału - nazwa kanału i jego próbki.
/// Wszystkie wejścia GIP rysujemy razem, więc ekran 3 pobiera listę takich serii.
/// </summary>
/// <param name="ChannelName">Nazwa lub numer kanału, np. "GIP2 - wejście 3".</param>
/// <param name="Samples">Próbki uporządkowane rosnąco po czasie.</param>
public record MeasurementSeries(string ChannelName, IReadOnlyList<MeasurementSample> Samples);
