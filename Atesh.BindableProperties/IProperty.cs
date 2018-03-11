namespace Atesh.BindableProperties
{
    public interface IProperty<T>
    {
        event ChangedEventHandler<T> Changed;
    }
}