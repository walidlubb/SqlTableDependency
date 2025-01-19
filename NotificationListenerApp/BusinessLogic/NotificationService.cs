
public class NotificationService
{
    private readonly INotificationProvider _provider;
    private readonly IEnumerable<INotificationChannel> _channels;
    private readonly NotificationRouter _router;

    public NotificationService(
        INotificationProvider provider,
        IEnumerable<INotificationChannel> channels,
        NotificationRouter router)
    {
        _provider = provider;
        _channels = channels;
        _router = router;
    }

    public void Initialize()
    {
       
        _provider.OnMessageReceived += (handler) =>
        {
            HandleNotification(handler.Message);
         
        };
        // Example of calling the Subscribe method
        var topics = new List<Topic> { new Topic { Name = "NotificationsUsers" } };
        _provider.Subscribe(topics);

        //foreach (var channel in _channels)
        //{
        //    channel.ValidateConfig();
        //}
    }

    public void HandleNotification(Message Message)
    {
        //_router.Route(notification);

        //foreach (var channel in _channels)
        //{
        //    channel.Send(notification);
        //}

        Console.WriteLine("Received message: ");
        Console.WriteLine($"Message ID: {Message.Id}");
        Console.WriteLine($"Message Title: {Message.Title}");
        Console.WriteLine($"Message Description: {Message.Description}");
        Console.WriteLine($"Message Source: {Message.SourceMessage}");
    }
}
