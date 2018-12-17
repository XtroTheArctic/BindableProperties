using System;
using NUnit.Framework;

namespace Atesh.BindableProperties.Test
{
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
            Property.Changed += (Sender, Args) =>
            {
                Assert.AreEqual(Args.Value, Value);
                Assert.False(Args.IsEmpty);
                Assert.Pass();
            };

            Assert.Fail();
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

            Assert.False(Property.IsBound);
            Property.Bind(TargetProperty);
            Assert.True(Property.IsBound);
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
        public void Unbind_Unbinds()
        {
            var Property = new BindableProperty<int>(this);
            var TargetProperty = new BindableProperty<int>(this);

            Property.Bind(TargetProperty);
            Property.Unbind();
            Assert.False(Property.IsBound);
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

            Property.Changed += (Sender, Args) =>
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
}