using System.IO;
using System.Text.Json;
using DTECH.IIoT.Viewer.Models;

namespace DTECH.IIoT.Viewer.Services;

/// <summary>
/// Zapisuje i odczytuje ustawienia połączenia SQL w profilu użytkownika Windows.
///
/// Ta klasa jest gotowa i służy jako wzór stylu kodu: jedna odpowiedzialność,
/// jawne zależności, błędy obsłużone tak, żeby nie wywrócić aplikacji przy starcie.
/// Hasło NIGDY nie trafia na dysk - zapisujemy <see cref="SqlConnectionSettings.WithoutPassword"/>.
/// </summary>
public class ConnectionSettingsStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    private readonly string _filePath;

    public ConnectionSettingsStore()
    {
        var folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "DTECH",
            "IIoT.Viewer");

        _filePath = Path.Combine(folder, "connection.json");
    }

    /// <summary>Ścieżka pliku ustawień - przydatna przy diagnostyce.</summary>
    public string FilePath => _filePath;

    /// <summary>
    /// Odczytuje zapisane ustawienia. Zwraca null, gdy pliku nie ma
    /// lub jest uszkodzony - wtedy formularz startuje z wartościami domyślnymi.
    /// </summary>
    public SqlConnectionSettings? Load()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                return null;
            }

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<SqlConnectionSettings>(json, JsonOptions);
        }
        catch (Exception)
        {
            // Uszkodzony plik ustawień nie może blokować uruchomienia programu.
            return null;
        }
    }

    /// <summary>
    /// Zapisuje ustawienia bez hasła. Gdy użytkownik odznaczył "Zapamiętaj ustawienia",
    /// wcześniej zapisany plik jest usuwany.
    /// </summary>
    public void Save(SqlConnectionSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        try
        {
            if (!settings.RememberSettings)
            {
                Clear();
                return;
            }

            var folder = Path.GetDirectoryName(_filePath);
            if (folder is not null)
            {
                Directory.CreateDirectory(folder);
            }

            var json = JsonSerializer.Serialize(settings.WithoutPassword(), JsonOptions);
            File.WriteAllText(_filePath, json);
        }
        catch (Exception)
        {
            // Brak możliwości zapisu ustawień to niedogodność, nie błąd krytyczny.
        }
    }

    /// <summary>Usuwa zapisane ustawienia.</summary>
    public void Clear()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
        }
        catch (Exception)
        {
            // Ignorujemy - patrz komentarz w Save.
        }
    }
}
