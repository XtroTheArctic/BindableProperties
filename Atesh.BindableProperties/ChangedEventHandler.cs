namespace Atesh.BindableProperties
{
    public delegate void ChangedEventHandler<T>(OwnerControlledProperty<T> Sender, ChangedEventArgs<T> Args);
}