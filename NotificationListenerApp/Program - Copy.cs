using System;
using Serilog;
using System.Threading.Tasks;
using System.Threading.Channels;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

class Program
{
    private static readonly string connectionString = "Data Source=.;Initial Catalog=NotificationsDB;Persist Security Info=True;User ID=NotificationAdmin;Password=123;MultipleActiveResultSets=True";
    private static readonly Channel<Func<Task>> notificationChannel = Channel.CreateUnbounded<Func<Task>>();

    static async Task Main(string[] args)
    {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        // Start the consumer task to process the notification queue
        _ = Task.Run(ConsumeNotificationQueue);

        try
        {
            Log.Information("Starting notification listener...");

            using (var tableDependency = new SqlTableDependency<NotificationsUser>(connectionString, tableName: "NotificationsUsers"))
            {
                tableDependency.OnChanged += OnNotificationTempChanged;
                tableDependency.OnError += Notification_OnError;

                tableDependency.Start();

                Console.WriteLine("Listening for changes... Press any key to exit.");
                Console.ReadKey();

                tableDependency.Stop();
            }
        }
        catch (Exception ex)
        {
            Log.Error("An error occurred: {Error}", ex.Message);
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static async Task ConsumeNotificationQueue()
    {
        await foreach (var task in notificationChannel.Reader.ReadAllAsync())
        {
            try
            {
                await task();
            }
            catch (Exception ex)
            {
                Log.Error("Error processing notification: {Error}", ex.Message);
            }
        }
    }

    private static void OnNotificationTempChanged(object sender, RecordChangedEventArgs<NotificationsUser> e)
    {
        if (e.ChangeType != ChangeType.Insert)
            return;

        var notificationsUser = e.Entity;

        // Enqueue the notification processing task
        notificationChannel.Writer.TryWrite(async () =>
        {
            try
            {
                Log.Information("Processing notification for user: {UserId}", notificationsUser.UserId);

                var notificationService = new NotificationService(connectionString);
                await notificationService.ProcessNotificationAsync(notificationsUser);

                //// Run Scheduled Cleanup
                //var cleaner = new ScheduledCleaner(connectionString);
                //await cleaner.CleanupProcessedRecordsAsync();

                Log.Information("Notification processing completed for user: {UserId}", notificationsUser.UserId);
            }
            catch (Exception ex)
            {
                Log.Error("Error in notification processing task: {Error}", ex.Message);
            }
        });
    }

    private static void Notification_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
    {
        Log.Error("An error occurred with SqlTableDependency: {Error}", e.Error.Message);
    }
}
