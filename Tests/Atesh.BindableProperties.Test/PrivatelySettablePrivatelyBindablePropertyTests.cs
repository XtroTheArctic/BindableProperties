using System;
using NUnit.Framework;

namespace Atesh.BindableProperties.Test
{
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
        public void Constructors_ReturnDelegates()
        {
            new PrivatelySettablePrivatelyBindableProperty<int>(this, out var SetDelegates, out var BindDelegates);

            if (SetDelegates.SetValue == null) Assert.Fail();
            if (SetDelegates.ClearValue == null) Assert.Fail();
            if (BindDelegates.Bind == null) Assert.Fail();
            if (BindDelegates.Unbind == null) Assert.Fail();

            new PrivatelySettablePrivatelyBindableProperty<int>(this, out SetDelegates, out BindDelegates, 0);

            if (SetDelegates.SetValue == null) Assert.Fail();
            if (SetDelegates.ClearValue == null) Assert.Fail();
            if (BindDelegates.Bind == null) Assert.Fail();
            if (BindDelegates.Unbind == null) Assert.Fail();
        }

        [Test]
        public void Constructor_StoresCorrectValue()
        {
            const int Value = 1;
            var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out _, Value);
            Property.Changed += (Sender, Args) =>
            {
                Assert.AreEqual(Args.Value, Value);
                Assert.False(Args.IsEmpty);
                Assert.Pass();
            };

