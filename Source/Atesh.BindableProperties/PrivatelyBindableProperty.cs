namespace Atesh.BindableProperties;

public partial class PrivatelyBindableProperty<T> : PrivatelySettablePrivatelyBindableProperty<T>
{
    protected new SetDelegates SetDelegates;

    public PrivatelyBindableProperty(object Owner, out BindDelegates BindDelegates, Action BinderCallback = null, CoerceValueDelegate CoerceValueCallback = null) : base(Owner, out TempSetDelegates, out BindDelegates, BinderCallback: BinderCallback, CoerceValueCallback: CoerceValueCallback) => SetDelegates = TempSetDelegates;
    public PrivatelyBindableProperty(object Owner, out BindDelegates BindDelegates, T Value, Action BinderCallback = null, CoerceValueDelegate CoerceValueCallback = null) : base(Owner, out TempSetDelegates, out BindDelegates, Value, BinderCallback, CoerceValueCallback) => SetDelegates = TempSetDelegates;
    internal PrivatelyBindableProperty(object Owner, out BindDelegates BindDelegates, bool IsEmpty = false, Action BinderCallback = null, CoerceValueDelegate CoerceValueCallback = null) : base(Owner, out TempSetDelegates, out BindDelegates, IsEmpty, BinderCallback, CoerceValueCallback) => SetDelegates = TempSetDelegates;

    public void SetValue(T Value) => SetDelegates.SetValue(Value);
}