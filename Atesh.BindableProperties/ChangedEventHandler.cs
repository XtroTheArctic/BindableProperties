namespace Atesh.BindableProperties
{
    public delegate void ChangedEventHandler<T>(PrivatelySettableProperty<T> Sender, ChangedEventArgs<T> Args);
}