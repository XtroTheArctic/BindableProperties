// ReSharper disable ObjectCreationAsStatement

using NUnit.Framework;

namespace Atesh.BindableProperties.Test;

[TestFixture]
public class BindablePropertyTests
{
    [Test]
    public void Constructors_ParameterValidation()
    {
        var E = Assert.Throws<ArgumentNullException>(() => new BindableProperty<int>(null));
        Assert.AreEqual(E.ParamName, nameof(BindableProperty<int>.Owner));

        E = Assert.Throws<ArgumentNullException>(() => new BindableProperty<int>(null, 0));
        Assert.AreEqual(E.ParamName, nameof(BindableProperty<int>.Owner));
    }

    [Test]
    public void Constructor_StoresCorrectValue()
    {
        const int Value = 1;
        var Property = new BindableProperty<int>(this, Value);
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
    public void Bind_ParameterValidation()
    {
        var Property = new BindableProperty<int>(this);

        var E = Assert.Throws<ArgumentNullException>(() => Property.Bind(null));
        Assert.AreEqual(E.ParamName, "Target");

        var E2 = Assert.Throws<ArgumentException>(() => Property.Bind(Property));
        Assert.AreEqual(E2.ParamName, "Target");
        Assert.True(E2.Message.Contains(Strings.PropertyCanNotBindToItself));
    }

    [Test]
    public void Bind_Binds()
    {
        var Property = new BindableProperty<int>(this);
        var TargetProperty = new BindableProperty<int>(this);

        Assert.Null(Property.BoundProperty);
        Property.Bind(TargetProperty);
        Assert.NotNull(Property.BoundProperty);
    }

    [Test]
    public void Bind_RaisesChangedEventWithCorrectParameters()
    {
        const int ValueOfTarget = 3;
        var PropertyValueReceivedOnce = false;

        var Property = new BindableProperty<int>(this);
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

        var PropertyA = new BindableProperty<int>(this);
        var PropertyB = new BindableProperty<int>(this);

        PropertyA.Changed += Property_ChangedA;
        PropertyB.Changed += Property_ChangedB;

        PropertyA.Bind(PropertyB, true);

        Assert.AreEqual(PropertyB, PropertyA.BoundProperty);
        Assert.AreEqual(PropertyA, PropertyB.BoundProperty);
        Assert.AreEqual(2, Counter);

        Counter = 0;
        PropertyA.SetValue(3);
        Assert.AreEqual(PropertyB, PropertyA.BoundProperty);

        Assert.AreEqual(2, Counter);
        Assert.AreEqual(3, ValueA);
        Assert.AreEqual(3, ValueB);

        Counter = 0;
        PropertyB.SetValue(5);
        Assert.AreEqual(PropertyA, PropertyB.BoundProperty);

        Assert.AreEqual(2, Counter);
        Assert.AreEqual(5, ValueA);
        Assert.AreEqual(5, ValueB);
    }

    [Test]
    public void BindExtended_ParameterValidation()
    {
        var Property = new BindableProperty<int>(this);
        var TargetProperty = new BindableProperty<string>(this);
        var TargetProperty2 = new BindableProperty<int>(this);

        var E = Assert.Throws<ArgumentNullException>(() => Property.BindExtended<string>(null, null));
        Assert.AreEqual(E.ParamName, "Target");

        var E2 = Assert.Throws<ArgumentNullException>(() => Property.BindExtended(TargetProperty, null));
        Assert.AreEqual(E2.ParamName, "PrimaryConverter");

        Assert.DoesNotThrow(() => Property.BindExtended(TargetProperty, X => new(false, Convert.ToInt32(X.Value))));

        var E3 = Assert.Throws<ArgumentNullException>(() => Property.BindExtended(TargetProperty, X => new(false, Convert.ToInt32(X.Value)), true));
        Assert.AreEqual(E3.ParamName, "SecondaryConverter");

        Assert.DoesNotThrow(() => Property.BindExtended(TargetProperty2, X => new(false, Convert.ToInt32(X.Value))));

        var E4 = Assert.Throws<ArgumentException>(() => Property.BindExtended(Property, X => new(false, Convert.ToInt32(X.Value))));
        Assert.AreEqual(E4.ParamName, "Target");
        Assert.True(E4.Message.Contains(Strings.PropertyCanNotBindToItself));
    }

    [Test]
    public void BindExtended_Binds()
    {
        var Property = new BindableProperty<int>(this);
        var TargetProperty = new BindableProperty<string>(this);

        Assert.Null(Property.BoundProperty);
        Property.BindExtended(TargetProperty, X => new(false, Convert.ToInt32(X.Value)));
        Assert.NotNull(Property.BoundProperty);
    }

    [Test]
    public void BindExtended_RaisesChangedEventWithCorrectParameters()
    {
        const string ValueOfTarget = "3";
        var PropertyValueReceivedOnce = false;

        var Property = new BindableProperty<int>(this);
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

        Property.BindExtended(TargetProperty, X => new(false, Convert.ToInt32(X.Value)));
        Assert.Fail();
    }

    [Test]
    public void BindExtended_DoesNotRaiseChangedEventWithSameValue()
    {
        var PropertyValueReceivedOnce = false;

        var Property = new BindableProperty<int>(this);
        var TargetProperty = new BindableProperty<string>(this);

        Property.Changed += delegate
        {
            // ReSharper disable once AccessToModifiedClosure
            if (PropertyValueReceivedOnce) Assert.Fail();
            else PropertyValueReceivedOnce = true;
        };

        PropertyValueReceivedOnce = false;
        Property.BindExtended(TargetProperty, X => new(false, Convert.ToInt32(X.Value)));
    }

    [Test]
    public void Unbind_Unbinds()
    {
        var Property = new BindableProperty<int>(this);
        var TargetProperty = new BindableProperty<int>(this);

        Property.Bind(TargetProperty);
        Property.Unbind();
        Assert.Null(Property.BoundProperty);
    }

    [Test]
    public void Unbind_ThrowsExceptionWhileUnbound()
    {
        var Property = new BindableProperty<int>(this);

        var E = Assert.Throws<InvalidOperationException>(() => Property.Unbind());
        Assert.AreEqual(Strings.PropertyNotBoundYet, E.Message);
    }

    [Test]
    public void Unbind_DoesNotRaiseChangedEvent()
    {
        const int ValueOfTarget = 3;
        var PropertyValueReceivedOnce = false;

        var Property = new BindableProperty<int>(this);
        var TargetProperty = new BindableProperty<int>(this, ValueOfTarget);

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
        var Property = new BindableProperty<int>(this);

        Assert.False(Property.IsMonitoringWithoutBinding);
        Property.StartMonitoring();
        Assert.True(Property.IsMonitoringWithoutBinding);
    }

    [Test]
    public void StartMonitoring_ThrowsExceptionWhileMonitoringWithoutBinding()
    {
        var Property = new BindableProperty<int>(this);

        Property.StartMonitoring();

        var E = Assert.Throws<InvalidOperationException>(() => Property.StartMonitoring());
        Assert.AreEqual(Strings.MonitoringAlreadyStarted, E.Message);
    }

    [Test]
    public void StopMonitoring_StopsMonitoring()
    {
        var Property = new BindableProperty<int>(this);

        Property.StartMonitoring();
        Property.StopMonitoring();
        Assert.False(Property.IsMonitoringWithoutBinding);
    }

    [Test]
    public void StopMonitoring_ThrowsExceptionWhileNotMonitoringWithoutBinding()
    {
        var Property = new BindableProperty<int>(this);

        var E = Assert.Throws<InvalidOperationException>(() => Property.StopMonitoring());
        Assert.AreEqual(Strings.MonitoringNotStartedYet, E.Message);
    }

    [Test]
    public void Monitor_ParameterValidation()
    {
        var Property = new BindableProperty<int>(this);

        var E = Assert.Throws<ArgumentNullException>(() => Property.Monitor(null));
        Assert.AreEqual(E.ParamName, "Target");

        var E2 = Assert.Throws<ArgumentException>(() => Property.Monitor(Property));
        Assert.AreEqual(E2.ParamName, "Target");
        Assert.True(E2.Message.Contains(Strings.PropertyCanNotMonitorItself));
    }

    [Test]
    public void Monitor_Monitors()
    {
        var Property = new BindableProperty<int>(this);
        var MonitoredProperty = new BindableProperty<DateTime>(this);

        Property.Monitor(MonitoredProperty);
    }

    [Test]
    public void Monitor_ThrowsExceptionWhileBound()
    {
        var Property = new BindableProperty<int>(this);
        var TargetProperty = new BindableProperty<int>(this);
        var MonitoredProperty = new BindableProperty<DateTime>(this);

        Property.Bind(TargetProperty);

        var E = Assert.Throws<InvalidOperationException>(() => Property.Monitor(MonitoredProperty));
        Assert.AreEqual(Strings.PropertyCanNotMonitorAfterMonitoringStarted, E.Message);
    }

    [Test]
    public void Monitor_ThrowsExceptionWhileMonitoringWithoutBinding()
    {
        var Property = new BindableProperty<int>(this);
        var MonitoredProperty = new BindableProperty<DateTime>(this);

        Property.StartMonitoring();

        var E = Assert.Throws<InvalidOperationException>(() => Property.Monitor(MonitoredProperty));
        Assert.AreEqual(Strings.PropertyCanNotMonitorAfterMonitoringStarted, E.Message);
    }
}