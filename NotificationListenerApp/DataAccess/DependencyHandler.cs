using NPOI.SS.Formula.Functions;
using TableDependency.SqlClient.Base.Delegates;

public class DependencyHandler<T>
{
    public event Action<DependencyHandler<T>> OnChanged;

    public void TriggerChange()
    {
        // Trigger the OnChanged event
        OnChanged?.Invoke(this);
    }
}