namespace Atesh.BindableProperties;

public class CoerceValueCallbackArgs<T>
{
    public bool IsEmpty { get; set; }
    public T Value { get; set; }
}