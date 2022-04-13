// ReSharper disable ObjectCreationAsStatement

using NUnit.Framework;

namespace Atesh.BindableProperties.Test;

[TestFixture]
public class PrivatelySettableBindablePropertyTests
{
    [Test]
    public void Constructors_ParameterValidation()
    {
        var E = Assert.Throws<ArgumentNullException>(() => new PrivatelySettableBindableProperty<int>(null, out _));
        Assert.AreEqual(E.ParamName, nameof(PrivatelySettableBindableProperty<int>.Owner));

        E = Assert.Throws<ArgumentNullException>(() => new PrivatelySettableBindableProperty<int>(null, out _, 0));
        Assert.AreEqual(E.ParamName, nameof(PrivatelySettableBindableProperty<int>.Owner));
    }

    [Test]
    public void Constructor_StoresCorrectValue()
    {
        const int Value = 1;
        var Property = new PrivatelySettableBindableProperty<int>(this, out _, Value);
        Assert.AreEqual(Property.GetValueVeryExpensively(out var IsEmpty), Value);
        Assert.False(IsEmpty);
    }

    [Test]
    public void Bind_ParameterValidation()
    {
        var Property = new PrivatelySettableBindableProperty<int>(this, out _);

        var E = Assert.Throws<ArgumentNullException>(() => Property.Bind(null));
        Assert.AreEqual(E.ParamName, "Target");

        var E2 = Assert.Throws<ArgumentException>(() => Property.Bind(Property));
        Assert.AreEqual(E2.ParamName, "Target");
        Assert.True(E2.Message.Contains(Strings.PropertyCanNotBindToItself));
    }

    [Test]
    public void Bind_Binds()
    {
        var Property = new PrivatelySettableBindableProperty<int>(this, out _);
        var TargetProperty = new PrivatelySettableBindableProperty<int>(this, out _);

        Assert.Null(Property.BoundProperty);
        Property.Bind(TargetProperty);
        Assert.NotNull(Property.BoundProperty);
    }

    [Test]
    public void Bind_RaisesChangedEventWithCorrectParameters()
    {
        const int ValueOfTarget = 3;
        var PropertyValueReceivedOnce = false;

        var Property = new PrivatelySettableBindableProperty<int>(this, out _);
        var TargetProperty = new PrivatelySettableBindableProperty<int>(this, out _, ValueOfTarget);

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

        Property.Bind(TargetProperty);
        Assert.Fail();
    }

    [Test]
    public void BindDelegate_TwoWayBinding()
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

        var PropertyA = new PrivatelySettableBindableProperty<int>(this, out var SetDelegatesA);
        var PropertyB = new PrivatelySettableBindableProperty<int>(this, out var SetDelegatesB);

        PropertyA.Changed += Property_ChangedA;
        PropertyB.Changed += Property_ChangedB;

        PropertyA.Bind(PropertyB, true);

        Assert.AreEqual(PropertyB, PropertyA.BoundProperty);
        Assert.AreEqual(PropertyA, PropertyB.BoundProperty);
        Assert.AreEqual(2, Counter);

        Counter = 0;
        SetDelegatesA.SetValue(3);
        Assert.AreEqual(PropertyB, PropertyA.BoundProperty);

        Assert.AreEqual(2, Counter);
        Assert.AreEqual(3, ValueA);
        Assert.AreEqual(3, ValueB);

        Counter = 0;
        SetDelegatesB.SetValue(5);
        Assert.AreEqual(PropertyA, PropertyB.BoundProperty);

