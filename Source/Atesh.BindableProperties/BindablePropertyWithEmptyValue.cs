namespace Atesh.BindableProperties
{
    public class BindablePropertyWithEmptyValue<T> : BindableProperty<T>
    {
        public BindablePropertyWithEmptyValue(object Owner, bool IsEmpty = false) : base(Owner, IsEmpty) { }

        public BindablePropertyWithEmptyValue(object Owner, T Value) : base(Owner, Value) { }

        public void ClearValue() => SetDelegates.ClearValue();
    }
}