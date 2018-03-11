using System;
using Atesh.BindableProperties;
using NUnit.Framework;

namespace BindableProperties.Test
{
    [TestFixture]
    public class ReadOnlyPropertyTests
    {
        [Test]
        public void Constructor_ParameterValidation()
        {
            // ReSharper disable once ObjectCreationAsStatement
            var E = Assert.Throws<ArgumentNullException>(() => new ReadOnlyProperty<int>(null, out _, out _));
            Assert.True(E.ParamName == "Owner");
        }

        [Test]
        public void Constructor_ReturnsDelegates()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new ReadOnlyProperty<int>(this, out var SetValue, out var ClearValue);

            if (SetValue == null) Assert.Fail();
            if (ClearValue == null) Assert.Fail();
        }

        [Test]
        public void Constructor_StoresCorrectValue()
        {
            const int Value = 1;
            var Property = new ReadOnlyProperty<int>(this, out var _, out var _, Value);
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
            var Property = new ReadOnlyProperty<int>(this, out var _, out var _, true);
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

            var Property = new ReadOnlyProperty<int>(this, out var SetValue, out _);
            Property.Changed += (Sender, Args) =>
            {
                if (PropertyValueReceivedOnce) Assert.Fail();
                else PropertyValueReceivedOnce = true;
            };

            SetValue(default(int));
        }

        [Test]
        public void ClearValueDelegate_DoesNotRaiseChangedEventWhenEmpty()
        {
            var PropertyValueReceivedOnce = false;

            var Property = new ReadOnlyProperty<int>(this, out var _, out var ClearValue, true);
            Property.Changed += (Sender, Args) =>
            {
                if (PropertyValueReceivedOnce) Assert.Fail();
                else PropertyValueReceivedOnce = true;
            };

            ClearValue();
        }

        [Test]
        public void ChangedAdder_RaisesChangedEventImmediatelyWithCorrectParameters()
        {
            var Property = new ReadOnlyProperty<int>(this, out _, out _);
            Property.Changed += (Sender, Args) =>
            {
                Assert.AreEqual(Sender, this);
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

            var Property = new ReadOnlyProperty<int>(this, out var SetValue, out var _);
            Property.Changed += (Sender, Args) =>
            {
                if (PropertyValueReceivedOnce)
                {
                    Assert.AreEqual(Sender, this);
                    Assert.AreEqual(Args.Value, NewValue);
                    Assert.False(Args.IsEmpty);
                    Assert.Pass();
                }
                else PropertyValueReceivedOnce = true;
            };

            SetValue(NewValue);
            Assert.Fail();
        }

        [Test]
        public void ClearValueDelegate_RaisesChangedEventWithCorrectParameters()
        {
            var PropertyValueReceivedOnce = false;

            var Property = new ReadOnlyProperty<int>(this, out var _, out var ClearValue);
            Property.Changed += (Sender, Args) =>
            {
                if (PropertyValueReceivedOnce)
                {
                    Assert.AreEqual(Sender, this);
                    Assert.True(Args.IsEmpty);
                    Assert.Pass();
                }
                else PropertyValueReceivedOnce = true;
            };

            ClearValue();
            Assert.Fail();
        }
    }
}
