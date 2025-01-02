
public class EmailChannel : INotificationChannel
{
    public void Send(Notification notification) => Console.WriteLine($"Email sent: {notification.Topic}");
    public void ValidateConfig() => Console.WriteLine("Email config validated.");
    public string GetDeliveryStatus() => "Email delivered.";
}
