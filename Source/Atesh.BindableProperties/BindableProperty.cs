using System;

namespace Atesh.BindableProperties;

public partial class BindableProperty<T> : PrivatelyBindableProperty<T>
{
    readonly BindDelegates Delegates;

    public BindableProperty(object Owner, Action BinderCallback = null, CoerceValueDelegate CoerceValueCallback = null) : base(Owner, out TempDelegates, BinderCallback, CoerceValueCallback) => Delegates = TempDelegates;
    public BindableProperty(object Owner, T Value, Action BinderCallback = null, CoerceValueDelegate CoerceValueCallback = null) : base(Owner, out TempDelegates, Value, BinderCallback, CoerceValueCallback) => Delegates = TempDelegates;
    internal BindableProperty(object Owner, bool IsEmpty = false, Action BinderCallback = null, CoerceValueDelegate CoerceValueCallback = null) : base(Owner, out TempDelegates, IsEmpty, BinderCallback, CoerceValueCallback) => Delegates = TempDelegates;

    public void Bind(PrivatelySettablePrivatelyBindableProperty<T> Target, bool TwoWay = false) => Delegates.Bind(Target, TwoWay);
    public void BindExtended<TargetType>(PrivatelySettablePrivatelyBindableProperty<TargetType> Target, Func<ChangedEventArgs<TargetType>, ChangedEventArgs<T>> PrimaryConverter, bool TwoWay = false, Func<ChangedEventArgs<T>, ChangedEventArgs<TargetType>> SecondaryConverter = null) => Delegates.BindExtended(Target, PrimaryConverter, TwoWay, SecondaryConverter);
    public new void Unbind() => Delegates.Unbind();
    public void Monitor(BindablePropertyBase Target) => Delegates.Monitor(Target);
    public void StartMonitoring() => Delegates.StartMonitoring();
    public void StopMonitoring() => Delegates.StopMonitoring();
}