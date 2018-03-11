using System;
using System.Collections.Generic;

namespace Atesh.BindableProperties
{
    public class OwnerControlledProperty<T> : IProperty<T>
    {
        public event ChangedEventHandler<T> Changed
        {
            add
            {
                _Changed += value;

                value(Owner, new ChangedEventArgs<T>(IsEmpty, Value));
            }
            remove => _Changed -= value;
        }

        event ChangedEventHandler<T> _Changed;

        readonly object Owner;
        T Value;
        bool IsEmpty;

        public OwnerControlledProperty(object Owner, out SetValueDelegate SetValueDelegate, out Action ClearValueDelegate, bool IsEmpty = false)
        {
            this.Owner = Owner ?? throw new ArgumentNullException(nameof(Owner));
            SetValueDelegate = SetValue;
            ClearValueDelegate = ClearValue;

            this.IsEmpty = IsEmpty;
        }

        public OwnerControlledProperty(object Owner, out SetValueDelegate SetValueDelegate, out Action ClearValueDelegate, T Value) : this(Owner, out SetValueDelegate, out ClearValueDelegate)
        // ReSharper disable ArrangeConstructorOrDestructorBody
        // We can't convert this to expression body because of a Resharper bug which complains about out parameters not being assigned upon exit.
        {
            this.Value = Value;
        }
        // ReSharper restore ArrangeConstructorOrDestructorBody

        void OnChanged() => _Changed?.Invoke(Owner, new ChangedEventArgs<T>(IsEmpty, Value));

        void SetValue(T Value)
        {
            if (!IsEmpty && IsSameValue(Value)) return;

            IsEmpty = false;
            this.Value = Value;

            OnChanged();
        }

        void ClearValue()
        {
            if (IsEmpty) return;

            IsEmpty = true;

            OnChanged();
        }

        // We use EqualityComparer instead of object.Equals because it avoids boxing of value types including structs.
        protected bool IsSameValue(T Value) => EqualityComparer<T>.Default.Equals(this.Value, Value);

        public delegate void SetValueDelegate(T Value);
    }
}