namespace Atesh.BindableProperties;

public partial class PrivatelySettableBindableProperty<T> : PrivatelySettablePrivatelyBindableProperty<T>
{
    readonly BindDelegates Delegates;

    public PrivatelySettableBindableProperty(object Owner, out SetDelegates SetDelegates, Action BinderCallback = null, CoerceValueDelegate CoerceValueCallback = null) : base(Owner, out SetDelegates, out TempBindDelegates, BinderCallback: BinderCallback, CoerceValueCallback: CoerceValueCallback) => Delegates = TempBindDelegates;
    public PrivatelySettableBindableProperty(object Owner, out SetDelegates SetDelegates, T Value, Action BinderCallback = null, CoerceValueDelegate CoerceValueCallback = null) : base(Owner, out SetDelegates, out TempBindDelegates, Value, BinderCallback, CoerceValueCallback) => Delegates = TempBindDelegates;
    internal PrivatelySettableBindableProperty(object Owner, out SetDelegates SetDelegates, bool IsEmpty = false, Action BinderCallback = null, CoerceValueDelegate CoerceValueCallback = null) : base(Owner, out SetDelegates, out TempBindDelegates, IsEmpty, BinderCallback, CoerceValueCallback) => Delegates = TempBindDelegates;

    public void Bind(PrivatelySettablePrivatelyBindableProperty<T> Target, bool TwoWay = false) => Delegates.Bind(Target, TwoWay);
    public void BindExtended<TargetType>(PrivatelySettablePrivatelyBindableProperty<TargetType> Target, Func<ChangedEventArgs<TargetType>, ChangedEventArgs<T>> PrimaryConverter, bool TwoWay = false, Func<ChangedEventArgs<T>, ChangedEventArgs<TargetType>> SecondaryConverter = null) => Delegates.BindExtended(Target, PrimaryConverter, TwoWay, SecondaryConverter);
    public new void Unbind() => Delegates.Unbind();
    public void Monitor(BindablePropertyBase Target) => Delegates.Monitor(Target);
    public void StartMonitoring() => Delegates.StartMonitoring();
    public void StopMonitoring() => Delegates.StopMonitoring();
}