using System;

namespace Atesh.BindableProperties
{
    public class PrivatelyBindablePropertyWithEmptyValue<T> : PrivatelyBindableProperty<T>
    {
        public PrivatelyBindablePropertyWithEmptyValue(object Owner, out BindDelegates BindDelegates, bool IsEmpty = false, Action BinderCallback = null) : base(Owner, out BindDelegates, IsEmpty, BinderCallback) { }

        public PrivatelyBindablePropertyWithEmptyValue(object Owner, out BindDelegates BindDelegates, T Value, Action BinderCallback = null) : base(Owner, out BindDelegates, Value, BinderCallback) { }

        public void ClearValue() => SetDelegates.ClearValue();
    }
}