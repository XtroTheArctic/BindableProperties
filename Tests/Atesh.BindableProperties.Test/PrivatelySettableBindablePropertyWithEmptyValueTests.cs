// ReSharper disable ObjectCreationAsStatement

using NUnit.Framework;

namespace Atesh.BindableProperties.Test;

[TestFixture]
public class PrivatelySettableBindablePropertyWithEmptyValueTests
{
    [Test]
    public void Constructors_ParameterValidation()
    {
        var E = Assert.Throws<ArgumentNullException>(() => new PrivatelySettableBindablePropertyWithEmptyValue<int>(null, out _));
        Assert.AreEqual(E.ParamName, nameof(PrivatelySettableBindablePropertyWithEmptyValue<int>.Owner));

        E = Assert.Throws<ArgumentNullException>(() => new PrivatelySettableBindablePropertyWithEmptyValue<int>(null, out _, 0));
        Assert.AreEqual(E.ParamName, nameof(PrivatelySettableBindablePropertyWithEmptyValue<int>.Owner));
    }

    [Test]
    public void Constructor_StoresCorrectValue()
    {
        const int Value = 1;
        var Property = new PrivatelySettableBindablePropertyWithEmptyValue<int>(this, out _, Value);
        var PropertyValue = int.MinValue;
        var PropertyIsEmpty = false;
        Property.Changed += Property_Changed;

        void Property_Changed(IBindableProperty<int> Sender, ChangedEventArgs<int> Args)
        {
            Property.Changed -= Property_Changed;
            PropertyValue = Args.Value;
            PropertyIsEmpty = Args.IsEmpty;
        }

        Assert.AreEqual(PropertyValue, Value);
        Assert.False(PropertyIsEmpty);
    }

    [Test]
    public void Constructor_StoresEmptyValue()
    {
        var Property = new PrivatelySettableBindablePropertyWithEmptyValue<int>(this, out _, true);
        var PropertyIsEmpty = false;
        Property.Changed += Property_Changed;

        void Property_Changed(IBindableProperty<int> Sender, ChangedEventArgs<int> Args)
        {
            Property.Changed -= Property_Changed;
            PropertyIsEmpty = Args.IsEmpty;
        }

        Assert.True(PropertyIsEmpty);
    }

    [Test]
    public void ClearValue_DoesNotRaiseChangedEventWhileEmpty()
    {
        var PropertyValueReceivedOnce = false;

        var Property = new PrivatelySettableBindablePropertyWithEmptyValue<int>(this, out var SetDelegates, true);
        Property.Changed += delegate
        {
            if (PropertyValueReceivedOnce) Assert.Fail();
            else PropertyValueReceivedOnce = true;
        };

        SetDelegates.ClearValue();
    }

    [Test]
    public void ClearValue_RaisesChangedEventWithCorrectParameters()
    {
        var PropertyValueReceivedOnce = false;

        var Property = new PrivatelySettableBindablePropertyWithEmptyValue<int>(this, out var SetDelegates);
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

        SetDelegates.ClearValue();
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