using System;

namespace Atesh.BindableProperties
{
    public class PrivatelyBindablePropertyWithEmptyValue<T> : PrivatelyBindableProperty<T>
    {
        public PrivatelyBindablePropertyWithEmptyValue(object Owner, out BindDelegates BindDelegates, bool IsEmpty = false, Action BinderCallback = null, CoerceValueDelegate CoerceValueCallback = null) : base(Owner, out BindDelegates, IsEmpty, BinderCallback, CoerceValueCallback) { }

        public PrivatelyBindablePropertyWithEmptyValue(object Owner, out BindDelegates BindDelegates, T Value, Action BinderCallback = null, CoerceValueDelegate CoerceValueCallback = null) : base(Owner, out BindDelegates, Value, BinderCallback, CoerceValueCallback) { }

        public void ClearValue() => SetDelegates.ClearValue();
    }
}