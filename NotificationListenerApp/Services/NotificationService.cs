
using Serilog;
using Polly;
using Polly.Retry;
using System;
using System.Threading.Tasks;
using System.Data.SqlClient;

public class NotificationService
{
    private readonly AsyncRetryPolicy _retryPolicy;
    private readonly DeadLetterQueueService _deadLetterQueue;

    public NotificationService(string connectionString)
    {
        // Configure retry policy with exponential backoff
        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (exception, timeSpan, retryCount, context) =>
                {
                    Log.Warning("Retry {RetryCount} after {TimeSpan} due to {Error}", retryCount, timeSpan, exception.Message);
                });

        _deadLetterQueue = new DeadLetterQueueService(connectionString);
    }

    public async Task ProcessNotificationAsync(NotificationsUser NotificationsUser)
    {
        try
        {
            // Process the notification with retry
            await _retryPolicy.ExecuteAsync(() => SendNotificationAsync(NotificationsUser));
            Log.Information("Notification {TIME} successfully sent for UserEmail: {UserEmail}",DateTime.Now, NotificationsUser.UserEmail);
        }
        catch (Exception ex)
        {
            Log.Error("Failed to send notification for UserEmail: {UserEmail}. Error: {Error}", NotificationsUser.UserEmail, ex.Message);
            // Here, you can push the notification to a dead-letter queue or log to a database for manual processing.
            await _deadLetterQueue.LogFailureAsync(NotificationsUser.Id, ex.Message);
        }
    }

    private Task SendNotificationAsync(NotificationsUser NotificationsUser)
    {
        // Simulate notification delivery
        Log.Information("Sending notification for UserEmail: {UserEmail}", NotificationsUser.UserEmail);

        //// Simulate a potential failure
        //if (new Random().Next(1, 4) == 1)
        //{
        //    throw new Exception("Simulated delivery failure.");
        //}

        // Simulate success
        return Task.CompletedTask;
    }
}
