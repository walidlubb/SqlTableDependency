public class SmsChannel : INotificationChannel
{
    public void Send(Notification notification) => Console.WriteLine($"SMS sent: {notification.Topic}");
    public void ValidateConfig() => Console.WriteLine("SMS config validated.");
    public string GetDeliveryStatus() => "SMS delivered.";
}