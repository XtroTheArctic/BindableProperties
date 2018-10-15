using System;
using NUnit.Framework;

namespace Atesh.BindableProperties.Test
{
    [TestFixture]
    public class PrivatelyBindablePropertyTests
    {
        [Test]
        public void Constructors_ParameterValidation()
        {
            var E = Assert.Throws<ArgumentNullException>(() => new PrivatelyBindableProperty<int>(null, out _));
            Assert.AreEqual(E.ParamName, nameof(PrivatelyBindableProperty<int>.Owner));

            E = Assert.Throws<ArgumentNullException>(() => new PrivatelyBindableProperty<int>(null, out _, 0));
            Assert.AreEqual(E.ParamName, nameof(PrivatelyBindableProperty<int>.Owner));
        }

        [Test]
        public void Constructor_StoresCorrectValue()
        {
            const int Value = 1;
            var Property = new PrivatelyBindableProperty<int>(this, out _, Value);
            Property.Changed += (Sender, Args) =>
            {
                Assert.AreEqual(Args.Value, Value);
                Assert.False(Args.IsEmpty);
                Assert.Pass();
            };

            Assert.Fail();
        }

        [Test]
        public void SetValue_DoesNotRaiseChangedEventWithSameValue()
        {
            var PropertyValueReceivedOnce = false;

            var Property = new PrivatelyBindableProperty<int>(this, out _);
            Property.Changed += (Sender, Args) =>
            {
                if (PropertyValueReceivedOnce) Assert.Fail();
                else PropertyValueReceivedOnce = true;
            };

            Property.SetValue(default(int));
        }

        [Test]
        public void SetValue_RaisesChangedEventWithCorrectParameters()
        {
            const int NewValue = 1;
            var PropertyValueReceivedOnce = false;

            var Property = new PrivatelyBindableProperty<int>(this, out _);
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

            Property.SetValue(NewValue);
            Assert.Fail();
        }

        [Test]
        public void SetValue_Unbinds()
        {
            var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out var SetDelegates, out var BindDelegates);
            var TargetProperty = new PrivatelySettablePrivatelyBindableProperty<int>(this, out _, out _);

            BindDelegates.Bind(TargetProperty);
            SetDelegates.SetValue(0);
            Assert.False(Property.IsBound);
        }
    }
}