            Assert.Fail();
        }

        [Test]
        public void Constructor_StoresEmptyValue()
        {
            var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out _, true);
            Property.Changed += (Sender, Args) =>
            {
                Assert.True(Args.IsEmpty);
                Assert.Pass();
            };

            Assert.Fail();
        }

        [Test]
        public void SetValueDelegate_DoesNotRaiseChangedEventWithSameValue()
        {
            var PropertyValueReceivedOnce = false;

            var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out var SetDelegates, out _);
            Property.Changed += (Sender, Args) =>
            {
                if (PropertyValueReceivedOnce) Assert.Fail();
                else PropertyValueReceivedOnce = true;
            };

            SetDelegates.SetValue(default(int));
        }

        [Test]
        public void SetValueDelegate_RaisesChangedEventWithCorrectParameters()
        {
            const int NewValue = 1;
            var PropertyValueReceivedOnce = false;

            var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out var SetDelegates, out _);
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

            SetDelegates.SetValue(NewValue);
            Assert.Fail();
        }

        [Test]
        public void SetValueDelegate_Unbinds()
        {
            var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out var SetDelegates, out var BindDelegates);
            var TargetProperty = new BindableProperty<int>(this);

            BindDelegates.Bind(TargetProperty);
            SetDelegates.SetValue(0);
            Assert.Null(Property.BoundProperty);
        }

        [Test]
        public void ClearValueDelegate_DoesNotRaiseChangedEventWhileEmpty()
        {
            var PropertyValueReceivedOnce = false;

            var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out var SetDelegates, out _, true);
            Property.Changed += (Sender, Args) =>
            {
                if (PropertyValueReceivedOnce) Assert.Fail();
                else PropertyValueReceivedOnce = true;
            };

            SetDelegates.ClearValue();
        }

        [Test]
        public void ClearValueDelegate_RaisesChangedEventWithCorrectParameters()
        {
            var PropertyValueReceivedOnce = false;

            var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out var SetDelegates, out _);
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

            SetDelegates.ClearValue();
            Assert.Fail();
        }

        [Test]
        public void ClearValueDelegate_Unbinds()
        {
            var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out var SetDelegates, out var BindDelegates);
            var TargetProperty = new BindableProperty<int>(this);

            BindDelegates.Bind(TargetProperty);
            SetDelegates.ClearValue();
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
        public void BindDelegate_ParameterValidation()
        {
            var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindDelegates);

            var E = Assert.Throws<ArgumentNullException>(() => BindDelegates.Bind(null));
            Assert.AreEqual(E.ParamName, "Target");

            var E2 = Assert.Throws<ArgumentException>(() => BindDelegates.Bind(Property));
            Assert.AreEqual(E2.ParamName, "Target");
            Assert.True(E2.Message.Contains(Strings.PropertyCanNotBindToItself));
        }

        [Test]
        public void BindDelegate_Binds()
        {
            var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindDelegates);
            var TargetProperty = new BindableProperty<int>(this);

            Assert.Null(Property.BoundProperty);
            BindDelegates.Bind(TargetProperty);
            Assert.NotNull(Property.BoundProperty);
        }

        [Test]
        public void BindDelegate_RaisesChangedEventWithCorrectParameters()
        {
            const int ValueOfTarget = 3;
            var PropertyValueReceivedOnce = false;

            var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindDelegates);
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

            BindDelegates.Bind(TargetProperty);
            Assert.Fail();
        }

        [Test]
        public void BindDelegate_DoesNotRaiseChangedEventWithSameValue()
        {
            var PropertyValueReceivedOnce = false;

            var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindDelegates);
            var TargetProperty = new BindableProperty<int>(this);

            Property.Changed += (Sender, Args) =>
            {
                if (PropertyValueReceivedOnce) Assert.Fail();
                else PropertyValueReceivedOnce = true;
            };

            BindDelegates.Bind(TargetProperty);
        }

        [Test]
        public void UnbindDelegate_Unbinds()
        {
            var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindDelegates);
            var TargetProperty = new BindableProperty<int>(this);

            BindDelegates.Bind(TargetProperty);
            BindDelegates.Unbind();
            Assert.Null(Property.BoundProperty);
        }

        [Test]
        public void UnbindDelegate_ThrowsExceptionWhileUnbound()
        {
            new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindDelegates);

            var E = Assert.Throws<InvalidOperationException>(() => BindDelegates.Unbind());
            Assert.AreEqual(Strings.PropertyNotBoundYet, E.Message);
        }

        [Test]
        public void UnbindDelegate_DoesNotRaiseChangedEvent()
        {
            const int ValueOfTarget = 3;
            var PropertyValueReceivedOnce = false;

            var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindDelegates);
            var TargetProperty = new BindableProperty<int>(this, ValueOfTarget);

            BindDelegates.Bind(TargetProperty);

            Property.Changed += (Sender, Args) =>
            {
                if (PropertyValueReceivedOnce) Assert.Fail();
                else PropertyValueReceivedOnce = true;
            };

            BindDelegates.Unbind();
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

            var PropertyA = new PrivatelySettablePrivatelyBindableProperty<int>(this, out var SetDelegatesA, out var BindDelegatesA);
            var PropertyB = new PrivatelySettablePrivatelyBindableProperty<int>(this, out var SetDelegatesB, out _);

            PropertyA.Changed += Property_ChangedA;
            PropertyB.Changed += Property_ChangedB;

            BindDelegatesA.Bind(PropertyB, true);

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
        public void StartMonitoringDelegate_StartsMonitoring()
        {
            var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindDelegates);

            Assert.False(Property.IsMonitoringWithoutBinding);
            BindDelegates.StartMonitoring();
            Assert.True(Property.IsMonitoringWithoutBinding);
        }

        [Test]
        public void StartMonitoringDelegate_ThrowsExceptionWhileMonitoringWithoutBinding()
        {
            new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindDelegates);

            BindDelegates.StartMonitoring();

            var E = Assert.Throws<InvalidOperationException>(() => BindDelegates.StartMonitoring());
            Assert.AreEqual(Strings.MonitoringAlreadyStarted, E.Message);
        }

        [Test]
        public void StopMonitoringDelegate_StopsMonitoring()
        {
            var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindDelegates);

            BindDelegates.StartMonitoring();
            BindDelegates.StopMonitoring();
            Assert.False(Property.IsMonitoringWithoutBinding);
        }

        [Test]
        public void StopMonitoringDelegate_ThrowsExceptionWhileNotMonitoringWithoutBinding()
        {
            new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindDelegates);

            var E = Assert.Throws<InvalidOperationException>(() => BindDelegates.StopMonitoring());
            Assert.AreEqual(Strings.MonitoringNotStartedYet, E.Message);
        }

        [Test]
        public void MonitorDelegate_ParameterValidation()
        {
            var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindDelegates);

            var E = Assert.Throws<ArgumentNullException>(() => BindDelegates.Monitor(null));
            Assert.AreEqual(E.ParamName, "Target");

            var E2 = Assert.Throws<ArgumentException>(() => BindDelegates.Monitor(Property));
            Assert.AreEqual(E2.ParamName, "Target");
            Assert.True(E2.Message.Contains(Strings.PropertyCanNotMonitorItself));
        }

        [Test]
        public void MonitorDelegate_Monitors()
        {
            new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindDelegates);
            var MonitoredProperty = new BindableProperty<DateTime>(this);

            BindDelegates.Monitor(MonitoredProperty);
        }

        [Test]
        public void MonitorDelegate_ThrowsExceptionWhileBound()
        {
            new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindDelegates);
            var TargetProperty = new BindableProperty<int>(this);
            var MonitoredProperty = new BindableProperty<DateTime>(this);

            BindDelegates.Bind(TargetProperty);

            var E = Assert.Throws<InvalidOperationException>(() => BindDelegates.Monitor(MonitoredProperty));
            Assert.AreEqual(Strings.PropertyCanNotMonitorAfterMonitoringStarted, E.Message);
        }

        [Test]
        public void MonitorDelegate_ThrowsExceptionWhileMonitoringWithoutBinding()
        {
            new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindDelegates);
            var MonitoredProperty = new BindableProperty<DateTime>(this);

            BindDelegates.StartMonitoring();

            var E = Assert.Throws<InvalidOperationException>(() => BindDelegates.Monitor(MonitoredProperty));
            Assert.AreEqual(Strings.PropertyCanNotMonitorAfterMonitoringStarted, E.Message);
        }

        [Test]
        public void ChangedEventOfMonitoredProperty_Unbinds()
        {
            var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindDelegates);
            var TargetProperty = new BindableProperty<int>(this);
            var MonitoredProperty = new BindableProperty<DateTime>(this);

            BindDelegates.Monitor(MonitoredProperty);
            BindDelegates.Bind(TargetProperty);

            MonitoredProperty.SetValue(DateTime.Now);
            Assert.Null(Property.BoundProperty);
        }

        [Test]
        public void ChangedEventOfMonitoredProperty_StopsMonitoring()
        {
            var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindDelegates);
            var MonitoredProperty = new BindableProperty<DateTime>(this);

            BindDelegates.Monitor(MonitoredProperty);
            BindDelegates.StartMonitoring();

            MonitoredProperty.SetValue(DateTime.Now);
            Assert.False(Property.IsMonitoringWithoutBinding);
        }

        [Test]
        public void ChangedEventOfMonitoredProperty_CallsBinderCallbackWhileBound()
        {
            new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindDelegates, BinderCallback: Assert.Pass);
            var TargetProperty = new BindableProperty<int>(this);
            var MonitoredProperty = new BindableProperty<DateTime>(this);

            BindDelegates.Monitor(MonitoredProperty);
            BindDelegates.Bind(TargetProperty);

            MonitoredProperty.SetValue(DateTime.Now);
            Assert.Fail();
        }

        [Test]
        public void ChangedEventOfMonitoredProperty_CallsBinderCallbackWhileMonitoringWithoutBinding()
        {
            new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindDelegates, BinderCallback: Assert.Pass);
            var MonitoredProperty = new BindableProperty<DateTime>(this);

            BindDelegates.Monitor(MonitoredProperty);
            BindDelegates.StartMonitoring();

            MonitoredProperty.SetValue(DateTime.Now);
            Assert.Fail();
        }
    }
}