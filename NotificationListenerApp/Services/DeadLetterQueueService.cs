using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Serilog;

public class DeadLetterQueueService
{
    private readonly string _connectionString;

    public DeadLetterQueueService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task LogFailureAsync(long NotificationId, string errorMessage)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                INSERT INTO DeadLetterQueue (NotificationId, ErrorMessage, CreatedAt)
                VALUES (@NotificationId, @ErrorMessage, @CreatedAt)";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NotificationId", NotificationId);
            command.Parameters.AddWithValue("@ErrorMessage", errorMessage);
            command.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);

            await command.ExecuteNonQueryAsync();
            Log.Information("Logged failure for NotificationId: {NotificationId}", NotificationId);
        }
        catch (Exception ex)
        {
            Log.Error("Failed to log dead-letter entry: {Error}", ex.Message);
        }
    }
}
