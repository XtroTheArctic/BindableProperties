using Atesh.BindableProperties;
using NUnit.Framework;

namespace BindableProperties.Test
{
    [TestFixture]
    public class PropertyTests
    {
        [Test]
        public void SetValue_DoesntRaiseChangedEventWithSameValue()
        {
            int? PropertyValue = null;

            var Property = new Property<int>(this);
            Property.Changed += (Sender, Value) =>
            {
                if (PropertyValue.HasValue) Assert.Fail();
                else PropertyValue = Value;
            };

            Property.SetValue(default(int));
        }

        [Test]
        public void SetValue_RaisesChangedEventWithCorrectParameters()
        {
            const int NewValue = 1;
            int? PropertyValue = null;

            var Property = new Property<int>(this);
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

            Property.SetValue(NewValue);
            Assert.Fail();
        }
    }
}
