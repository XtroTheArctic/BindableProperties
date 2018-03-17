namespace Atesh.BindableProperties
{
    public class Property<T> : PrivatelySettableProperty<T>
    {
        #region Static
        // ReSharper disable PrivateFieldCanBeConvertedToLocalVariable

        static Delegates TempDelegates;

        // ReSharper restore PrivateFieldCanBeConvertedToLocalVariable
        #endregion

        new Delegates Delegates;

        public Property(object Owner, bool IsEmpty = false) : base(Owner, out TempDelegates, IsEmpty) => Delegates = TempDelegates;
        public Property(object Owner, T Value) : base(Owner, out TempDelegates, Value) => Delegates = TempDelegates;

        public void SetValue(T Value) => Delegates.SetValue(Value);
        public void ClearValue() => Delegates.ClearValue();
    }
}