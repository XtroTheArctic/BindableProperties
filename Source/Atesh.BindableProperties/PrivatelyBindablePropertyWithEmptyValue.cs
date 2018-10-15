namespace Atesh.BindableProperties
{
    public class PrivatelyBindablePropertyWithEmptyValue<T> : PrivatelyBindableProperty<T>
    {
        public PrivatelyBindablePropertyWithEmptyValue(object Owner, out BindDelegates BindDelegates, bool IsEmpty = false) : base(Owner, out TempSetDelegates, out BindDelegates, IsEmpty) { }

        public PrivatelyBindablePropertyWithEmptyValue(object Owner, out BindDelegates BindDelegates, T Value) : base(Owner, out BindDelegates, Value) { }

        public void ClearValue() => SetDelegates.ClearValue();
    }
}