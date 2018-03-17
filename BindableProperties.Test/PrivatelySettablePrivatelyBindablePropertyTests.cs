using System;
using NUnit.Framework;

namespace Atesh.BindableProperties.Test
{
    [TestFixture]
    public class PrivatelySettablePrivatelyBindablePropertyTests
    {
        [Test]
        public void Constructor_ParameterValidation()
        {
            // ReSharper disable once ObjectCreationAsStatement
            var E = Assert.Throws<ArgumentNullException>(() => new PrivatelySettablePrivatelyBindableProperty<int>(null, out _, out _));
            Assert.True(E.ParamName == "Owner");
        }

        [Test]
        public void Constructor_ReturnsDelegates()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new PrivatelySettablePrivatelyBindableProperty<int>(this, out var SetDelegates, out var BindDelegates);

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
    }
}                                                                                                            