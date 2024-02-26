namespace Atesh.BindableProperties;

public class PrivatelySettablePrivatelyBindableProperty<T> : BindablePropertyBase
{
    public object Owner { get; }
    public BindablePropertyBase BoundProperty { get; private set; }
    public bool IsMonitoringWithoutBinding { get; private set; }

    protected internal override bool IsEmpty { get; set; }
    protected internal override bool TwoWay { get; set; }

    public new event ChangedEventHandler<T> Changed
    {
        add
        {
            Changed_ += value;

            value(this, new(IsEmpty, Value));
        }
        remove => Changed_ -= value;
    }

    event ChangedEventHandler<T> Changed_;

    readonly Action BinderCallback;
    readonly CoerceValueDelegate CoerceValueCallback;
    T Value;
    readonly HashSet<BindablePropertyBase> MonitoredProperties = [];
    Delegate Converter;

    public PrivatelySettablePrivatelyBindableProperty(object Owner, out SetDelegates SetDelegates, out BindDelegates BindDelegates, bool IsEmpty = false, Action BinderCallback = null, CoerceValueDelegate CoerceValueCallback = null)
    {
        this.Owner = Owner ?? throw new ArgumentNullException(nameof(Owner));
        this.IsEmpty = IsEmpty;
        this.BinderCallback = BinderCallback;
        this.CoerceValueCallback = CoerceValueCallback;

        SetDelegates.SetValue = SetValue;
        SetDelegates.ClearValue = ClearValue;
        BindDelegates.Owner = this;
        BindDelegates.Bind = Bind;
        BindDelegates.Unbind = Unbind;
        BindDelegates.Monitor = Monitor;
        BindDelegates.StartMonitoring = StartMonitoring;
        BindDelegates.StopMonitoring = StopMonitoring;
    }

    public PrivatelySettablePrivatelyBindableProperty(object Owner, out SetDelegates SetDelegates, out BindDelegates BindDelegates, T Value, Action BinderCallback = null, CoerceValueDelegate CoerceValueCallback = null) : this(Owner, out SetDelegates, out BindDelegates, BinderCallback: BinderCallback, CoerceValueCallback: CoerceValueCallback) => this.Value = Value;

    void OnChanged()
    {
        var Args = new ChangedEventArgs<T>(IsEmpty, Value);
        Changed_?.Invoke(this, Args);
        base.Changed?.Invoke(this, Args);
    }

