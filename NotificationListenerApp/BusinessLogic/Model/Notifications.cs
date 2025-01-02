public class Notification
{
    public string Id { get; set; }
    public string Topic { get; set; }
    public object Payload { get; set; }
    public Dictionary<string, string> Metadata { get; set; }
    public string Priority { get; set; }
    public DateTime Expiry { get; set; }
}