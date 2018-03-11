using System;

namespace Atesh.BindableProperties
{
    public class Property<T> : ReadOnlyProperty<T>
    {
        #region Static
        // ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
        // ReSharper disable StaticMemberInGenericType

        static SetValueDelegate TempSetValueDelegate;
        static Action TempClearValueDelegate;

        // ReSharper restore StaticMemberInGenericType
        // ReSharper restore PrivateFieldCanBeConvertedToLocalVariable
        #endregion

        public readonly SetValueDelegate SetValue;
        public readonly Action ClearValue;

        public Property(object Owner, bool IsEmpty = false) : base(Owner, out TempSetValueDelegate, out TempClearValueDelegate, IsEmpty)
        {
            SetValue = TempSetValueDelegate;
            ClearValue = TempClearValueDelegate;
        }

        public Property(object Owner, T Value) : base(Owner, out TempSetValueDelegate, out TempClearValueDelegate, Value)
        {
            SetValue = TempSetValueDelegate;
            ClearValue = TempClearValueDelegate;
        }
    }
}