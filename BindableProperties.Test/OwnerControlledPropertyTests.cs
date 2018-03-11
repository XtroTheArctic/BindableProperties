using System;
using NUnit.Framework;

namespace Atesh.BindableProperties.Test
{
    [TestFixture]
    public class OwnerControlledPropertyTests
    {
        [Test]
        public void Constructor_ParameterValidation()
        {
            // ReSharper disable once ObjectCreationAsStatement
            var E = Assert.Throws<ArgumentNullException>(() => new OwnerControlledProperty<int>(null, out _));
            Assert.True(E.ParamName == "Owner");
        }

        [Test]
        public void Constructor_ReturnsDelegates()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new OwnerControlledProperty<int>(this, out var Delegates);

            if (Delegates.SetValue == null) Assert.Fail();
            if (Delegates.ClearValue == null) Assert.Fail();
        }

        [Test]
        public void Constructor_StoresCorrectValue()
        {
            const int Value = 1;
            var Property = new OwnerControlledProperty<int>(this, out _, Value);
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
            var Property = new OwnerControlledProperty<int>(this, out _, true);
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

            var Property = new OwnerControlledProperty<int>(this, out var Delegates);
            Property.Changed += (Sender, Args) =>
            {
                if (PropertyValueReceivedOnce) Assert.Fail();
                else PropertyValueReceivedOnce = true;
            };

            Delegates.SetValue(default(int));
        }

        [Test]
        public void ClearValueDelegate_DoesNotRaiseChangedEventWhenEmpty()
        {
            var PropertyValueReceivedOnce = false;

            var Property = new OwnerControlledProperty<int>(this, out var Delegates, true);
            Property.Changed += (Sender, Args) =>
            {
                if (PropertyValueReceivedOnce) Assert.Fail();
                else PropertyValueReceivedOnce = true;
            };

            Delegates.ClearValue();
        }

        [Test]
        public void ChangedAdder_RaisesChangedEventImmediatelyWithCorrectParameters()
        {
            var Property = new OwnerControlledProperty<int>(this, out _);
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

            var Property = new OwnerControlledProperty<int>(this, out var Delegates);
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

            Delegates.SetValue(NewValue);
            Assert.Fail();
        }

        [Test]
        public void ClearValueDelegate_RaisesChangedEventWithCorrectParameters()
        {
            var PropertyValueReceivedOnce = false;

            var Property = new OwnerControlledProperty<int>(this, out var Delegates);
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

            Delegates.ClearValue();
            Assert.Fail();
        }
    }
}
