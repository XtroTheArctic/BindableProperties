namespace Atesh.BindableProperties;

public interface IChangedEventArgs<out T>
{
    bool IsEmpty { get; }
    T Value { get; }
}