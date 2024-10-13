namespace Atesh.BindableProperties;

public abstract class BindablePropertyBase
{
    protected internal abstract bool IsEmpty { get; set; }
    protected internal abstract bool TwoWay { get; set; }

    internal Action<object, object> Changed;

    protected internal abstract void Unbind();
}