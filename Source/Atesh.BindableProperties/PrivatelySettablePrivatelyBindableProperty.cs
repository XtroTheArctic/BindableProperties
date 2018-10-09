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

        #region Properties

        public bool IsBound => BoundProperty != null;

        #endregion

        public readonly object Owner;

        T Value;
        bool IsEmpty;
        PrivatelySettablePrivatelyBindableProperty<T> BoundProperty;

        public PrivatelySettablePrivatelyBindableProperty(object Owner, out SetDelegates SetDelegates, out BindDelegates BindDelegates, bool IsEmpty = false)
        {
            this.Owner = Owner ?? throw new ArgumentNullException(nameof(Owner));
            this.IsEmpty = IsEmpty;

            SetDelegates.SetValue = SetValue;
            SetDelegates.ClearValue = ClearValue;
            BindDelegates.Bind = Bind;
            BindDelegates.Unbind = Unbind;
        }

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
            if (BoundProperty != null) Unbind();

            // We use EqualityComparer instead of object.Equals because it avoids boxing of value types including structs.
            if (IsEmpty || !EqualityComparer<T>.Default.Equals(this.Value, Value)) SetAndRaise(Value);
        }

        void SetAndRaise(T Value)
        {
            IsEmpty = false;
            this.Value = Value;

            OnChanged();
        }

        void ClearValue()
        {
            if (BoundProperty != null) Unbind();

            if (!IsEmpty) ClearAndRaise();
        }

        void ClearAndRaise()
        {
            IsEmpty = true;

            OnChanged();
        }

        void Bind(PrivatelySettablePrivatelyBindableProperty<T> Target)
        {
#pragma warning disable IDE0016 // Use 'throw' expression
            if (Target == null) throw new ArgumentNullException(nameof(Target));
#pragma warning restore IDE0016 // Use 'throw' expression
            if (Target == this) throw new ArgumentException(Strings.PropertyCanNotBindToItself, nameof(Target));

            if (BoundProperty != null) Unbind();

            BoundProperty = Target;
            BoundProperty.Changed += BoundProperty_Changed;
        }

        void Unbind()
        {
            if (BoundProperty == null) return;

            BoundProperty.Changed -= BoundProperty_Changed;
            BoundProperty = null;

            BoundProperty_Changed(this, new ChangedEventArgs<T>(IsEmpty, Value));
        }

        void BoundProperty_Changed(PrivatelySettablePrivatelyBindableProperty<T> Sender, ChangedEventArgs<T> Args)
        {
            if (Args.IsEmpty) ClearAndRaise();
            else SetAndRaise(Args.Value);
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