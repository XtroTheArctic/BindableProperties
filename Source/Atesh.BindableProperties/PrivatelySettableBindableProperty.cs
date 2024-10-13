namespace Atesh.BindableProperties;

public partial class PrivatelySettableBindableProperty<T> : PrivatelySettablePrivatelyBindableProperty<T>
{
    new readonly BindMethods BindMethods;

    public PrivatelySettableBindableProperty(object Owner, out SetMethods SetMethods, Action Binder = null, CoerceValueCallback CoerceValue = null) : base(Owner, out SetMethods, out TempBindMethods, Binder: Binder, CoerceValue: CoerceValue) => BindMethods = TempBindMethods;
    public PrivatelySettableBindableProperty(object Owner, out SetMethods SetMethods, T Value, Action Binder = null, CoerceValueCallback CoerceValue = null) : base(Owner, out SetMethods, out TempBindMethods, Value, Binder, CoerceValue) => BindMethods = TempBindMethods;
    internal PrivatelySettableBindableProperty(object Owner, out SetMethods SetMethods, bool IsEmpty = false, Action Binder = null, CoerceValueCallback CoerceValue = null) : base(Owner, out SetMethods, out TempBindMethods, IsEmpty, Binder, CoerceValue) => BindMethods = TempBindMethods;

    public void Bind(PrivatelySettablePrivatelyBindableProperty<T> Target, bool TwoWay = false) => BindMethods.Bind(Target, TwoWay);
    public void BindExtended<TTarget>(PrivatelySettablePrivatelyBindableProperty<TTarget> Target, Func<ChangedEventArgs<TTarget>, ChangedEventArgs<T>> PrimaryConverter, bool TwoWay = false, Func<ChangedEventArgs<T>, ChangedEventArgs<TTarget>> SecondaryConverter = null) => BindMethods.BindExtended(Target, PrimaryConverter, TwoWay, SecondaryConverter);
    public new void Unbind() => BindMethods.Unbind();
    public void Monitor(BindablePropertyBase Target) => BindMethods.Monitor(Target);
    public void StartMonitoring() => BindMethods.StartMonitoring();
    public void StopMonitoring() => BindMethods.StopMonitoring();
}