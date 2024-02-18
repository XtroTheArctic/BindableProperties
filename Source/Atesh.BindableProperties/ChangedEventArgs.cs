namespace Atesh.BindableProperties;

public class ChangedEventArgs<T>(bool IsEmpty, T Value)
{
    public bool IsEmpty { get; } = IsEmpty;
    public T Value { get; } = Value;
}