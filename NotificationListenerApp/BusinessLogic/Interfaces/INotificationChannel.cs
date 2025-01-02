public interface INotificationChannel
{
    void Send(Notification notification);
    void ValidateConfig();
    string GetDeliveryStatus();
}