    void SetValue(T Value)
    {
        if (BoundProperty is { } && !TwoWay) Unbind();

        if (!IsEmpty && ValueEquals(Value)) return;

        if (CoerceValueCallback == null) SetAndRaise(Value);
        else
        {
            var Args = new CoerceValueDelegateArgs<T> { Value = Value, IsEmpty = false };
            CoerceValue(Args);
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
        if (BoundProperty is { }) Unbind();

        if (IsEmpty) return;

        if (CoerceValueCallback == null) ClearAndRaise();
        else
        {
            var Args = new CoerceValueDelegateArgs<T> { IsEmpty = true };
            CoerceValue(Args);
        }
    }

    void ClearAndRaise()
    {
        IsEmpty = true;

        OnChanged();
    }

    void Bind(PrivatelySettablePrivatelyBindableProperty<T> Target, bool TwoWay)
    {
        if (Target == null) throw new ArgumentNullException(nameof(Target));
        if (Target == this) throw new ArgumentException(Strings.PropertyCanNotBindToItself, nameof(Target));

        if (IsMonitoringWithoutBinding) StopMonitoring();
        else if (BoundProperty is { }) Unbind();

        StartMonitoring();
        IsMonitoringWithoutBinding = false;

        BoundProperty = Target;
        ((PrivatelySettablePrivatelyBindableProperty<T>)BoundProperty).Changed += BoundProperty_Changed;

        this.TwoWay = TwoWay;

        if (!TwoWay) return;

        Target.Bind(this, false);
        Target.TwoWay = true;
    }

    void BindExtended<TTarget>(PrivatelySettablePrivatelyBindableProperty<TTarget> Target, Func<ChangedEventArgs<TTarget>, ChangedEventArgs<T>> PrimaryConverter, bool TwoWay, Func<ChangedEventArgs<T>, ChangedEventArgs<TTarget>> SecondaryConverter = null)
    {
        if (Target == null) throw new ArgumentNullException(nameof(Target));
#pragma warning disable IDE0016 // Use 'throw' expression
        if (PrimaryConverter == null) throw new ArgumentNullException(nameof(PrimaryConverter));
#pragma warning restore IDE0016 // Use 'throw' expression
        if (TwoWay && SecondaryConverter == null) throw new ArgumentNullException(nameof(SecondaryConverter), Strings.PropertyCanNotBindTwoWayWithoutSecondaryConverter);
        if (typeof(T) == typeof(TTarget) && Target as PrivatelySettablePrivatelyBindableProperty<T> == this) throw new ArgumentException(Strings.PropertyCanNotBindToItself, nameof(Target));

        if (IsMonitoringWithoutBinding) StopMonitoring();
        else if (BoundProperty is { }) Unbind();

        Converter = PrimaryConverter;

        StartMonitoring();
        IsMonitoringWithoutBinding = false;

        BoundProperty = Target;
        var BoundPropertyType = BoundProperty.GetType();

        while (BoundPropertyType.BaseType != typeof(BindablePropertyBase))
        {
            BoundPropertyType = BoundPropertyType.BaseType;
        }

        if (BoundProperty is PrivatelySettablePrivatelyBindableProperty<T> BoundPropertyWithSameType) BoundPropertyWithSameType.Changed += BoundProperty_Changed;
        else
        {
            BoundProperty.Changed += BoundPropertyWithDifferentType_Changed;
            BoundPropertyWithDifferentType_Changed(BoundProperty, new ChangedEventArgs<TTarget>(BoundProperty.IsEmpty, ((PrivatelySettablePrivatelyBindableProperty<TTarget>)BoundProperty).Value));
        }

        this.TwoWay = TwoWay;

        if (!TwoWay) return;

        Target.BindExtended(this, SecondaryConverter, false);
        Target.TwoWay = true;
    }

    protected internal override void Unbind()
    {
        if (BoundProperty == null) throw new InvalidOperationException(Strings.PropertyNotBoundYet);

        IsMonitoringWithoutBinding = true;
        StopMonitoring();

        if (TwoWay)
        {
            BoundProperty.TwoWay = false;
            BoundProperty.Unbind();
        }

        if (BoundProperty is PrivatelySettablePrivatelyBindableProperty<T> BoundPropertyWithSameType) BoundPropertyWithSameType.Changed -= BoundProperty_Changed;
        else BoundProperty.Changed -= BoundPropertyWithDifferentType_Changed;
        BoundProperty = null;
        Converter = null;
    }

    void Monitor(BindablePropertyBase Target)
    {
        if (Target == null) throw new ArgumentNullException(nameof(Target));
        if (Target == this) throw new ArgumentException(Strings.PropertyCanNotMonitorItself, nameof(Target));
        if (IsMonitoringWithoutBinding || BoundProperty is { }) throw new InvalidOperationException(Strings.PropertyCanNotMonitorAfterMonitoringStarted);

        MonitoredProperties.Add(Target);
    }

    void StartMonitoring()
    {
        if (IsMonitoringWithoutBinding || BoundProperty is { }) throw new InvalidOperationException(Strings.MonitoringAlreadyStarted);

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

    void MonitoredProperty_Changed(object Sender, object Args)
    {
        if (BoundProperty == null)
        {
            IsMonitoringWithoutBinding = true;
            StopMonitoring();
        }
        else Unbind();

        BinderCallback?.Invoke();
    }

    void BoundProperty_Changed(PrivatelySettablePrivatelyBindableProperty<T> Sender, ChangedEventArgs<T> Args)
    {
        // If Sender and BoundProperty aren't the same, ignore this Changed call. The new BoundProperty should call Changed event handler with correct value.
        if (Sender != BoundProperty) return;

        if (Converter is { }) BoundPropertyWithDifferentType_Changed(Sender, Args);
        else
        {
            if (Args.IsEmpty)
            {
                if (!IsEmpty) ClearAndRaise();
            }
            else
            {
                var NewValue = Args.Value;
                if (IsEmpty || !ValueEquals(NewValue)) SetAndRaise(NewValue);
            }
        }
    }

    void BoundPropertyWithDifferentType_Changed(object Sender, object Args)
    {
        // If Sender and BoundProperty aren't the same, ignore this Changed call. The new BoundProperty should call Changed event handler with correct value.
        if (Sender != BoundProperty) return;

        var ConverterResult = (ChangedEventArgs<T>)Converter.DynamicInvoke(Args);

        if (ConverterResult.IsEmpty)
        {
            if (!IsEmpty) ClearAndRaise();
        }
        else
        {
            if (IsEmpty || !Equals(Value, ConverterResult.Value)) SetAndRaise(ConverterResult.Value);
        }
    }

    public struct SetDelegates
    {
        public SetValueDelegate SetValue;
        public Action ClearValue;
    }

    public struct BindDelegates
    {
        public BindDelegate Bind;
        public Action Unbind;
        public MonitorDelegate Monitor;
        public Action StartMonitoring;
        public Action StopMonitoring;

        internal PrivatelySettablePrivatelyBindableProperty<T> Owner;

        public readonly void BindExtended<TTarget>(PrivatelySettablePrivatelyBindableProperty<TTarget> Target, Func<ChangedEventArgs<TTarget>, ChangedEventArgs<T>> PrimaryConverter, bool TwoWay = false, Func<ChangedEventArgs<T>, ChangedEventArgs<TTarget>> SecondaryConverter = null) => Owner.BindExtended(Target, PrimaryConverter, TwoWay, SecondaryConverter);
    }

    public delegate void BindDelegate(PrivatelySettablePrivatelyBindableProperty<T> Target, bool TwoWay = false);

    public delegate void SetValueDelegate(T Value);

    public delegate void CoerceValueDelegate(CoerceValueDelegateArgs<T> Args);

    public delegate void MonitorDelegate(BindablePropertyBase Target);
}