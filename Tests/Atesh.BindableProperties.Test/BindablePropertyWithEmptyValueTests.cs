// ReSharper disable ObjectCreationAsStatement

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Atesh.BindableProperties.Test;

[TestFixture]
public class BindablePropertyWithEmptyValueTests
{
    [Test]
    public void Constructors_ParameterValidation()
    {
        var E = Assert.Throws<ArgumentNullException>(() => new BindablePropertyWithEmptyValue<int>(null));
        ClassicAssert.AreEqual(E.ParamName, nameof(BindablePropertyWithEmptyValue<>.Owner));

        E = Assert.Throws<ArgumentNullException>(() => new BindablePropertyWithEmptyValue<int>(null, 0));
        ClassicAssert.AreEqual(E.ParamName, nameof(BindablePropertyWithEmptyValue<>.Owner));
    }

    [Test]
    public void Constructor_StoresCorrectValue()
    {
        const int Value = 1;
        var Property = new BindablePropertyWithEmptyValue<int>(this, Value);
        var PropertyValue = int.MinValue;
        var PropertyIsEmpty = false;
        Property.Changed += Property_Changed;

        void Property_Changed(PrivatelySettablePrivatelyBindableProperty<int> Sender, ChangedEventArgs<int> Args)
        {
            Property.Changed -= Property_Changed;
            PropertyValue = Args.Value;
            PropertyIsEmpty = Args.IsEmpty;
        }

        ClassicAssert.AreEqual(PropertyValue, Value);
        ClassicAssert.False(PropertyIsEmpty);
    }

    [Test]
    public void Constructor_StoresEmptyValue()
    {
        var Property = new BindablePropertyWithEmptyValue<int>(this, true);
        var PropertyIsEmpty = false;
        Property.Changed += Property_Changed;

        void Property_Changed(PrivatelySettablePrivatelyBindableProperty<int> Sender, ChangedEventArgs<int> Args)
        {
            Property.Changed -= Property_Changed;
            PropertyIsEmpty = Args.IsEmpty;
        }

        ClassicAssert.True(PropertyIsEmpty);
    }

    [Test]
    public void ClearValue_DoesNotRaiseChangedEventWhileEmpty()
    {
        var PropertyValueReceivedOnce = false;

        var Property = new BindablePropertyWithEmptyValue<int>(this, true);
        Property.Changed += delegate
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
                ClassicAssert.AreEqual(Sender, Property);
                ClassicAssert.True(Args.IsEmpty);
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
        var Property = new PrivatelyBindablePropertyWithEmptyValue<int>(this, out var BindMethods);
        var TargetProperty = new BindableProperty<int>(this);

        BindMethods.Bind(TargetProperty);
        Property.ClearValue();
        ClassicAssert.Null(Property.BoundProperty);
    }
}