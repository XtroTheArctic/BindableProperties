// ReSharper disable ObjectCreationAsStatement

using System;
using NUnit.Framework;

namespace Atesh.BindableProperties.Test
{
    [TestFixture]
    public class PrivatelyBindablePropertyWithEmptyValueTests
    {
        [Test]
        public void Constructors_ParameterValidation()
        {
            var E = Assert.Throws<ArgumentNullException>(() => new PrivatelyBindablePropertyWithEmptyValue<int>(null, out _));
            Assert.AreEqual(E.ParamName, nameof(PrivatelyBindablePropertyWithEmptyValue<int>.Owner));

            E = Assert.Throws<ArgumentNullException>(() => new PrivatelyBindablePropertyWithEmptyValue<int>(null, out _, 0));
            Assert.AreEqual(E.ParamName, nameof(PrivatelyBindablePropertyWithEmptyValue<int>.Owner));
        }

        [Test]
        public void Constructor_StoresCorrectValue()
        {
            const int Value = 1;
            var Property = new PrivatelyBindablePropertyWithEmptyValue<int>(this, out _, Value);
            Assert.AreEqual(Property.GetValueVeryExpensively(out var IsEmpty), Value);
            Assert.False(IsEmpty);
        }

        [Test]
        public void Constructor_StoresEmptyValue()
        {
            var Property = new PrivatelyBindablePropertyWithEmptyValue<int>(this, out _, true);
            Property.GetValueVeryExpensively(out var IsEmpty);
            Assert.True(IsEmpty);
        }

        [Test]
        public void ClearValue_DoesNotRaiseChangedEventWhileEmpty()
        {
            var PropertyValueReceivedOnce = false;

            var Property = new PrivatelyBindablePropertyWithEmptyValue<int>(this, out _, true);
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

            var Property = new PrivatelyBindablePropertyWithEmptyValue<int>(this, out _);
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
            Assert.Null(Property.BoundProperty);
        }
    }
}