using System;

namespace Atesh.BindableProperties;

public class BindablePropertyWithEmptyValue<T> : BindableProperty<T>
{
    public BindablePropertyWithEmptyValue(object Owner, bool IsEmpty = false, Action BinderCallback = null, CoerceValueDelegate CoerceValueCallback = null) : base(Owner, IsEmpty, BinderCallback, CoerceValueCallback) { }

    public BindablePropertyWithEmptyValue(object Owner, T Value, Action BinderCallback = null, CoerceValueDelegate CoerceValueCallback = null) : base(Owner, Value, BinderCallback, CoerceValueCallback) { }

    public void ClearValue() => SetDelegates.ClearValue();
}