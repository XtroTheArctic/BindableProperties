namespace Atesh.BindableProperties
{
    public class BindableProperty<T> : PrivatelyBindableProperty<T>
    {
        #region Static
        // ReSharper disable PrivateFieldCanBeConvertedToLocalVariable

        static Delegates TempDelegates;

        // ReSharper restore PrivateFieldCanBeConvertedToLocalVariable
        #endregion

        new Delegates Delegates;

        public BindableProperty(object Owner, bool IsEmpty = false) : base(Owner,  out TempDelegates, IsEmpty) => Delegates = TempDelegates;
        public BindableProperty(object Owner, T Value) : base(Owner, out TempDelegates, Value) => Delegates = TempDelegates;

        public void Bind(PrivatelySettableProperty<T> Target) => Delegates.Bind(Target);
        public void Unbind() => Delegates.Unbind();
    }
}