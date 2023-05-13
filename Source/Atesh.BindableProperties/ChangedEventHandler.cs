namespace Atesh.BindableProperties;

public delegate void ChangedEventHandler<T>(IBindableProperty<T> Sender, ChangedEventArgs<T> Args);