// ReSharper disable ObjectCreationAsStatement

using NUnit.Framework;

namespace Atesh.BindableProperties.Test;

[TestFixture]
public class PrivatelySettablePrivatelyBindablePropertyTests
{
    [Test]
    public void Constructors_ParameterValidation()
    {
        var E = Assert.Throws<ArgumentNullException>(() => new PrivatelySettablePrivatelyBindableProperty<int>(null, out _, out _));
        Assert.AreEqual(E.ParamName, nameof(PrivatelySettablePrivatelyBindableProperty<int>.Owner));

        E = Assert.Throws<ArgumentNullException>(() => new PrivatelySettablePrivatelyBindableProperty<int>(null, out _, out _, 0));
        Assert.AreEqual(E.ParamName, nameof(PrivatelySettablePrivatelyBindableProperty<int>.Owner));
    }

    [Test]
    public void Constructors_ReturnMethods()
    {
        new PrivatelySettablePrivatelyBindableProperty<int>(this, out var SetMethods, out var BindMethods);

        if (SetMethods.SetValue == null) Assert.Fail();
        if (SetMethods.ClearValue == null) Assert.Fail();
        if (BindMethods.Bind == null) Assert.Fail();
        if (BindMethods.Unbind == null) Assert.Fail();

        new PrivatelySettablePrivatelyBindableProperty<int>(this, out SetMethods, out BindMethods, 0);

        if (SetMethods.SetValue == null) Assert.Fail();
        if (SetMethods.ClearValue == null) Assert.Fail();
        if (BindMethods.Bind == null) Assert.Fail();
        if (BindMethods.Unbind == null) Assert.Fail();
    }

    [Test]
    public void Constructor_StoresCorrectValue()
    {
        const int Value = 1;
        var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out _, Value);
        var PropertyValue = int.MinValue;
        var PropertyIsEmpty = false;
        Property.Changed += Property_Changed;

        void Property_Changed(PrivatelySettablePrivatelyBindableProperty<int> Sender, ChangedEventArgs<int> Args)
        {
            Property.Changed -= Property_Changed;
            PropertyValue = Args.Value;
            PropertyIsEmpty = Args.IsEmpty;
        }

