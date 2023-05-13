namespace Atesh.BindableProperties;

public interface IBindableProperty
{
    protected internal bool IsEmpty { get; set; }
    protected internal bool TwoWay { get; set; }

    protected internal void Unbind();
}

public interface IBindableProperty<out T> : IBindableProperty
{
    public event ChangedEventHandler<T> Changed;
}