        Assert.AreEqual(2, Counter);
        Assert.AreEqual(5, ValueA);
        Assert.AreEqual(5, ValueB);
    }

    [Test]
    public void BindExtended_ParameterValidation()
    {
        var Property = new PrivatelySettableBindableProperty<int>(this, out _);
        var TargetProperty = new BindableProperty<string>(this);
        var TargetProperty2 = new PrivatelySettableBindableProperty<int>(this, out _);

        var E = Assert.Throws<ArgumentNullException>(() => Property.BindExtended<string>(null, null));
        Assert.AreEqual(E.ParamName, "Target");

        var E2 = Assert.Throws<ArgumentNullException>(() => Property.BindExtended(TargetProperty, null));
        Assert.AreEqual(E2.ParamName, "PrimaryConverter");

        Assert.DoesNotThrow(() => Property.BindExtended(TargetProperty, Convert.ToInt32));

        var E3 = Assert.Throws<ArgumentNullException>(() => Property.BindExtended(TargetProperty, Convert.ToInt32, true));
        Assert.AreEqual(E3.ParamName, "SecondaryConverter");

        Assert.DoesNotThrow(() => Property.BindExtended(TargetProperty2, Convert.ToInt32));

        var E4 = Assert.Throws<ArgumentException>(() => Property.BindExtended(Property, Convert.ToInt32));
        Assert.AreEqual(E4.ParamName, "Target");
        Assert.True(E4.Message.Contains(Strings.PropertyCanNotBindToItself));
    }

    [Test]
    public void BindExtended_Binds()
    {
        var Property = new PrivatelySettableBindableProperty<int>(this, out _);
        var TargetProperty = new BindableProperty<string>(this);

        Assert.Null(Property.BoundProperty);
        Property.BindExtended(TargetProperty, Convert.ToInt32);
        Assert.NotNull(Property.BoundProperty);
    }

    [Test]
    public void BindExtended_RaisesChangedEventWithCorrectParameters()
    {
        const string ValueOfTarget = "3";
        var PropertyValueReceivedOnce = false;

        var Property = new PrivatelySettableBindableProperty<int>(this, out _);
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

        Property.BindExtended(TargetProperty, Convert.ToInt32);
        Assert.Fail();
    }

    [Test]
    public void BindExtended_DoesNotRaiseChangedEventWithSameValue()
    {
        var PropertyValueReceivedOnce = false;

        var Property = new PrivatelySettableBindableProperty<int>(this, out _);
        var TargetProperty = new BindableProperty<string>(this);

        Property.Changed += delegate
        {
            if (PropertyValueReceivedOnce) Assert.Fail();
            else PropertyValueReceivedOnce = true;
        };

        Property.BindExtended(TargetProperty, Convert.ToInt32);
    }

    [Test]
    public void Unbind_Unbinds()
    {
        var Property = new PrivatelySettableBindableProperty<int>(this, out _);
        var TargetProperty = new PrivatelySettableBindableProperty<int>(this, out _);

        Property.Bind(TargetProperty);
        Property.Unbind();
        Assert.Null(Property.BoundProperty);
    }

    [Test]
    public void Unbind_ThrowsExceptionWhileUnbound()
    {
        var Property = new PrivatelySettableBindableProperty<int>(this, out _);

        var E = Assert.Throws<InvalidOperationException>(() => Property.Unbind());
        Assert.AreEqual(Strings.PropertyNotBoundYet, E.Message);
    }

    [Test]
    public void Unbind_DoesNotRaiseChangedEvent()
    {
        const int ValueOfTarget = 3;
        var PropertyValueReceivedOnce = false;

        var Property = new PrivatelySettableBindableProperty<int>(this, out _);
        var TargetProperty = new PrivatelySettableBindableProperty<int>(this, out _, ValueOfTarget);

        Property.Bind(TargetProperty);

        Property.Changed += delegate
        {
            if (PropertyValueReceivedOnce) Assert.Fail();
            else PropertyValueReceivedOnce = true;
        };

        Property.Unbind();
    }

    [Test]
    public void StartMonitoring_StartsMonitoring()
    {
        var Property = new PrivatelySettableBindableProperty<int>(this, out _);

        Assert.False(Property.IsMonitoringWithoutBinding);
        Property.StartMonitoring();
        Assert.True(Property.IsMonitoringWithoutBinding);
    }

    [Test]
    public void StartMonitoring_ThrowsExceptionWhileMonitoringWithoutBinding()
    {
        var Property = new PrivatelySettableBindableProperty<int>(this, out _);

        Property.StartMonitoring();

        var E = Assert.Throws<InvalidOperationException>(() => Property.StartMonitoring());
        Assert.AreEqual(Strings.MonitoringAlreadyStarted, E.Message);
    }

    [Test]
    public void StopMonitoring_StopsMonitoring()
    {
        var Property = new PrivatelySettableBindableProperty<int>(this, out _);

        Property.StartMonitoring();
        Property.StopMonitoring();
        Assert.False(Property.IsMonitoringWithoutBinding);
    }

    [Test]
    public void StopMonitoring_ThrowsExceptionWhileNotMonitoringWithoutBinding()
    {
        var Property = new PrivatelySettableBindableProperty<int>(this, out _);

        var E = Assert.Throws<InvalidOperationException>(() => Property.StopMonitoring());
        Assert.AreEqual(Strings.MonitoringNotStartedYet, E.Message);
    }

    [Test]
    public void Monitor_ParameterValidation()
    {
        var Property = new PrivatelySettableBindableProperty<int>(this, out _);

        var E = Assert.Throws<ArgumentNullException>(() => Property.Monitor(null));
        Assert.AreEqual(E.ParamName, "Target");

        var E2 = Assert.Throws<ArgumentException>(() => Property.Monitor(Property));
        Assert.AreEqual(E2.ParamName, "Target");
        Assert.True(E2.Message.Contains(Strings.PropertyCanNotMonitorItself));
    }

    [Test]
    public void Monitor_Monitors()
    {
        var Property = new PrivatelySettableBindableProperty<int>(this, out _);
        var MonitoredProperty = new PrivatelySettableBindableProperty<DateTime>(this, out _);

        Property.Monitor(MonitoredProperty);
    }

    [Test]
    public void Monitor_ThrowsExceptionWhileBound()
    {
        var Property = new PrivatelySettableBindableProperty<int>(this, out _);
        var TargetProperty = new PrivatelySettableBindableProperty<int>(this, out _);
        var MonitoredProperty = new PrivatelySettableBindableProperty<DateTime>(this, out _);

        Property.Bind(TargetProperty);

        var E = Assert.Throws<InvalidOperationException>(() => Property.Monitor(MonitoredProperty));
        Assert.AreEqual(Strings.PropertyCanNotMonitorAfterMonitoringStarted, E.Message);
    }

    [Test]
    public void Monitor_ThrowsExceptionWhileMonitoringWithoutBinding()
    {
        var Property = new PrivatelySettableBindableProperty<int>(this, out _);
        var MonitoredProperty = new PrivatelySettableBindableProperty<DateTime>(this, out _);

        Property.StartMonitoring();

        var E = Assert.Throws<InvalidOperationException>(() => Property.Monitor(MonitoredProperty));
        Assert.AreEqual(Strings.PropertyCanNotMonitorAfterMonitoringStarted, E.Message);
    }
}