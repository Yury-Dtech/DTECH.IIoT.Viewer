using System.Data;
using Microsoft.Data.SqlClient;
using DTECH.IIoT.Viewer.Models;

namespace DTECH.IIoT.Viewer.Services;

/// <summary>
/// Cały odczyt danych z SQL Server w jednym miejscu.
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
    /// </summary>
    private string CurrentConnectionString =>
        _appState.Connection?.BuildConnectionString()
        ?? throw new InvalidOperationException(
            "Brak aktywnego połączenia. Najpierw wykonaj test połączenia na ekranie 1.");

    /// <summary>
    /// Sprawdza, czy z podanymi ustawieniami da się połączyć z bazą.
    /// </summary>
    public async Task<(bool Success, string Message)> TestConnectionAsync(
        SqlConnectionSettings settings,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var connectionString = settings.BuildConnectionString();
            using var connection = new SqlConnection(connectionString);

            await connection.OpenAsync(cancellationToken);

            return (
                Success: true,
                Message: $"Połączono pomyślnie z serwerem '{settings.Server}' i bazą '{settings.Database}'."
            );
        }
        catch (SqlException ex)
        {
            string userFriendlyMessage = ex.Number switch
            {
                18456 => "Nieprawidłowa nazwa użytkownika lub hasło SQL Server.",
                53 or -2 => "Serwer SQL jest niedostępny lub nie odpowiada (sprawdź adres i połączenie sieciowe).",
                4060 => $"Baza danych '{settings.Database}' nie istnieje na wskazanym serwerze.",
                _ => $"Błąd SQL Server (Kod {ex.Number}): {ex.Message}"
            };

            return (Success: false, Message: userFriendlyMessage);
        }
        catch (Exception ex)
        {
            return (
                Success: false,
                Message: $"Wystąpił nieoczekiwany błąd podczas próby połączenia: {ex.Message}"
            );
        }
    }

    /// <summary>
    /// Pobiera listę kolektorów wraz z czasem ostatniego pomiaru.
    /// </summary>
    public async Task<IReadOnlyList<Collector>> GetCollectorsAsync(
        CancellationToken cancellationToken = default)
    {
        var result = new List<Collector>();

        const string sql = @"
            SELECT 
                Id, 
                Name, 
                SerialNo, 
                IP AS IpAddress, 
                PingDate AS LastDataTime
            FROM [dbo].[Device] WITH (NOLOCK)
            ORDER BY Id ASC";

        using var connection = new SqlConnection(CurrentConnectionString);
        await connection.OpenAsync(cancellationToken);

        using var command = new SqlCommand(sql, connection);
        using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            int id = reader.GetInt32(0);
            string name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
            string serialNumber = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
            string ipAddress = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
            DateTime? lastDataTime = reader.IsDBNull(4) ? null : reader.GetDateTime(4);


            result.Add(new Collector
            {
                Id = id,
                Name = name,
                SerialNumber = serialNumber,
                IpAddress = ipAddress,
                LastSampleAt = lastDataTime
            });
        }
        ;


        return result;
    }

    /// <summary>
    /// Pobiera przebiegi wszystkich wejść GIP wybranego kolektora w zadanym zakresie czasu.
    /// </summary>
    public async Task<IReadOnlyList<MeasurementSeries>> GetGipSeriesAsync(
        int collectorId,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default)
    {
        const string sql = @"
SELECT 
                DeviceTime, 
                Input1State, 
                Input2State, 
                Input3State, 
                Input4State, 
                Input5State, 
                Input6State 
            FROM [gip].[Data] WITH (NOLOCK)
            WHERE DeviceId = @CollectorId 
              AND DeviceTime >= @FromTime 
              AND DeviceTime <= @ToTime
            ORDER BY DeviceTime ASC";

        var samplesByChannel = new Dictionary<string, List<MeasurementSample>>();

        using var connection = new SqlConnection(CurrentConnectionString);
        await connection.OpenAsync(cancellationToken);

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@CollectorId", collectorId);
        command.Parameters.AddWithValue("@FromTime", from);
        command.Parameters.AddWithValue("@ToTime", to);

        using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            string channelName = reader.IsDBNull(0) ? "GIP" : reader.GetString(0);
            DateTime timestamp = reader.GetDateTime(1);
            double value = reader.GetDouble(2);

            if (!samplesByChannel.TryGetValue(channelName, out var samples))
            {
                samples = new List<MeasurementSample>();
                samplesByChannel[channelName] = samples;
            }

            samples.Add(new MeasurementSample(timestamp, value));
        }

        return samplesByChannel
            .Select(kvp => new MeasurementSeries(kvp.Key, kvp.Value))
            .ToList();
    }

    /// <summary>
    /// Pobiera przebieg energii: moc w kW albo licznik w kWh.
    /// </summary>
    public async Task<MeasurementSeries> GetEnergySeriesAsync(
            int collectorId,
            DateTime from,
            DateTime to,
            bool asPower,
            CancellationToken cancellationToken = default)
    {
        string columnName = asPower ? "MocCzynna" : "EnergiaCzynna";
        string seriesName = asPower ? "Moc [kW]" : "Licznik [kWh]";

        string sql = $@"
            SELECT 
                DeviceTime, 
                {columnName} 
            FROM [le].[Data] WITH (NOLOCK)
            WHERE DeviceId = @CollectorId 
              AND DeviceTime >= @FromTime 
              AND DeviceTime <= @ToTime
            ORDER BY DeviceTime ASC";

        var samples = new List<MeasurementSample>();

        using var connection = new SqlConnection(CurrentConnectionString);
        await connection.OpenAsync(cancellationToken);

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@CollectorId", collectorId);
        command.Parameters.AddWithValue("@FromTime", from);
        command.Parameters.AddWithValue("@ToTime", to);

        using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            DateTime timestamp = reader.GetDateTime(0);
            double value = reader.IsDBNull(1) ? 0.0 : Convert.ToDouble(reader.GetValue(1));

            samples.Add(new MeasurementSample(timestamp, value));
        }

        return new MeasurementSeries(seriesName, samples);
    }
}
