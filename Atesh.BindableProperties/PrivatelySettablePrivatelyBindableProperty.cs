using System;
using System.Collections.Generic;

namespace Atesh.BindableProperties
{
    public class PrivatelySettablePrivatelyBindableProperty<T>
    {
        #region Events
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
        #endregion

        public readonly object Owner;

        T Value;
        bool IsEmpty;
        PrivatelySettablePrivatelyBindableProperty<T> BoundProperty;

        public PrivatelySettablePrivatelyBindableProperty(object Owner, out SetDelegates SetDelegates, out BindDelegates BindDelegates, bool IsEmpty = false)
        // ReSharper disable ArrangeConstructorOrDestructorBody
        // We can't convert this to expression body because of a Resharper bug which complains about out parameters not being assigned upon exit.
        {
            this.Owner = Owner ?? throw new ArgumentNullException(nameof(Owner));
            this.IsEmpty = IsEmpty;

            SetDelegates.SetValue = SetValue;
            SetDelegates.ClearValue = ClearValue;
            BindDelegates.Bind = Bind;
            BindDelegates.Unbind = Unbind;
        }
        // ReSharper restore ArrangeConstructorOrDestructorBody

        public PrivatelySettablePrivatelyBindableProperty(object Owner, out SetDelegates SetDelegates, out BindDelegates BindDelegates, T Value) : this(Owner, out SetDelegates, out BindDelegates)
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
        bool IsSameValue(T Value) => !IsEmpty && EqualityComparer<T>.Default.Equals(this.Value, Value);

        void Bind(PrivatelySettablePrivatelyBindableProperty<T> Target)
        {
#pragma warning disable IDE0016 // Use 'throw' expression
            if (Target == null) throw new ArgumentNullException(nameof(Target));
#pragma warning restore IDE0016 // Use 'throw' expression

            if (BoundProperty != null) Unbind();

            BoundProperty = Target;
            BoundProperty.Changed += BoundProperty_Changed;
        }

        void Unbind()
        {
            if (BoundProperty == null) return;

            BoundProperty.Changed -= BoundProperty_Changed;
            BoundProperty = null;
        }

        void BoundProperty_Changed(PrivatelySettablePrivatelyBindableProperty<T> Sender, ChangedEventArgs<T> Args)
        {
        }

        public delegate void BindToPropertyDelegate(PrivatelySettablePrivatelyBindableProperty<T> Target);

        public delegate void SetValueDelegate(T Value);

        public struct SetDelegates
        {
            public SetValueDelegate SetValue;
            public Action ClearValue;
        }

        public struct BindDelegates
        {
            public BindToPropertyDelegate Bind;
            public Action Unbind;
        }
    }
}