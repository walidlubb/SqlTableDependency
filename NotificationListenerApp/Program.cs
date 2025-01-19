using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main(string[] args)
    {
        // Configure DI
     

        //var serviceProvider = new ServiceCollection()
        //    .AddSingleton(new DependencyHandler("YourConnectionStringHere")) // Provide connection string
        //    .AddSingleton<INotificationProvider, SqlDependencyProvider>() // Use SqlProvider
        //    .AddSingleton<INotificationChannel, EmailChannel>()
        //    .AddSingleton<NotificationService>()
        //    .BuildServiceProvider();

        var serviceProvider = new ServiceCollection()
                  .AddSingleton<INotificationProvider>(provider =>
                   {
                       // Replace with your actual connection string
                       var connectionString = "Data Source=.;Initial Catalog=NotificationsDB;Persist Security Info=True;User ID=NotificationAdmin;Password=123;MultipleActiveResultSets=True";
                       return new SqlDependencyProvider(connectionString);
                   })
                   .AddSingleton<INotificationChannel, EmailChannel>()
                    .AddSingleton<NotificationRuleEngine>()
                     .AddSingleton<NotificationRouter>()
                   .AddSingleton<NotificationService>()
                   .BuildServiceProvider();

        // Resolve and use the NotificationService
        var notificationService = serviceProvider.GetService<NotificationService>();
        notificationService.Initialize();

        //var notification = new Notification
        //{
        //    Id = "1",
        //    Topic = "Welcome",
        //    Payload = "Hello, World!",
        //    Metadata = new Dictionary<string, string> { { "Key", "Value" } },
        //    Priority = "High",
        //    Expiry = DateTime.Now.AddMinutes(10)
        //};

        //notificationService.HandleNotification(notification);
    }


}
