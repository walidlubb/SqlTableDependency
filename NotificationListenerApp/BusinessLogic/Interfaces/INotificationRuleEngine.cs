public interface INotificationRuleEngine
{
    void EvaluateRules(Notification notification);
    void AddRule(string rule);
    void RemoveRule(string ruleId);
}