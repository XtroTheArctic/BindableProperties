using System;
using System.Collections.Generic;

namespace Atesh.BindableProperties
{
    public class PrivatelySettablePrivatelyBindableProperty<T> : BindablePropertyBase
    {
        public PrivatelySettablePrivatelyBindableProperty<T> BoundProperty { get; private set; }
        public bool IsMonitoringWithoutBinding { get; private set; }

        public new event ChangedEventHandler<T> Changed
        {
            add
            {
                Changed_ += value;

                value(this, new ChangedEventArgs<T>(IsEmpty, Value));
            }
            remove => Changed_ -= value;
        }
        
        event ChangedEventHandler<T> Changed_;

        public readonly object Owner;

        readonly Action BinderCallback;
        readonly CoerceValueDelegate CoerceValueCallback;
        T Value;
        bool IsEmpty;
        bool TwoWay;
        readonly HashSet<BindablePropertyBase> MonitoredProperties = new HashSet<BindablePropertyBase>();

        public PrivatelySettablePrivatelyBindableProperty(object Owner, out SetDelegates SetDelegates, out BindDelegates BindDelegates, bool IsEmpty = false, Action BinderCallback = null, CoerceValueDelegate CoerceValueCallback = null)
        {
            this.Owner = Owner ?? throw new ArgumentNullException(nameof(Owner));
            this.IsEmpty = IsEmpty;
            this.BinderCallback = BinderCallback;
            this.CoerceValueCallback = CoerceValueCallback;

            SetDelegates.SetValue = SetValue;
            SetDelegates.ClearValue = ClearValue;
            BindDelegates.Bind = Bind;
            BindDelegates.Unbind = Unbind;
            BindDelegates.Monitor = Monitor;
            BindDelegates.StartMonitoring = StartMonitoring;
            BindDelegates.StopMonitoring = StopMonitoring;
        }

        public PrivatelySettablePrivatelyBindableProperty(object Owner, out SetDelegates SetDelegates, out BindDelegates BindDelegates, T Value, Action BinderCallback = null, CoerceValueDelegate CoerceValueCallback = null) : this(Owner, out SetDelegates, out BindDelegates, BinderCallback: BinderCallback, CoerceValueCallback: CoerceValueCallback) => this.Value = Value;

        void OnChanged()
        {
            Changed_?.Invoke(this, new ChangedEventArgs<T>(IsEmpty, Value));
            base.Changed?.Invoke();
        }

        void SetValue(T Value)
        {
            if (BoundProperty != null && !TwoWay) Unbind();

            if (IsEmpty || !ValueEquals(Value))
            {
                if (CoerceValueCallback == null) SetAndRaise(Value);
                else
                {
                    var Args = new CoerceValueDelegateArgs<T> { Value = Value, IsEmpty = false };
                    CoerceValue(Args);
                }
            }
        }

        void CoerceValue(CoerceValueDelegateArgs<T> Args)
        {
            CoerceValueCallback(Args);

            if (Args.IsEmpty) ClearAndRaise();
            else SetAndRaise(Args.Value);
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

            if (!IsEmpty)
            {
                if (CoerceValueCallback == null) ClearAndRaise();
                else
                {
                    var Args = new CoerceValueDelegateArgs<T> { IsEmpty = true };
                    CoerceValue(Args);
                }
            }
        }

        void ClearAndRaise()
        {
            IsEmpty = true;

            OnChanged();
        }

        void Bind(PrivatelySettablePrivatelyBindableProperty<T> Target, bool TwoWay = false)
        {
            if (Target == null) throw new ArgumentNullException(nameof(Target));
            if (Target == this) throw new ArgumentException(Strings.PropertyCanNotBindToItself, nameof(Target));

            if (IsMonitoringWithoutBinding) StopMonitoring();
            else if (BoundProperty != null) Unbind();

            StartMonitoring();
            IsMonitoringWithoutBinding = false;

            this.TwoWay = TwoWay;

            if (TwoWay)
            {
                Target.Bind(this);
                Target.TwoWay = true;
            }

            BoundProperty = Target;
            BoundProperty.Changed += BoundProperty_Changed;
        }

        void Unbind()
        {
            if (BoundProperty == null) throw new InvalidOperationException(Strings.PropertyNotBoundYet);

            IsMonitoringWithoutBinding = true;
            StopMonitoring();

            if (TwoWay)
            {
                BoundProperty.TwoWay = false;
                BoundProperty.Unbind();
            }

            BoundProperty.Changed -= BoundProperty_Changed;
            BoundProperty = null;
        }

        void Monitor(BindablePropertyBase Target)
        {
            if (Target == null) throw new ArgumentNullException(nameof(Target));
            if (Target == this) throw new ArgumentException(Strings.PropertyCanNotMonitorItself, nameof(Target));
            if (IsMonitoringWithoutBinding || BoundProperty != null) throw new InvalidOperationException(Strings.PropertyCanNotMonitorAfterMonitoringStarted);

            MonitoredProperties.Add(Target);
        }

        void StartMonitoring()
        {
            if (IsMonitoringWithoutBinding || BoundProperty != null) throw new InvalidOperationException(Strings.MonitoringAlreadyStarted);

            foreach (var MonitoredProperty in MonitoredProperties)
            {
                MonitoredProperty.Changed += MonitoredProperty_Changed;
            }

            IsMonitoringWithoutBinding = true;
        }

        void StopMonitoring()
        {
            if (!IsMonitoringWithoutBinding) throw new InvalidOperationException(Strings.MonitoringNotStartedYet);

            foreach (var MonitoredProperty in MonitoredProperties)
            {
                MonitoredProperty.Changed -= MonitoredProperty_Changed;
            }

            MonitoredProperties.Clear();

            IsMonitoringWithoutBinding = false;
        }

        void MonitoredProperty_Changed()
        {
            if (BoundProperty == null) StopMonitoring();
            else Unbind();

            BinderCallback?.Invoke();
        }

        void BoundProperty_Changed(PrivatelySettablePrivatelyBindableProperty<T> Sender, ChangedEventArgs<T> Args)
        {
            // If Sender and BoundProperty aren't the same, ignore this Changed call. The new BoundProperty should call Changed event handler with correct value.
            if (Sender != BoundProperty) return;

            if (Args.IsEmpty)
            {
                if (!IsEmpty) ClearAndRaise();
            }
            else
            {
                if (IsEmpty || !ValueEquals(Args.Value)) SetAndRaise(Args.Value);
            }
        }

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
            public Action StartMonitoring;
            public Action StopMonitoring;
        }

        public delegate void BindToPropertyDelegate(PrivatelySettablePrivatelyBindableProperty<T> Target, bool TwoWay = false);

        public delegate void SetValueDelegate(T Value);

        public delegate void CoerceValueDelegate(CoerceValueDelegateArgs<T> Args);

        public delegate void MonitorDelegate(BindablePropertyBase Target);
    }
}