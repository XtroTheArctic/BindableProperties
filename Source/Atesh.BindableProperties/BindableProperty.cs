namespace Atesh.BindableProperties;

public partial class BindableProperty<T> : PrivatelyBindableProperty<T>
{
    new readonly BindMethods BindMethods;

    public BindableProperty(object Owner, Action Binder = null, CoerceValueCallback CoerceValue = null) : base(Owner, out TempBindMethods, Binder, CoerceValue) => BindMethods = TempBindMethods;
    public BindableProperty(object Owner, T Value, Action Binder = null, CoerceValueCallback CoerceValue = null) : base(Owner, out TempBindMethods, Value, Binder, CoerceValue) => BindMethods = TempBindMethods;
    internal BindableProperty(object Owner, bool IsEmpty = false, Action Binder = null, CoerceValueCallback CoerceValue = null) : base(Owner, out TempBindMethods, IsEmpty, Binder, CoerceValue) => BindMethods = TempBindMethods;

    public void Bind(PrivatelySettablePrivatelyBindableProperty<T> Target, bool TwoWay = false) => BindMethods.Bind(Target, TwoWay);
    public void BindExtended<TTarget>(PrivatelySettablePrivatelyBindableProperty<TTarget> Target, Func<ChangedEventArgs<TTarget>, ChangedEventArgs<T>> PrimaryConverter, bool TwoWay = false, Func<ChangedEventArgs<T>, ChangedEventArgs<TTarget>> SecondaryConverter = null) => BindMethods.BindExtended(Target, PrimaryConverter, TwoWay, SecondaryConverter);
    public new void Unbind() => BindMethods.Unbind();
    public void Monitor(BindablePropertyBase Target) => BindMethods.Monitor(Target);
    public void StartMonitoring() => BindMethods.StartMonitoring();
    public void StopMonitoring() => BindMethods.StopMonitoring();
}