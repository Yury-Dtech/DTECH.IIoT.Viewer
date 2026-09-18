using System.Windows;

namespace DTECH.IIoT.Viewer;

/// <summary>
/// Okno główne. Jego jedyną zawartością jest BlazorWebView, który hostuje
/// komponenty Razor. Cała logika ekranów znajduje się w folderze Components.
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(IServiceProvider services)
    {
        InitializeComponent();

        // BlazorWebView korzysta z tego samego kontenera, co reszta aplikacji.
        BlazorWebViewHost.Services = services;
    }
}
