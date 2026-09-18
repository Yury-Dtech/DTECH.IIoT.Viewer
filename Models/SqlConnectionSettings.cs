namespace DTECH.IIoT.Viewer.Models;

/// <summary>
/// Sposób uwierzytelniania w SQL Server.
/// </summary>
public enum SqlAuthMode
{
    /// <summary>Uwierzytelnianie Windows (konto zalogowanego użytkownika).</summary>
    Windows,

    /// <summary>Uwierzytelnianie SQL Server (login + hasło).</summary>
    SqlServer
}

/// <summary>
/// Ustawienia połączenia z bazą danych (ekran 1).
///
/// UWAGA: hasło jest polem tylko na czas działania programu.
/// Nigdy nie zapisujemy go na dysku - patrz <see cref="Services.ConnectionSettingsStore"/>.
/// </summary>
public class SqlConnectionSettings
{
    /// <summary>Serwer lub instancja, np. SRV-SQL\INSTANCE.</summary>
    public string Server { get; set; } = string.Empty;

    /// <summary>Nazwa bazy danych.</summary>
    public string Database { get; set; } = string.Empty;

    /// <summary>Wybrany sposób uwierzytelniania.</summary>
    public SqlAuthMode AuthMode { get; set; } = SqlAuthMode.Windows;

    /// <summary>Login SQL - używany tylko przy <see cref="SqlAuthMode.SqlServer"/>.</summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>Hasło SQL - tylko w pamięci, nie jest zapisywane.</summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>Czy zapamiętać ustawienia (bez hasła) na tym komputerze.</summary>
    public bool RememberSettings { get; set; } = true;

    /// <summary>
    /// Kopia ustawień bez hasła - tę wersję wolno zapisać na dysku.
    /// </summary>
    public SqlConnectionSettings WithoutPassword() => new()
    {
        Server = Server,
        Database = Database,
        AuthMode = AuthMode,
        Login = Login,
        Password = string.Empty,
        RememberSettings = RememberSettings
    };

    /// <summary>
    /// Buduje connection string na podstawie ustawień.
    ///
    /// TODO (dzień 3): sprawdź w debuggerze, jak wygląda gotowy connection string
    /// dla obu trybów uwierzytelniania. Zwróć uwagę, dlaczego TrustServerCertificate
    /// jest potrzebne na testowym serwerze bez zaufanego certyfikatu.
    /// </summary>
    public string BuildConnectionString()
    {
        var builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder
        {
            DataSource = Server,
            InitialCatalog = Database,
            TrustServerCertificate = true,
            ConnectTimeout = 10,
            ApplicationName = "DTECH.IIoT.Viewer"
        };

        if (AuthMode == SqlAuthMode.Windows)
        {
            builder.IntegratedSecurity = true;
        }
        else
        {
            builder.UserID = Login;
            builder.Password = Password;
        }

        return builder.ConnectionString;
    }
}
