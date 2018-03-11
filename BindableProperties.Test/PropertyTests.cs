using Atesh.BindableProperties;
using NUnit.Framework;

namespace BindableProperties.Test
{
    [TestFixture]
    public class PropertyTests
    {
        [Test]
        public void SetValue_DoesNotRaiseChangedEventWithSameValue()
        {
            var PropertyValueReceivedOnce = false;

            var Property = new Property<int>(this);
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

            var Property = new Property<int>(this);
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

            Property.SetValue(NewValue);
            Assert.Fail();
        }

        [Test]
        public void ClearValue_DoesNotRaiseChangedEventWhenEmpty()
        {
            var PropertyValueReceivedOnce = false;

            var Property = new Property<int>(this, true);
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

            var Property = new Property<int>(this);
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

            Property.ClearValue();
            Assert.Fail();
        }
    }
}
