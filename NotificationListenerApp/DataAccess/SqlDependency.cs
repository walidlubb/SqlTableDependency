using TableDependency.SqlClient.Base.EventArgs;
using TableDependency.SqlClient;

public class SqlDependency<T> : ISQLDependency where T : class, new()
{
    private readonly string _connectionString;
    private SqlTableDependency<T> _tableDependency;

    public event Action<DependencyHandler> OnMessage;

    // Constructor Injection for connection string
    public SqlDependency(string connectionString)
    {
        _connectionString = connectionString;
    }



    // Handle change events from SQL table dependency
    private void TableDependency_OnChanged(object sender, RecordChangedEventArgs<T> e)
    {
        var entity = e.Entity;

        // Simulate a message being generated based on the change
        var message = new Message
        {
            Id = 1,
            SourceMessage = "Entity: " + typeof(T).Name,
            Description = "Change detected in table: " + typeof(T).Name,
            Title = "ChangeType: " + e.ChangeType.ToString()
        };

        var handler = new DependencyHandler { Message = message };

        // Only trigger the event on Insert changes (you can change this logic)
        if (e.ChangeType == TableDependency.SqlClient.Base.Enums.ChangeType.Insert)
        {
            OnMessage?.Invoke(handler);
        }
    }





    public void Subscribe(Topic topic)
    {
        _tableDependency = new SqlTableDependency<T>(_connectionString, tableName: topic.Name);
        _tableDependency.OnChanged += TableDependency_OnChanged;
        _tableDependency.Start();
    }

    public void Unsubscribe(Topic topic)
    {
        if (_tableDependency != null)
        {
            _tableDependency.OnChanged -= TableDependency_OnChanged;
            _tableDependency.Stop();
        }
    }
}
