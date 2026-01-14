// ReSharper disable ObjectCreationAsStatement

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Atesh.BindableProperties.Test;

[TestFixture]
public class PrivatelyBindablePropertyTests
{
    [Test]
    public void Constructors_ParameterValidation()
    {
        var E = Assert.Throws<ArgumentNullException>(() => new PrivatelyBindableProperty<int>(null, out _));
        ClassicAssert.AreEqual(E.ParamName, nameof(PrivatelyBindableProperty<>.Owner));

        E = Assert.Throws<ArgumentNullException>(() => new PrivatelyBindableProperty<int>(null, out _, 0));
        ClassicAssert.AreEqual(E.ParamName, nameof(PrivatelyBindableProperty<>.Owner));
    }

    [Test]
    public void Constructor_StoresCorrectValue()
    {
        const int Value = 1;
        var Property = new PrivatelyBindableProperty<int>(this, out _, Value);
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
    public void SetValue_DoesNotRaiseChangedEventWithSameValue()
    {
        var PropertyValueReceivedOnce = false;

        var Property = new PrivatelyBindableProperty<int>(this, out _);
        Property.Changed += delegate
        {
            if (PropertyValueReceivedOnce) Assert.Fail();
            else PropertyValueReceivedOnce = true;
        };

        Property.SetValue(default);
    }

    [Test]
    public void SetValue_RaisesChangedEventWithCorrectParameters()
    {
        const int NewValue = 1;
        var PropertyValueReceivedOnce = false;

        var Property = new PrivatelyBindableProperty<int>(this, out _);
        Property.Changed += (Sender, Args) =>
        {
            if (PropertyValueReceivedOnce)
            {
                ClassicAssert.AreEqual(Sender, Property);
                ClassicAssert.AreEqual(Args.Value, NewValue);
                ClassicAssert.False(Args.IsEmpty);
                Assert.Pass();
            }
            else PropertyValueReceivedOnce = true;
        };

        Property.SetValue(NewValue);
        Assert.Fail();
    }

    [Test]
    public void SetValue_Unbinds()
    {
        var Property = new PrivatelySettablePrivatelyBindableProperty<int>(this, out var SetMethods, out var BindMethods);
        var TargetProperty = new BindableProperty<int>(this);

        BindMethods.Bind(TargetProperty);
        SetMethods.SetValue(0);
        ClassicAssert.Null(Property.BoundProperty);
    }
}