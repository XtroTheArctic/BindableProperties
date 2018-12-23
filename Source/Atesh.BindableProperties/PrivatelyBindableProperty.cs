using System;

namespace Atesh.BindableProperties
{
    public class PrivatelyBindableProperty<T> : PrivatelySettablePrivatelyBindableProperty<T>
    {
        #region Static

        [ThreadStatic] internal static SetDelegates TempSetDelegates;

        #endregion

        protected new SetDelegates SetDelegates;

        public PrivatelyBindableProperty(object Owner, out BindDelegates BindDelegates, Action BinderCallback = null, CoerceValueDelegate CoerceValueCallback = null) : base(Owner, out TempSetDelegates, out BindDelegates, BinderCallback: BinderCallback, CoerceValueCallback: CoerceValueCallback)
        // ReSharper disable ArrangeConstructorOrDestructorBody
        // We can't convert this to expression body because of a Resharper bug which complains about out parameters not being assigned upon exit.
        {
            SetDelegates = TempSetDelegates;
        }
        // ReSharper restore ArrangeConstructorOrDestructorBody

        public PrivatelyBindableProperty(object Owner, out BindDelegates BindDelegates, T Value, Action BinderCallback = null, CoerceValueDelegate CoerceValueCallback = null) : base(Owner, out TempSetDelegates, out BindDelegates, Value, BinderCallback, CoerceValueCallback)
        // ReSharper disable ArrangeConstructorOrDestructorBody
        // We can't convert this to expression body because of a Resharper bug which complains about out parameters not being assigned upon exit.
        {
            SetDelegates = TempSetDelegates;
        }
        // ReSharper restore ArrangeConstructorOrDestructorBody

        internal PrivatelyBindableProperty(object Owner, out BindDelegates BindDelegates, bool IsEmpty = false, Action BinderCallback = null, CoerceValueDelegate CoerceValueCallback = null) : base(Owner, out TempSetDelegates, out BindDelegates, IsEmpty, BinderCallback, CoerceValueCallback)
        // ReSharper disable ArrangeConstructorOrDestructorBody
        // We can't convert this to expression body because of a Resharper bug which complains about out parameters not being assigned upon exit.
        {
            SetDelegates = TempSetDelegates;
        }
        // ReSharper restore ArrangeConstructorOrDestructorBody

        public void SetValue(T Value) => SetDelegates.SetValue(Value);
    }
}