namespace Atesh.BindableProperties;

public class PrivatelySettableBindablePropertyWithEmptyValue<T> : PrivatelySettableBindableProperty<T>
{
    public PrivatelySettableBindablePropertyWithEmptyValue(object Owner, out SetMethods SetMethods, bool IsEmpty = false, Action Binder = null, CoerceValueCallback CoerceValue = null) : base(Owner, out SetMethods, IsEmpty, Binder, CoerceValue) { }
    public PrivatelySettableBindablePropertyWithEmptyValue(object Owner, out SetMethods SetMethods, T Value, Action Binder = null, CoerceValueCallback CoerceValue = null) : base(Owner, out SetMethods, Value, Binder, CoerceValue) { }
}