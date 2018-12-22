using System;

namespace Atesh.BindableProperties
{
    public class BindableProperty<T> : PrivatelyBindableProperty<T>
    {
        #region Static

        [ThreadStatic] static BindDelegates TempDelegates;

        #endregion

        BindDelegates Delegates;

        public BindableProperty(object Owner, Action BinderCallback = null) : base(Owner, out TempDelegates, BinderCallback) => Delegates = TempDelegates;

        public BindableProperty(object Owner, T Value, Action BinderCallback = null) : base(Owner, out TempDelegates, Value, BinderCallback) => Delegates = TempDelegates;

        internal BindableProperty(object Owner, bool IsEmpty = false, Action BinderCallback = null) : base(Owner, out TempDelegates, IsEmpty, BinderCallback) => Delegates = TempDelegates;

        public void Bind(PrivatelySettablePrivatelyBindableProperty<T> Target, bool TwoWay = false) => Delegates.Bind(Target, TwoWay);
        public void Unbind() => Delegates.Unbind();
        public void Monitor(BindablePropertyBase Target) => Delegates.Monitor(Target);
        public void StartMonitoring() => Delegates.StartMonitoring();
        public void StopMonitoring() => Delegates.StopMonitoring();
    }
}