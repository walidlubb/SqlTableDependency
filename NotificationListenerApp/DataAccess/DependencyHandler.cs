using System.Data.SqlClient;

public class DependencyHandler
{
    private readonly string _connectionString;

    public DependencyHandler(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public SqlConnection GetSqlConnection()
    {
        // Create and return a new SQL connection
        return new SqlConnection(_connectionString);
    }

    public void ValidateConnection()
    {
        using var connection = GetSqlConnection();
        try
        {
            connection.Open();
            Console.WriteLine("SQL Connection validated successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SQL Connection validation failed: {ex.Message}");
            throw;
        }
    }
}
