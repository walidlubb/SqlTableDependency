

public interface ISQLDependency
{
    void Subscribe(Topic topic) ;
    void Unsubscribe(Topic topic);

    event Action<DependencyHandler> OnMessage;

}