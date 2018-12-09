using System;
using System.Collections.Generic;

namespace Atesh.BindableProperties
{
    public class PrivatelySettablePrivatelyBindableProperty<T> : BindablePropertyBase
    {
        #region Events

        public new event ChangedEventHandler<T> Changed
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

        readonly Action BinderCallback;
        T Value;
        bool IsEmpty;
        PrivatelySettablePrivatelyBindableProperty<T> BoundProperty;
        readonly HashSet<BindablePropertyBase> MonitoredProperties = new HashSet<BindablePropertyBase>();

        public PrivatelySettablePrivatelyBindableProperty(object Owner, out SetDelegates SetDelegates, out BindDelegates BindDelegates, bool IsEmpty = false, Action BinderCallback = null)
        {
            this.Owner = Owner ?? throw new ArgumentNullException(nameof(Owner));
            this.IsEmpty = IsEmpty;
            this.BinderCallback = BinderCallback;

            SetDelegates.SetValue = SetValue;
            SetDelegates.ClearValue = ClearValue;
            BindDelegates.Bind = Bind;
            BindDelegates.Unbind = Unbind;
            BindDelegates.Monitor = Monitor;
        }

        public PrivatelySettablePrivatelyBindableProperty(object Owner, out SetDelegates SetDelegates, out BindDelegates BindDelegates, T Value, Action BinderCallback = null) : this(Owner, out SetDelegates, out BindDelegates, BinderCallback: BinderCallback)
        // ReSharper disable ArrangeConstructorOrDestructorBody
        // We can't convert this to expression body because of a Resharper bug which complains about out parameters not being assigned upon exit.
        {
            this.Value = Value;
        }
        // ReSharper restore ArrangeConstructorOrDestructorBody

        void OnChanged()
        {
            base.Changed?.Invoke();
            _Changed?.Invoke(this, new ChangedEventArgs<T>(IsEmpty, Value));
        }

        void SetValue(T Value)
        {
            if (BoundProperty != null) Unbind();

            if (IsEmpty || !ValueEquals(Value)) SetAndRaise(Value);
        }

        // We use EqualityComparer instead of object.Equals because it avoids boxing of value types including structs.
        bool ValueEquals(T Value) => EqualityComparer<T>.Default.Equals(this.Value, Value);

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
            if (Target == null) throw new ArgumentNullException(nameof(Target));
            if (Target == this) throw new ArgumentException(Strings.PropertyCanNotBindToItself, nameof(Target));

            if (BoundProperty != null) Unbind();

            BoundProperty = Target;
            BoundProperty.Changed += BoundProperty_Changed;

            MonitorAll();
        }

        void Unbind()
        {
            if (BoundProperty == null) return;

            UnmonitorAll();

            BoundProperty.Changed -= BoundProperty_Changed;
            BoundProperty = null;
        }

        void Monitor(BindablePropertyBase Target)
        {
            if (Target == null) throw new ArgumentNullException(nameof(Target));
            if (Target == this) throw new ArgumentException(Strings.PropertyCanNotMonitorItself, nameof(Target));
            if (IsBound) throw new InvalidOperationException(Strings.PropertyCanNotMonitorAfterBind);

            MonitoredProperties.Add(Target);
        }

        void MonitorAll()
        {
            foreach (var MonitoredProperty in MonitoredProperties)
            {
                MonitoredProperty.Changed += MonitoredProperty_Changed;
            }
        }

        void UnmonitorAll()
        {
            foreach (var MonitoredProperty in MonitoredProperties)
            {
                MonitoredProperty.Changed -= MonitoredProperty_Changed;
            }

            MonitoredProperties.Clear();
        }

        void MonitoredProperty_Changed()
        {
            Unbind();

            BinderCallback?.Invoke();
        }

        void BoundProperty_Changed(PrivatelySettablePrivatelyBindableProperty<T> Sender, ChangedEventArgs<T> Args)
        {
            if (Args.IsEmpty)
            {
                if (!IsEmpty) ClearAndRaise();
            }
            else
            {
                if (IsEmpty || !ValueEquals(Args.Value)) SetAndRaise(Args.Value);
            }
        }

        public delegate void BindToPropertyDelegate(PrivatelySettablePrivatelyBindableProperty<T> Target);

        public delegate void SetValueDelegate(T Value);

        public delegate void MonitorDelegate(BindablePropertyBase Target);

        public struct SetDelegates
        {
            public SetValueDelegate SetValue;
            public Action ClearValue;
        }

        public struct BindDelegates
        {
            public BindToPropertyDelegate Bind;
            public Action Unbind;
            public MonitorDelegate Monitor;
        }
    }
}