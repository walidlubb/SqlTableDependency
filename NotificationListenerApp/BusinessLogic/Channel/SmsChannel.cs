public class SignalRChannel : INotificationChannel
{
    public void Send(Notification notification) => Console.WriteLine($"SignalR notification sent: {notification.Topic}");
    public void ValidateConfig() => Console.WriteLine("SignalR config validated.");
    public string GetDeliveryStatus() => "SignalR notification delivered.";
}
