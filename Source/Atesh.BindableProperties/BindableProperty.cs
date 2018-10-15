using System;

namespace Atesh.BindableProperties
{
    public class BindableProperty<T> : PrivatelyBindableProperty<T>
    {
        #region Static

        [ThreadStatic] static BindDelegates TempDelegates;

        #endregion

        BindDelegates Delegates;

        public BindableProperty(object Owner, T Value) : base(Owner, out TempDelegates, Value) => Delegates = TempDelegates;

        internal BindableProperty(object Owner, bool IsEmpty = false) : base(Owner, out TempSetDelegates, out TempDelegates, IsEmpty) => Delegates = TempDelegates;

        public void Bind(PrivatelySettablePrivatelyBindableProperty<T> Target) => Delegates.Bind(Target);
        public void Unbind() => Delegates.Unbind();
    }
}