using System;
using NUnit.Framework;

namespace Atesh.BindableProperties.Test
{
    [TestFixture]
    public class BindablePropertyWithEmptyValueTests
    {
        [Test]
        public void Constructors_ParameterValidation()
        {
            var E = Assert.Throws<ArgumentNullException>(() => new BindablePropertyWithEmptyValue<int>(null));
            Assert.AreEqual(E.ParamName, nameof(BindablePropertyWithEmptyValue<int>.Owner));

            E = Assert.Throws<ArgumentNullException>(() => new BindablePropertyWithEmptyValue<int>(null, 0));
            Assert.AreEqual(E.ParamName, nameof(BindablePropertyWithEmptyValue<int>.Owner));
        }

        [Test]
        public void Constructor_StoresCorrectValue()
        {
            const int Value = 1;
            var Property = new BindablePropertyWithEmptyValue<int>(this, Value);
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
            var Property = new BindablePropertyWithEmptyValue<int>(this, true);
            Property.Changed += (Sender, Args) =>
            {
                Assert.True(Args.IsEmpty);
                Assert.Pass();
            };

            Assert.Fail();
        }

        [Test]
        public void ClearValue_DoesNotRaiseChangedEventWhenEmpty()
        {
            var PropertyValueReceivedOnce = false;

            var Property = new BindablePropertyWithEmptyValue<int>(this, true);
            Property.Changed += (Sender, Args) =>
            {
                if (PropertyValueReceivedOnce) Assert.Fail();
                else PropertyValueReceivedOnce = true;
            };

            Property.ClearValue();
        }

        [Test]
        public void ClearValue_RaisesChangedEventWithCorrectParameters()
        {
            var PropertyValueReceivedOnce = false;

            var Property = new BindablePropertyWithEmptyValue<int>(this);
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

            Property.ClearValue();
            Assert.Fail();
        }

        [Test]
        public void ClearValue_Unbinds()
        {
            var Property = new PrivatelyBindablePropertyWithEmptyValue<int>(this, out var BindDelegates);
            var TargetProperty = new BindableProperty<int>(this);

            BindDelegates.Bind(TargetProperty);
            Property.ClearValue();
            Assert.False(Property.IsBound);
        }
    }
}