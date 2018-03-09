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
        public void SetValue_DoesntRaiseChangedEventWithSameValue()
        {
            var Property = new ReadOnlyProperty<int>(this, out var SetValue);
            Property.Changed += (Sender, Value) => Assert.Fail();

            SetValue(default(int));
        }

        [Test]
        public void SetValue_RaisesChangedEventWithCorrectParameters()
        {
            const int NewValue = 1;

            var Property = new ReadOnlyProperty<int>(this, out var SetValue);
            Property.Changed += (Sender, Value) =>
            {
                Assert.AreEqual(Sender, this);
                Assert.AreEqual(Value, NewValue);
                Assert.Pass();
            };

            SetValue(NewValue);
            Assert.Fail();
        }
    }
}
