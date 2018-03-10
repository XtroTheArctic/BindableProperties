namespace Atesh.BindableProperties
{
    public interface IProperty<out T>
    {
        event ChangedEventHandler<T> Changed;
    }
}