public class SqlDependencyProvider : INotificationProvider
{
    private readonly string _connectionString;
    private readonly List<object> _dependencies = new List<object>();

   
    public event Action<DependencyHandler> OnMessageReceived;

    public SqlDependencyProvider(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Subscribe(IEnumerable<Topic> topics)
    {
        foreach (var topic in topics)
        {
            var dependency = CreateDependency(topic);
            _dependencies.Add(dependency);
        }
    }

    public void Unsubscribe(IEnumerable<Topic> topics)
    {
        foreach (var dependency in _dependencies)
        {
            if (dependency is ISQLDependency sqlDependency)
            {
                sqlDependency.Unsubscribe(new Topic { Name = ((dynamic)sqlDependency).TableName });
            }
        }

        _dependencies.Clear();
    }

    private ISQLDependency CreateDependency(Topic topic)
    {
        var dependency = (ISQLDependency)Activator.CreateInstance(typeof(SqlDependency<>).MakeGenericType(Type.GetType(topic.Name)), _connectionString);

        if (dependency != null)
        {
        
            // Subscribe to the OnMessageReceived event from the SqlDependency<T> instance
            if (dependency is ISQLDependency sqlDep) // Adjust the condition as needed for specific types
            {
                sqlDep.OnMessage += (handler) =>
                {
                    Console.WriteLine("Message received in SqlDependencyProvider.");
                    // When a message is received, propagate it to the SqlDependencyProvider's event
                    OnMessageReceived?.Invoke(handler);
                };

                sqlDep.OnMessage += HandleMessageReceived;
            }
            dependency.Subscribe(topic);
        }
        return dependency;
    }

    // Event handler method to propagate the event
    private void HandleMessageReceived(DependencyHandler handler)
    {
        Console.WriteLine("Message received in SqlDependencyProvider.");
        // Propagate the event to SqlDependencyProvider's OnMessageReceived event
        OnMessageReceived?.Invoke(handler);
    }
}
