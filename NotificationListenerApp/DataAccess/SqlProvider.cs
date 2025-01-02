
using System.Data.SqlClient;
using NotificationApp.DataAccess.Interfaces;

using System;
using System.Data.SqlClient;
using System.Threading;

public class SqlProvider : INotificationProvider
{
    private readonly string _connectionString;
    private readonly DependencyHandler _dependencyHandler;

    public SqlProvider(string connectionString, DependencyHandler dependencyHandler)
    {
        _connectionString = connectionString;
        _dependencyHandler = dependencyHandler;
    }

    public void Subscribe(string topic)
    {
        Console.WriteLine($"Subscribed to {topic} via SQL.");
        StartSqlDependency(topic);
    }

    public void Unsubscribe(string topic)
    {
        Console.WriteLine($"Unsubscribed from {topic} via SQL.");
        // Add logic to stop listening to the topic if required.
    }

    public void OnMessageReceived(Action<string> handler)
    {
        _dependencyHandler.OnChange += (sender, e) =>
        {
            if (e.Info != SqlNotificationInfo.Invalid)
            {
                handler?.Invoke($"Data change detected: {e.Info}");
                Subscribe(e.Source); // Re-subscribe for continuous notifications
            }
        };
    }

    private void StartSqlDependency(string topic)
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlDependency.Start(_connectionString);

                var command = new SqlCommand($"SELECT * FROM {topic}", connection);
                var sqlDependency = new SqlDependency(command);

                sqlDependency.OnChange += (sender, e) =>
                {
                    Console.WriteLine($"SQL Notification: {e.Type} - {e.Info}");
                    _dependencyHandler.HandleDependencyChange(e);
                };

                // Execute the command to establish dependency
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Optionally process initial data
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error starting SQL Dependency: {ex.Message}");
        }
    }
}

public class DependencyHandler
{
    public event OnChangeEventHandler OnChange;

    public void HandleDependencyChange(SqlNotificationEventArgs args)
    {
        OnChange?.Invoke(this, args);
    }
}
