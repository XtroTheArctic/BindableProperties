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
            var E = Assert.Throws<ArgumentNullException>(() => new ReadOnlyProperty<int>(null, out var _));
            Assert.True(E.ParamName == "Owner");
        }

        [Test]
        public void Constructor_ReturnsSetValueDelegate()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new ReadOnlyProperty<int>(this, out var SetValue);

            if (SetValue == null) Assert.Fail();
        }

        [Test]
        public void SetValueDelegate_DoesntRaiseChangedEventWithSameValue()
        {
            int? PropertyValue = null;

            var Property = new ReadOnlyProperty<int>(this, out var SetValue);
            Property.Changed += (Sender, Value) =>
            {
                if (PropertyValue.HasValue) Assert.Fail();
                else PropertyValue = Value;
            };

            SetValue(default(int));
        }

        [Test]
        public void ChangedAdder_RaisesChangedEventImmediatelyWithCorrectParameters()
        {
            var Property = new ReadOnlyProperty<int>(this, out var _);
            Property.Changed += (Sender, Value) =>
            {
                Assert.AreEqual(Sender, this);
                Assert.AreEqual(Value, default(int));
                Assert.Pass();
            };

            Assert.Fail();
        }

        [Test]
        public void SetValueDelegate_RaisesChangedEventWithCorrectParameters()
        {
            const int NewValue = 1;
            int? PropertyValue = null;

            var Property = new ReadOnlyProperty<int>(this, out var SetValue);
            Property.Changed += (Sender, Value) =>
            {
                if (PropertyValue.HasValue)
                {
                    Assert.AreEqual(Sender, this);
                    Assert.AreEqual(Value, NewValue);
                    Assert.Pass();
                }
                else PropertyValue = Value;
            };

            SetValue(NewValue);
            Assert.Fail();
        }
    }
}