        Assert.AreEqual(PropertyValue, Value);
        Assert.False(PropertyIsEmpty);
    }

    [Test]
    public void Constructor_StoresEmptyValue()
    {
        var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out _, true);
        var PropertyIsEmpty = false;
        Property.Changed += Property_Changed;

        void Property_Changed(PrivatelySettablePrivatelyBindableProperty<int> Sender, ChangedEventArgs<int> Args)
        {
            Property.Changed -= Property_Changed;
            PropertyIsEmpty = Args.IsEmpty;
        }

        Assert.True(PropertyIsEmpty);
    }

    [Test]
    public void SetValue_DoesNotRaiseChangedEventWithSameValue()
    {
        var PropertyValueReceivedOnce = false;

        var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out var SetMethods, out _);
        Property.Changed += delegate
        {
            if (PropertyValueReceivedOnce) Assert.Fail();
            else PropertyValueReceivedOnce = true;
        };

        SetMethods.SetValue(default);
    }

    [Test]
    public void SetValue_RaisesChangedEventWithCorrectParameters()
    {
        const int NewValue = 1;
        var PropertyValueReceivedOnce = false;

        var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out var SetMethods, out _);
        Property.Changed += (Sender, Args) =>
        {
            if (PropertyValueReceivedOnce)
            {
                Assert.AreEqual(Sender, Property);
                Assert.AreEqual(Args.Value, NewValue);
                Assert.False(Args.IsEmpty);
                Assert.Pass();
            }
            else PropertyValueReceivedOnce = true;
        };

        SetMethods.SetValue(NewValue);
        Assert.Fail();
    }

    [Test]
    public void SetValue_Unbinds()
    {
        var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out var SetMethods, out var BindMethods);
        var TargetProperty = new BindableProperty<int>(this);

        BindMethods.Bind(TargetProperty);
        SetMethods.SetValue(0);
        Assert.Null(Property.BoundProperty);
    }

    [Test]
    public void ClearValue_DoesNotRaiseChangedEventWhileEmpty()
    {
        var PropertyValueReceivedOnce = false;

        var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out var SetMethods, out _, true);
        Property.Changed += delegate
        {
            if (PropertyValueReceivedOnce) Assert.Fail();
            else PropertyValueReceivedOnce = true;
        };

        SetMethods.ClearValue();
    }

    [Test]
    public void ClearValue_RaisesChangedEventWithCorrectParameters()
    {
        var PropertyValueReceivedOnce = false;

        var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out var SetMethods, out _);
        Property.Changed += (Sender, Args) =>
        {
            if (PropertyValueReceivedOnce)
            {
                Assert.AreEqual(Sender, Property);
                Assert.True(Args.IsEmpty);
                Assert.Pass();
            }
            else PropertyValueReceivedOnce = true;
        };

        SetMethods.ClearValue();
        Assert.Fail();
    }

    [Test]
    public void ClearValue_Unbinds()
    {
        var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out var SetMethods, out var BindMethods);
        var TargetProperty = new BindableProperty<int>(this);

        BindMethods.Bind(TargetProperty);
        SetMethods.ClearValue();
        Assert.Null(Property.BoundProperty);
    }

    [Test]
    public void ChangedAdder_RaisesChangedEventImmediatelyWithCorrectParameters()
    {
        var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out _);
        Property.Changed += (Sender, Args) =>
        {
            Assert.AreEqual(Sender, Property);
            Assert.AreEqual(Args.Value, default(int));
            Assert.False(Args.IsEmpty);
            Assert.Pass();
        };

        Assert.Fail();
    }

    [Test]
    public void Bind_ParameterValidation()
    {
        var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindMethods);

        var E = Assert.Throws<ArgumentNullException>(() => BindMethods.Bind(null));
        Assert.AreEqual(E.ParamName, "Target");

        var E2 = Assert.Throws<ArgumentException>(() => BindMethods.Bind(Property));
        Assert.AreEqual(E2.ParamName, "Target");
        Assert.True(E2.Message.Contains(Strings.PropertyCanNotBindToItself));
    }

    [Test]
    public void Bind_Binds()
    {
        var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindMethods);
        var TargetProperty = new BindableProperty<int>(this);

        Assert.Null(Property.BoundProperty);
        BindMethods.Bind(TargetProperty);
        Assert.NotNull(Property.BoundProperty);
    }

    [Test]
    public void Bind_RaisesChangedEventWithCorrectParameters()
    {
        const int ValueOfTarget = 3;
        var PropertyValueReceivedOnce = false;

        var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindMethods);
        var TargetProperty = new BindableProperty<int>(this, ValueOfTarget);

        Property.Changed += (Sender, Args) =>
        {
            if (PropertyValueReceivedOnce)
            {
                Assert.AreEqual(Sender, Property);
                Assert.AreEqual(Args.Value, ValueOfTarget);
                Assert.False(Args.IsEmpty);
                Assert.Pass();
            }
            else PropertyValueReceivedOnce = true;
        };

        BindMethods.Bind(TargetProperty);
        Assert.Fail();
    }

    [Test]
    public void Bind_DoesNotRaiseChangedEventWithSameValue()
    {
        var PropertyValueReceivedOnce = false;

        var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindMethods);
        var TargetProperty = new BindableProperty<int>(this);

        Property.Changed += delegate
        {
            if (PropertyValueReceivedOnce) Assert.Fail();
            else PropertyValueReceivedOnce = true;
        };

        BindMethods.Bind(TargetProperty);
    }

    [Test]
    public void Bind_TwoWayBinding()
    {
        var Counter = 0;
        var ValueA = 0;
        var ValueB = 0;

        void Property_ChangedA(PrivatelySettablePrivatelyBindableProperty<int> Sender, ChangedEventArgs<int> Args)
        {
            // ReSharper disable once AccessToModifiedClosure
            Counter++;
            ValueA = Args.Value;
        }

        void Property_ChangedB(PrivatelySettablePrivatelyBindableProperty<int> Sender, ChangedEventArgs<int> Args)
        {
            // ReSharper disable once AccessToModifiedClosure
            Counter++;
            ValueB = Args.Value;
        }

        var PropertyA = new PrivatelySettablePrivatelyBindableProperty<int>(this, out var SetMethodsA, out var BindMethodsA);
        var PropertyB = new PrivatelySettablePrivatelyBindableProperty<int>(this, out var SetMethodsB, out _);

        PropertyA.Changed += Property_ChangedA;
        PropertyB.Changed += Property_ChangedB;

        BindMethodsA.Bind(PropertyB, true);

        Assert.AreEqual(PropertyB, PropertyA.BoundProperty);
        Assert.AreEqual(PropertyA, PropertyB.BoundProperty);
        Assert.AreEqual(2, Counter);

        Counter = 0;
        SetMethodsA.SetValue(3);
        Assert.AreEqual(PropertyB, PropertyA.BoundProperty);

        Assert.AreEqual(2, Counter);
        Assert.AreEqual(3, ValueA);
        Assert.AreEqual(3, ValueB);

        Counter = 0;
        SetMethodsB.SetValue(5);
        Assert.AreEqual(PropertyA, PropertyB.BoundProperty);

        Assert.AreEqual(2, Counter);
        Assert.AreEqual(5, ValueA);
        Assert.AreEqual(5, ValueB);
    }

    [Test]
    public void BindExtended_ParameterValidation()
    {
        var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindMethods);
        var TargetProperty = new BindableProperty<string>(this);
        var TargetProperty2 = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out _);

        var E = Assert.Throws<ArgumentNullException>(() => BindMethods.BindExtended<string>(null, null));
        Assert.AreEqual(E.ParamName, "Target");

        var E2 = Assert.Throws<ArgumentNullException>(() => BindMethods.BindExtended(TargetProperty, null));
        Assert.AreEqual(E2.ParamName, "PrimaryConverter");

        Assert.DoesNotThrow(() => BindMethods.BindExtended(TargetProperty, X => new(false, Convert.ToInt32(X.Value))));

        var E3 = Assert.Throws<ArgumentNullException>(() => BindMethods.BindExtended(TargetProperty, X => new(false, Convert.ToInt32(X.Value)), true));
        Assert.AreEqual(E3.ParamName, "SecondaryConverter");

        Assert.DoesNotThrow(() => BindMethods.BindExtended(TargetProperty2, X => new(false, Convert.ToInt32(X.Value))));

        var E4 = Assert.Throws<ArgumentException>(() => BindMethods.BindExtended(Property, X => new(false, Convert.ToInt32(X.Value))));
        Assert.AreEqual(E4.ParamName, "Target");
        Assert.True(E4.Message.Contains(Strings.PropertyCanNotBindToItself));
    }

    [Test]
    public void BindExtended_Binds()
    {
        var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindMethods);
        var TargetProperty = new BindableProperty<string>(this);

        Assert.Null(Property.BoundProperty);
        BindMethods.BindExtended(TargetProperty, X => new(false, Convert.ToInt32(X.Value)));
        Assert.NotNull(Property.BoundProperty);
    }

    [Test]
    public void BindExtended_RaisesChangedEventWithCorrectParameters()
    {
        const string ValueOfTarget = "3";
        var PropertyValueReceivedOnce = false;

        var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindMethods);
        var TargetProperty = new BindableProperty<string>(this, ValueOfTarget);

        Property.Changed += (Sender, Args) =>
        {
            if (PropertyValueReceivedOnce)
            {
                Assert.AreEqual(Sender, Property);
                Assert.AreEqual(Args.Value, Convert.ToInt32(ValueOfTarget));
                Assert.False(Args.IsEmpty);
                Assert.Pass();
            }
            else PropertyValueReceivedOnce = true;
        };

        BindMethods.BindExtended(TargetProperty, X => new(false, Convert.ToInt32(X.Value)));
        Assert.Fail();
    }

    [Test]
    public void BindExtended_DoesNotRaiseChangedEventWithSameValue()
    {
        var PropertyValueReceivedOnce = false;

        var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindMethods);
        var TargetProperty = new BindableProperty<string>(this);

        Property.Changed += delegate
        {
            // ReSharper disable once AccessToModifiedClosure
            if (PropertyValueReceivedOnce) Assert.Fail();
            else PropertyValueReceivedOnce = true;
        };

        PropertyValueReceivedOnce = false;
        BindMethods.BindExtended(TargetProperty, X => new(false, Convert.ToInt32(X.Value)));
    }

    [Test]
    public void BindExtended_TwoWayBinding()
    {
        var Counter = 0;
        var ValueA = 0;
        var ValueB = "0";

        void Property_ChangedA(PrivatelySettablePrivatelyBindableProperty<int> Sender, ChangedEventArgs<int> Args)
        {
            // ReSharper disable once AccessToModifiedClosure
            Counter++;
            ValueA = Args.Value;
        }

        void Property_ChangedB(PrivatelySettablePrivatelyBindableProperty<string> Sender, ChangedEventArgs<string> Args)
        {
            // ReSharper disable once AccessToModifiedClosure
            Counter++;
            ValueB = Args.Value;
        }

        var PropertyA = new PrivatelySettablePrivatelyBindableProperty<int>(this, out var SetMethodsA, out var BindMethodsA);
        var PropertyB = new PrivatelySettablePrivatelyBindableProperty<string>(this, out var SetMethodsB, out _, "0");

        PropertyA.Changed += Property_ChangedA;
        PropertyB.Changed += Property_ChangedB;

        BindMethodsA.BindExtended(PropertyB, X => new(false, Convert.ToInt32(X.Value)), true, X => new(false, X.Value.ToString()));

        Assert.AreEqual(PropertyB, PropertyA.BoundProperty);
        Assert.AreEqual(PropertyA, PropertyB.BoundProperty);
        Assert.AreEqual(2, Counter);

        Counter = 0;
        SetMethodsA.SetValue(3);
        Assert.AreEqual(PropertyB, PropertyA.BoundProperty);

        Assert.AreEqual(2, Counter);
        Assert.AreEqual(3, ValueA);
        Assert.AreEqual("3", ValueB);

        Counter = 0;
        SetMethodsB.SetValue("5");
        Assert.AreEqual(PropertyA, PropertyB.BoundProperty);

        Assert.AreEqual(2, Counter);
        Assert.AreEqual(5, ValueA);
        Assert.AreEqual("5", ValueB);
    }

    [Test]
    public void Unbind_Unbinds()
    {
        var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindMethods);
        var TargetProperty = new BindableProperty<int>(this);

        BindMethods.Bind(TargetProperty);
        BindMethods.Unbind();
        Assert.Null(Property.BoundProperty);
    }

    [Test]
    public void Unbind_ThrowsExceptionWhileUnbound()
    {
        new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindMethods);

        var E = Assert.Throws<InvalidOperationException>(() => BindMethods.Unbind());
        Assert.AreEqual(Strings.PropertyNotBoundYet, E.Message);
    }

    [Test]
    public void Unbind_DoesNotRaiseChangedEvent()
    {
        const int ValueOfTarget = 3;
        var PropertyValueReceivedOnce = false;

        var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindMethods);
        var TargetProperty = new BindableProperty<int>(this, ValueOfTarget);

        BindMethods.Bind(TargetProperty);

        Property.Changed += delegate
        {
            if (PropertyValueReceivedOnce) Assert.Fail();
            else PropertyValueReceivedOnce = true;
        };

        BindMethods.Unbind();
    }

    [Test]
    public void StartMonitoring_StartsMonitoring()
    {
        var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindMethods);

        Assert.False(Property.IsMonitoringWithoutBinding);
        BindMethods.StartMonitoring();
        Assert.True(Property.IsMonitoringWithoutBinding);
    }

    [Test]
    public void StartMonitoring_ThrowsExceptionWhileMonitoringWithoutBinding()
    {
        new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindMethods);

        BindMethods.StartMonitoring();

        var E = Assert.Throws<InvalidOperationException>(() => BindMethods.StartMonitoring());
        Assert.AreEqual(Strings.MonitoringAlreadyStarted, E.Message);
    }

    [Test]
    public void StopMonitoring_StopsMonitoring()
    {
        var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindMethods);

        BindMethods.StartMonitoring();
        BindMethods.StopMonitoring();
        Assert.False(Property.IsMonitoringWithoutBinding);
    }

    [Test]
    public void StopMonitoring_ThrowsExceptionWhileNotMonitoringWithoutBinding()
    {
        new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindMethods);

        var E = Assert.Throws<InvalidOperationException>(() => BindMethods.StopMonitoring());
        Assert.AreEqual(Strings.MonitoringNotStartedYet, E.Message);
    }

    [Test]
    public void Monitor_ParameterValidation()
    {
        var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindMethods);

        var E = Assert.Throws<ArgumentNullException>(() => BindMethods.Monitor(null));
        Assert.AreEqual(E.ParamName, "Target");

        var E2 = Assert.Throws<ArgumentException>(() => BindMethods.Monitor(Property));
        Assert.AreEqual(E2.ParamName, "Target");
        Assert.True(E2.Message.Contains(Strings.PropertyCanNotMonitorItself));
    }

    [Test]
    public void Monitor_Monitors()
    {
        new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindMethods);
        var MonitoredProperty = new BindableProperty<DateTime>(this);

        BindMethods.Monitor(MonitoredProperty);
    }

    [Test]
    public void Monitor_ThrowsExceptionWhileBound()
    {
        new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindMethods);
        var TargetProperty = new BindableProperty<int>(this);
        var MonitoredProperty = new BindableProperty<DateTime>(this);

        BindMethods.Bind(TargetProperty);

        var E = Assert.Throws<InvalidOperationException>(() => BindMethods.Monitor(MonitoredProperty));
        Assert.AreEqual(Strings.PropertyCanNotMonitorAfterMonitoringStarted, E.Message);
    }

    [Test]
    public void Monitor_ThrowsExceptionWhileMonitoringWithoutBinding()
    {
        new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindMethods);
        var MonitoredProperty = new BindableProperty<DateTime>(this);

        BindMethods.StartMonitoring();

        var E = Assert.Throws<InvalidOperationException>(() => BindMethods.Monitor(MonitoredProperty));
        Assert.AreEqual(Strings.PropertyCanNotMonitorAfterMonitoringStarted, E.Message);
    }

    [Test]
    public void ChangedEventOfMonitoredProperty_Unbinds()
    {
        var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindMethods);
        var TargetProperty = new BindableProperty<int>(this);
        var MonitoredProperty = new BindableProperty<DateTime>(this);

        BindMethods.Monitor(MonitoredProperty);
        BindMethods.Bind(TargetProperty);

        MonitoredProperty.SetValue(DateTime.Now);
        Assert.Null(Property.BoundProperty);
    }

    [Test]
    public void ChangedEventOfMonitoredProperty_StopsMonitoring()
    {
        var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindMethods);
        var MonitoredProperty = new BindableProperty<DateTime>(this);

        BindMethods.Monitor(MonitoredProperty);
        BindMethods.StartMonitoring();

        MonitoredProperty.SetValue(DateTime.Now);
        Assert.False(Property.IsMonitoringWithoutBinding);
    }

    [Test]
    public void ChangedEventOfMonitoredProperty_CallsBinderCallbackWhileBound()
    {
        new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindMethods, Binder: Assert.Pass);
        var TargetProperty = new BindableProperty<int>(this);
        var MonitoredProperty = new BindableProperty<DateTime>(this);

        BindMethods.Monitor(MonitoredProperty);
        BindMethods.Bind(TargetProperty);

        MonitoredProperty.SetValue(DateTime.Now);
        Assert.Fail();
    }

    [Test]
    public void ChangedEventOfMonitoredProperty_CallsBinderCallbackWhileMonitoringWithoutBinding()
    {
        new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindMethods, Binder: Assert.Pass);
        var MonitoredProperty = new BindableProperty<DateTime>(this);

        BindMethods.Monitor(MonitoredProperty);
        BindMethods.StartMonitoring();

        MonitoredProperty.SetValue(DateTime.Now);
        Assert.Fail();
    }
}