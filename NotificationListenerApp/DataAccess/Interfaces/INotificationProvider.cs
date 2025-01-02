namespace NotificationApp.DataAccess.Interfaces
{
    public interface INotificationProvider
    {
        void Subscribe(string topic);
        void Unsubscribe(string topic);
        void OnMessageReceived(Action<string> handler);
    }
}




