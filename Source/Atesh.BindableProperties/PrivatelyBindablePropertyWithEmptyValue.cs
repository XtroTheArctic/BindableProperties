namespace Atesh.BindableProperties;

public class PrivatelyBindablePropertyWithEmptyValue<T> : PrivatelyBindableProperty<T>
{
    public PrivatelyBindablePropertyWithEmptyValue(object Owner, out BindMethods BindMethods, bool IsEmpty = false, Action Binder = null, CoerceValueCallback CoerceValue = null) : base(Owner, out BindMethods, IsEmpty, Binder, CoerceValue) { }
    public PrivatelyBindablePropertyWithEmptyValue(object Owner, out BindMethods BindMethods, T Value, Action Binder = null, CoerceValueCallback CoerceValue = null) : base(Owner, out BindMethods, Value, Binder, CoerceValue) { }

    public void ClearValue() => SetMethods.ClearValue();
}