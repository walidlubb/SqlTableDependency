using NotificationApp.DataAccess.Interfaces;

public class MongoProvider : INotificationProvider
{
    public void Subscribe(string topic) => Console.WriteLine($"Subscribed to {topic} via MongoDB.");
    public void Unsubscribe(string topic) => Console.WriteLine($"Unsubscribed from {topic} via MongoDB.");
    public void OnMessageReceived(Action<string> handler) => handler?.Invoke("Message from MongoDB.");
}
