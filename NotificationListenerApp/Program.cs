
using System;
using Serilog;
using Polly;
using System.Threading.Tasks;
using TableDependency.SqlClient;


using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

class Program
{
  static   string connectionString = "Data Source=.;Initial Catalog=NotificationsDB;Persist Security Info=True;User ID=NotificationAdmin;Password=123;MultipleActiveResultSets=True";


    static void Main(string[] args)
    {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        try
        {
            Log.Information("Starting notification listener...");
               using (var tableDependency = new SqlTableDependency<NotificationsUser>(connectionString, tableName: "NotificationsUsers"))
                {
                    tableDependency.OnChanged += OnNotificationTempChanged;
                    tableDependency.OnError += Notification_OnError;

                    tableDependency.Start();

                    Console.WriteLine("Listening for changes... Press a key to exit.");
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

    private static void OnNotificationTempChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<NotificationsUser> e)
    {
        if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.Insert)
            return;

        var NotificationsUser = e.Entity;

        Task.Run(async () =>
        {
            var notificationService = new NotificationService(connectionString);
            await notificationService.ProcessNotificationAsync(NotificationsUser);

            // Run Scheduled Cleanup
            var cleaner = new ScheduledCleaner(connectionString);
            await cleaner.CleanupProcessedRecordsAsync();
        });
    }

    private static void Notification_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
    {
        Log.Error("An error occurred with SqlTableDependency: {Error}", e.Error.Message);
    }
}
