using System;

namespace Atesh.BindableProperties
{
    public class Property<T> : OwnerControlledProperty<T>
    {
        #region Static
        // ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
        // ReSharper disable StaticMemberInGenericType

        static Delegates TempDelegates;

        // ReSharper restore StaticMemberInGenericType
        // ReSharper restore PrivateFieldCanBeConvertedToLocalVariable
        #endregion

        public readonly SetValueDelegate SetValue;
        public readonly Action ClearValue;

        public Property(object Owner, bool IsEmpty = false) : base(Owner, out TempDelegates, IsEmpty)
        {
            SetValue = TempDelegates.SetValue;
            ClearValue = TempDelegates.ClearValue;
        }

        public Property(object Owner, T Value) : base(Owner, out TempDelegates, Value)
        {
            SetValue = TempDelegates.SetValue;
            ClearValue = TempDelegates.ClearValue;
        }
    }
}