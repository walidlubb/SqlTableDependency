public class NotificationRouter : INotificationRouter
{
    private readonly NotificationRuleEngine _ruleEngine;

    public NotificationRouter(NotificationRuleEngine ruleEngine)
    {
        _ruleEngine = ruleEngine;
    }

    public void Route(Notification notification)
    {
        Console.WriteLine($"Routing notification with ID: {notification.Id}");
        _ruleEngine.EvaluateRules(notification);
    }

    public bool ValidateRouting() => true;

   
}
