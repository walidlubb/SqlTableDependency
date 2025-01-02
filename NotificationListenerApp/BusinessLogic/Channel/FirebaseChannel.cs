public class FirebaseChannel : INotificationChannel
{
    public void Send(Notification notification) => Console.WriteLine($"Firebase notification sent: {notification.Topic}");
    public void ValidateConfig() => Console.WriteLine("Firebase config validated.");
    public string GetDeliveryStatus() => "Firebase notification delivered.";
}
