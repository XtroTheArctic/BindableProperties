namespace Atesh.BindableProperties;

public class BindablePropertyWithEmptyValue<T> : BindableProperty<T>
{
    public BindablePropertyWithEmptyValue(object Owner, bool IsEmpty = false, Action Binder = null, CoerceValueCallback CoerceValue = null) : base(Owner, IsEmpty, Binder, CoerceValue) { }
    public BindablePropertyWithEmptyValue(object Owner, T Value, Action Binder = null, CoerceValueCallback CoerceValue = null) : base(Owner, Value, Binder, CoerceValue) { }

    public void ClearValue() => SetMethods.ClearValue();
}