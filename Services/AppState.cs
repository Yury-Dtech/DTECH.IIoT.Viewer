using DTECH.IIoT.Viewer.Models;

namespace DTECH.IIoT.Viewer.Services;

/// <summary>
/// Wspólny stan aplikacji: aktywne połączenie SQL i wybrany kolektor.
///
/// Dzięki tej klasie wybór kolektora zrobiony na ekranie 2 jest widoczny
/// na ekranach 3 i 4. Rejestrujemy ją jako singleton - w Blazor Hybrid
/// cała aplikacja to jedno okno i jeden użytkownik.
///
/// Komponenty, które mają reagować na zmianę stanu, subskrybują
/// <see cref="Changed"/> i wywołują StateHasChanged.
/// </summary>
public class AppState
{
    private SqlConnectionSettings? _connection;
    private Collector? _selectedCollector;

    /// <summary>Zgłaszane przy każdej zmianie stanu.</summary>
    public event Action? Changed;

    /// <summary>
    /// Ustawienia połączenia potwierdzone udanym testem na ekranie 1.
    /// Null oznacza, że aplikacja nie ma jeszcze źródła danych.
    /// </summary>
    public SqlConnectionSettings? Connection
    {
        get => _connection;
        set
        {
            _connection = value;
            NotifyChanged();
        }
    }

    /// <summary>Kolektor wybrany na ekranie 2.</summary>
    public Collector? SelectedCollector
    {
        get => _selectedCollector;
        set
        {
            _selectedCollector = value;
            NotifyChanged();
        }
    }

    /// <summary>Czy aplikacja ma sprawdzone połączenie z bazą.</summary>
    public bool IsConnected => _connection is not null;

    /// <summary>Czy operator wybrał kolektor - warunek wejścia na ekrany 3 i 4.</summary>
    public bool HasSelectedCollector => _selectedCollector is not null;

    private void NotifyChanged() => Changed?.Invoke();
}
