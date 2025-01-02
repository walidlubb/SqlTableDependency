
using NotificationApp.DataAccess.Interfaces;

public class RedisProvider : INotificationProvider
{
    public void Subscribe(string topic) => Console.WriteLine($"Subscribed to {topic} via Redis.");
    public void Unsubscribe(string topic) => Console.WriteLine($"Unsubscribed from {topic} via Redis.");
    public void OnMessageReceived(Action<string> handler) => handler?.Invoke("Message from Redis.");
}
