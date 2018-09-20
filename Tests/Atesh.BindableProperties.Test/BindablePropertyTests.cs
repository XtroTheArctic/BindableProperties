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
        public void Constructor_StoresEmptyValue()
        {
            var Property = new BindableProperty<int>(this, true);
            Property.Changed += (Sender, Args) =>
            {
                Assert.True(Args.IsEmpty);
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
            Assert.True(E2.Message.Contains(Strings.PropertyCanNotBeBindToItself));
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
    }
}