// ReSharper disable ObjectCreationAsStatement

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Atesh.BindableProperties.Test;

[TestFixture]
public class PrivatelySettableBindablePropertyWithEmptyValueTests
{
    [Test]
    public void Constructors_ParameterValidation()
    {
        var E = Assert.Throws<ArgumentNullException>(() => new PrivatelySettableBindablePropertyWithEmptyValue<int>(null, out _));
        ClassicAssert.AreEqual(E.ParamName, nameof(PrivatelySettableBindablePropertyWithEmptyValue<>.Owner));

        E = Assert.Throws<ArgumentNullException>(() => new PrivatelySettableBindablePropertyWithEmptyValue<int>(null, out _, 0));
        ClassicAssert.AreEqual(E.ParamName, nameof(PrivatelySettableBindablePropertyWithEmptyValue<>.Owner));
    }

    [Test]
    public void Constructor_StoresCorrectValue()
    {
        const int Value = 1;
        var Property = new PrivatelySettableBindablePropertyWithEmptyValue<int>(this, out _, Value);
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
        var Property = new PrivatelySettableBindablePropertyWithEmptyValue<int>(this, out _, true);
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

        var Property = new PrivatelySettableBindablePropertyWithEmptyValue<int>(this, out var SetMethods, true);
        Property.Changed += delegate
        {
            if (PropertyValueReceivedOnce) Assert.Fail();
            else PropertyValueReceivedOnce = true;
        };

        SetMethods.ClearValue();
    }

    [Test]
    public void ClearValue_RaisesChangedEventWithCorrectParameters()
    {
        var PropertyValueReceivedOnce = false;

        var Property = new PrivatelySettableBindablePropertyWithEmptyValue<int>(this, out var SetMethods);
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

        SetMethods.ClearValue();
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