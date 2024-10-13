namespace Atesh.BindableProperties;

public partial class PrivatelyBindableProperty<T> : PrivatelySettablePrivatelyBindableProperty<T>
{
    protected new SetMethods SetMethods;

    public PrivatelyBindableProperty(object Owner, out BindMethods BindMethods, Action Binder = null, CoerceValueCallback CoerceValue = null) : base(Owner, out TempSetMethods, out BindMethods, Binder: Binder, CoerceValue: CoerceValue) => SetMethods = TempSetMethods;
    public PrivatelyBindableProperty(object Owner, out BindMethods BindMethods, T Value, Action Binder = null, CoerceValueCallback CoerceValue = null) : base(Owner, out TempSetMethods, out BindMethods, Value, Binder, CoerceValue) => SetMethods = TempSetMethods;
    internal PrivatelyBindableProperty(object Owner, out BindMethods BindMethods, bool IsEmpty = false, Action Binder = null, CoerceValueCallback CoerceValue = null) : base(Owner, out TempSetMethods, out BindMethods, IsEmpty, Binder, CoerceValue) => SetMethods = TempSetMethods;

    public void SetValue(T Value) => SetMethods.SetValue(Value);
}