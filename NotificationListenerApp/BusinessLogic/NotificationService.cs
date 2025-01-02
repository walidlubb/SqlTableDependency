using System.Threading.Channels;
using NotificationApp.DataAccess.Interfaces;

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
        _provider.Subscribe("General");
        foreach (var channel in _channels)
        {
            channel.ValidateConfig();
        }
    }

    public void HandleNotification(Notification notification)
    {
        _router.Route(notification);

        foreach (var channel in _channels)
        {
            channel.Send(notification);
        }
    }
}
