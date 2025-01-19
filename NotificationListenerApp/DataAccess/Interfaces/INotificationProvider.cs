public interface INotificationProvider
{
    void Subscribe(IEnumerable<Topic> topics);
    void Unsubscribe(IEnumerable<Topic> topics);

    event Action<DependencyHandler> OnMessageReceived;

}