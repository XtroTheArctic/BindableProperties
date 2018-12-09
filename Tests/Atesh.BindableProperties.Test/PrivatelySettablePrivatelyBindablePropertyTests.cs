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
            Assert.False(Property.IsBound);
        }

        [Test]
        public void ClearValueDelegate_DoesNotRaiseChangedEventWhenEmpty()
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
            Assert.False(Property.IsBound);
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

            Assert.False(Property.IsBound);
            BindDelegates.Bind(TargetProperty);
            Assert.True(Property.IsBound);
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
            Assert.False(Property.IsBound);
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
        public void MonitorDelegate_WhileBound()
        {
            new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindDelegates);
            var TargetProperty = new BindableProperty<int>(this);
            var MonitoredProperty = new BindableProperty<DateTime>(this);

            BindDelegates.Bind(TargetProperty);

            var E = Assert.Throws<InvalidOperationException>(() => BindDelegates.Monitor(MonitoredProperty));
            Assert.AreEqual(Strings.PropertyCanNotMonitorAfterBind, E.Message);
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
            Assert.False(Property.IsBound);
        }

        [Test]
        public void ChangedEventOfMonitoredProperty_CallsBinderCallback()
        {
            new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out var BindDelegates, BinderCallback: Assert.Pass);
            var TargetProperty = new BindableProperty<int>(this);
            var MonitoredProperty = new BindableProperty<DateTime>(this);

            BindDelegates.Monitor(MonitoredProperty);
            BindDelegates.Bind(TargetProperty);

            MonitoredProperty.SetValue(DateTime.Now);
            Assert.Fail();
        }
    }
}