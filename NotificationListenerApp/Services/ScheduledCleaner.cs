using System;
using  System.Data.SqlClient;
using System.Threading.Tasks;
using Serilog;

public class ScheduledCleaner
{
    private readonly string _connectionString;

    public ScheduledCleaner(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task CleanupProcessedRecordsAsync()
    {
        return;
      
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                DELETE FROM NotificationsUsers
                WHERE  CreatedDate < DATEADD(day, -1, GETDATE())";

            using var command = new SqlCommand(query, connection);
            int rowsAffected = await command.ExecuteNonQueryAsync();

            Log.Information("Cleaned up {RowsAffected} processed records from NotificationsUsers.", rowsAffected);
        }
        catch (Exception ex)
        {
            Log.Error("Failed to clean processed records: {Error}", ex.Message);
        }
    }
}