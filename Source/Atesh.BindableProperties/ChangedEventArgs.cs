namespace Atesh.BindableProperties;

public class ChangedEventArgs<T> : IChangedEventArgs<T>
{
    public bool IsEmpty { get; }
    public T Value { get; }

    public ChangedEventArgs(bool IsEmpty, T Value)
    {
        this.IsEmpty = IsEmpty;
        this.Value = Value;
    }
}