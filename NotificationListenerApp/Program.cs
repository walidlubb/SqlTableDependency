using Microsoft.Extensions.DependencyInjection;
using NotificationApp.DataAccess.Interfaces;

class Program
{
    static void Main(string[] args)
    {
        // Configure DI
     

        var serviceProvider = new ServiceCollection()
            .AddSingleton(new DependencyHandler("YourConnectionStringHere")) // Provide connection string
            .AddSingleton<INotificationProvider, SqlProvider>() // Use SqlProvider
            .AddSingleton<INotificationChannel, EmailChannel>()
            .AddSingleton<NotificationService>()
            .BuildServiceProvider();

        // Resolve and use the NotificationService
        var notificationService = serviceProvider.GetService<NotificationService>();
        notificationService.Initialize();

        var notification = new Notification
        {
            Id = "1",
            Topic = "Welcome",
            Payload = "Hello, World!",
            Metadata = new Dictionary<string, string> { { "Key", "Value" } },
            Priority = "High",
            Expiry = DateTime.Now.AddMinutes(10)
        };

        notificationService.HandleNotification(notification);
    }
}
