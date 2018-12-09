using System;

namespace Atesh.BindableProperties
{
    public class BindablePropertyWithEmptyValue<T> : BindableProperty<T>
    {
        public BindablePropertyWithEmptyValue(object Owner, bool IsEmpty = false, Action BinderCallback = null) : base(Owner, IsEmpty, BinderCallback) { }

        public BindablePropertyWithEmptyValue(object Owner, T Value, Action BinderCallback = null) : base(Owner, Value, BinderCallback) { }

        public void ClearValue() => SetDelegates.ClearValue();
    }
}