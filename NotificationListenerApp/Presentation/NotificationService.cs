
using Microsoft.Extensions.DependencyInjection;
using NotificationApp.DataAccess.Interfaces;

class Program
{
    static void Main(string[] args)
    {
        // Configure DI
        var serviceProvider = new ServiceCollection()
            .AddSingleton<INotificationProvider, RedisProvider>() // Ì„ﬂ‰ﬂ «· »œÌ· ≈·Ï MongoProvider √Ê SqlProvider
            .AddSingleton<INotificationChannel, EmailChannel>()
            .AddSingleton<INotificationChannel, SmsChannel>()
            .AddSingleton<INotificationChannel, FirebaseChannel>()
            .AddSingleton<INotificationChannel, SignalRChannel>()
            .AddSingleton<NotificationRuleEngine>()
            .AddSingleton<NotificationRouter>()
            .AddSingleton<NotificationService>()
            .BuildServiceProvider();

        // Resolve and use the NotificationService
        var notificationService = serviceProvider.GetService<NotificationService>();
        notificationService.Initialize();

        var notification = new Notification
        {
            Id = "1",
            Topic = "System Alert",
            Payload = "Critical system update required!",
            Metadata = new Dictionary<string, string> { { "Urgency", "High" } },
            Priority = "High",
            Expiry = DateTime.Now.AddMinutes(5)
        };

        notificationService.HandleNotification(notification);
    }
}