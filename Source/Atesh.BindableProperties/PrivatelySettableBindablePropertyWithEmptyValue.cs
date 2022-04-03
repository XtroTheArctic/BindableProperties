using System;

namespace Atesh.BindableProperties;

public class PrivatelySettableBindablePropertyWithEmptyValue<T> : PrivatelySettableBindableProperty<T>
{
    public PrivatelySettableBindablePropertyWithEmptyValue(object Owner, out SetDelegates SetDelegates, bool IsEmpty = false, Action BinderCallback = null, CoerceValueDelegate CoerceValueCallback = null) : base(Owner, out SetDelegates, IsEmpty, BinderCallback, CoerceValueCallback) { }
    public PrivatelySettableBindablePropertyWithEmptyValue(object Owner, out SetDelegates SetDelegates, T Value, Action BinderCallback = null, CoerceValueDelegate CoerceValueCallback = null) : base(Owner, out SetDelegates, Value, BinderCallback, CoerceValueCallback) { }
}