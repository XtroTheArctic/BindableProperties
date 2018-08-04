namespace Atesh.BindableProperties
{
    public class PrivatelyBindableProperty<T> : PrivatelySettablePrivatelyBindableProperty<T>
    {
        #region Static

        // ReSharper disable PrivateFieldCanBeConvertedToLocalVariable

        static SetDelegates TempSetDelegates;

        // ReSharper restore PrivateFieldCanBeConvertedToLocalVariable

        #endregion

        new SetDelegates SetDelegates;

        public PrivatelyBindableProperty(object Owner, out BindDelegates BindDelegates, bool IsEmpty = false) : base(Owner, out TempSetDelegates, out BindDelegates, IsEmpty)
        // ReSharper disable ArrangeConstructorOrDestructorBody
        // We can't convert this to expression body because of a Resharper bug which complains about out parameters not being assigned upon exit.
        {
            SetDelegates = TempSetDelegates;
        }
        // ReSharper restore ArrangeConstructorOrDestructorBody

        public PrivatelyBindableProperty(object Owner, out BindDelegates BindDelegates, T Value) : base(Owner, out TempSetDelegates, out BindDelegates, Value)
        // ReSharper disable ArrangeConstructorOrDestructorBody
        // We can't convert this to expression body because of a Resharper bug which complains about out parameters not being assigned upon exit.
        {
            SetDelegates = TempSetDelegates;
        }
        // ReSharper restore ArrangeConstructorOrDestructorBody

        public void SetValue(T Value) => SetDelegates.SetValue(Value);
        public void ClearValue() => SetDelegates.ClearValue();
    }
}