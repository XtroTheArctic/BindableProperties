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
            var Property = new Property<int>(this);
            Property.Changed += (Sender, Value) => Assert.Fail();

            Property.SetValue(default(int));
        }

        [Test]
        public void SetValue_RaisesChangedEventWithCorrectParameters()
        {
            const int NewValue = 1;

            var Property = new Property<int>(this);
            Property.Changed += (Sender, Value) =>
            {
                Assert.AreEqual(Sender, this);
                Assert.AreEqual(Value, NewValue);
                Assert.Pass();
            };

            Property.SetValue(NewValue);
            Assert.Fail();
        }
    }
}
