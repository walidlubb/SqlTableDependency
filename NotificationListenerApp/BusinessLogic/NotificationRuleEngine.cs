public class NotificationRuleEngine :INotificationRuleEngine
{
    private readonly List<string> _rules = new();

    public void EvaluateRules(Notification notification)
    {
        Console.WriteLine($"Evaluating rules for notification: {notification.Topic}");
        // Logic to evaluate rules
    }

    public void AddRule(string rule)
    {
        _rules.Add(rule);
        Console.WriteLine($"Rule added: {rule}");
    }

    public void RemoveRule(string ruleId)
    {
        _rules.Remove(ruleId);
        Console.WriteLine($"Rule removed: {ruleId}");
    }

    public void voidAddRule(string rule)
    {
        throw new NotImplementedException();
    }
}
