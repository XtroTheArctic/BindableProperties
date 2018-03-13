using System;
using System.Collections.Generic;

namespace Atesh.BindableProperties
{
    public class OwnerControlledProperty<T>
    {
        protected bool IsEmpty { get; private set; }

        public event ChangedEventHandler<T> Changed
        {
            add
            {
                _Changed += value;

                value(this, new ChangedEventArgs<T>(IsEmpty, Value));
            }
            remove => _Changed -= value;
        }

        event ChangedEventHandler<T> _Changed;

        public readonly object Owner;

        T Value;

        public OwnerControlledProperty(object Owner, out Delegates Delegates, bool IsEmpty = false)
        {
            this.Owner = Owner ?? throw new ArgumentNullException(nameof(Owner));
            Delegates.SetValue = SetValue;
            Delegates.ClearValue = ClearValue;

            this.IsEmpty = IsEmpty;
        }

        public OwnerControlledProperty(object Owner, out Delegates Delegates, T Value) : this(Owner, out Delegates)
        // ReSharper disable ArrangeConstructorOrDestructorBody
        // We can't convert this to expression body because of a Resharper bug which complains about out parameters not being assigned upon exit.
        {
            this.Value = Value;
        }
        // ReSharper restore ArrangeConstructorOrDestructorBody

        void OnChanged() => _Changed?.Invoke(this, new ChangedEventArgs<T>(IsEmpty, Value));

        void SetValue(T Value)
        {
            if (IsSameValue(Value)) return;

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
        protected bool IsSameValue(T Value) => !IsEmpty && EqualityComparer<T>.Default.Equals(this.Value, Value);

        public delegate void SetValueDelegate(T Value);

        public struct Delegates
        {
            public SetValueDelegate SetValue;
            public Action ClearValue;
        }
    }
}