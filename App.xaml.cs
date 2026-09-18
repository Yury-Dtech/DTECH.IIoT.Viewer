using System.IO;
using System.Windows;
using System.Windows.Threading;
using DTECH.IIoT.Viewer.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DTECH.IIoT.Viewer;

/// <summary>
/// Punkt startowy aplikacji WPF.
///
/// Tutaj budujemy kontener zależności (DI). Wszystkie serwisy rejestrujemy w jednym
/// miejscu, a komponenty Razor dostają je przez @inject - nie tworzymy ich ręcznie
/// w środku stron.
/// </summary>
public partial class App : Application
{
    private ServiceProvider? _services;

    /// <summary>
    /// Plik, do którego trafiają nieobsłużone błędy. Gdy aplikacja zamknie się
    /// bez komunikatu, zajrzyj tutaj w pierwszej kolejności.
    /// </summary>
    public static string CrashLogPath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "DTECH",
        "IIoT.Viewer",
        "crash.log");

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Bez tego nieobsłużony wyjątek zamyka okno bez żadnego śladu.
        DispatcherUnhandledException += OnDispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += OnDomainUnhandledException;

        var services = new ServiceCollection();
        ConfigureServices(services);
        _services = services.BuildServiceProvider();

        var mainWindow = new MainWindow(_services);
        mainWindow.Show();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Obsługa komponentów Razor w oknie WPF.
        services.AddWpfBlazorWebView();

        // Komponenty Telerik UI for Blazor.
        services.AddTelerikBlazor();

        // Stan wspólny dla wszystkich ekranów: połączenie i wybrany kolektor.
        // Singleton, bo aplikacja to jedno okno i jeden użytkownik.
        services.AddSingleton<AppState>();

        // Zapis ustawień połączenia w profilu użytkownika (bez hasła).
        services.AddSingleton<ConnectionSettingsStore>();

        // Odczyt danych z SQL Server.
        services.AddSingleton<SqlDataService>();

        // Klient API kolektora. IHttpClientFactory pilnuje poprawnego
        // zarządzania połączeniami HTTP - nie tworzymy HttpClient ręcznie.
        services.AddHttpClient<CollectorApiClient>(client =>
        {
            // Polecenie musi się zakończyć wynikiem albo timeoutem, bez zawieszania UI.
            client.Timeout = TimeSpan.FromSeconds(10);
        });

#if DEBUG
        // Narzędzia developerskie w WebView (menu kontekstowe, konsola przeglądarki).
        services.AddBlazorWebViewDeveloperTools();
#endif
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        var exception = e.Exception;
        LogCrash(exception);
        var message = string.Join(
            Environment.NewLine + Environment.NewLine,
            "Wystąpił nieobsłużony błąd:",
            exception.Message,
            $"Szczegóły zapisano w pliku: {CrashLogPath}");

        MessageBox.Show(message, "D-TECH IIoT Viewer", MessageBoxButton.OK, MessageBoxImage.Error);

        // Błąd został pokazany operatorowi - nie zamykamy z tego powodu aplikacji.
        e.Handled = true;
    }

    private void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception exception)
        {
            LogCrash(exception);
        }
    }

    private static void LogCrash(Exception exception)
    {
        try
        {
            var folder = Path.GetDirectoryName(CrashLogPath);
            if (folder is not null)
            {
                Directory.CreateDirectory(folder);
            }

            File.AppendAllText(
                CrashLogPath,
                $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}{Environment.NewLine}{exception}{Environment.NewLine}{Environment.NewLine}");
        }
        catch (Exception)
        {
            // Nie ma gdzie zgłosić błędu logowania błędu.
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _services?.Dispose();
        base.OnExit(e);
    }
}
