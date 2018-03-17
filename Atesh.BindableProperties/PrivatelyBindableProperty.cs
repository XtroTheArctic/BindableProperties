namespace Atesh.BindableProperties
{
    public class PrivatelyBindableProperty<T> : PrivatelySettablePrivatelyBindableProperty<T>
    {
        #region Static
        // ReSharper disable PrivateFieldCanBeConvertedToLocalVariable

        static PrivatelySettableProperty<T>.Delegates TempDelegates;
        static Delegates TempBindingDelegates;

        // ReSharper restore PrivateFieldCanBeConvertedToLocalVariable
        #endregion

        new PrivatelySettableProperty<T>.Delegates Delegates;

        public PrivatelyBindableProperty(object Owner, out Delegates BindingDelegates, bool IsEmpty = false) : base(Owner, out TempDelegates, out TempBindingDelegates, IsEmpty)
        {
            Delegates = TempDelegates;
            BindingDelegates = TempBindingDelegates;
        }

        public PrivatelyBindableProperty(object Owner, out Delegates BindingDelegates, T Value) : base(Owner, out TempDelegates, out TempBindingDelegates, Value)
        {
            Delegates = TempDelegates;
            BindingDelegates = TempBindingDelegates;
        }

        public void SetValue(T Value) => Delegates.SetValue(Value);
        public void ClearValue() => Delegates.ClearValue();
    }
}