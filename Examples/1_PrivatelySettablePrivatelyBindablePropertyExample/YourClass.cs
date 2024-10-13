using System.Windows;
using Atesh.BindableProperties;

namespace PrivatelySettablePrivatelyBindablePropertyExample;

// This class represents your own class which you want to define a bindable property for demonstration purposes.
class YourClass
{
    // About bindable properties:
    //
    // A bindable property is a value binding target. Other objects or properties can bind to it in order to receive value changed notifications from it.
    // Also a bindable property can bind to another value target so it can reflect the same value as the target.
    // The value of a bindable property can be set directly but this operation breaks any existing binding since the property will have its own value.
    //
    // The important point here is that a bindable property is not directly readable. Even its owner can NOT read its value directly.
    // Only way to know its value is to subscribe to its Changed event.
    // By enforcing this rule, the bindable properties system makes sure that every object in the binding chain gets notified for a value change and none of them left behind with an outdated value. 

    // Usage:
    //
    // First, you must define a bindable property by:
    // 1) declaring a bindable property type of your choice and
    // 2) declaring the value type as generic parameter.
    // It's a good practice to mark it as readonly to prevent unexpected problems.
    // You can define it as non-private for other classes to access it as a bind target or you can keep it private if you want.
    //
    // In this example, the height property is defined as a PrivatelySettablePrivatelyBindableProperty.
    // As the name of PrivatelySettablePrivatelyBindableProperty class implies, only its owner can set its value or bind it to another target.
    // There are alternative bindable property types such as PrivatelyBindableProperty and BindableProperty if you want it to be settable or bindable by other classes.
    public PrivatelySettablePrivatelyBindableProperty<int> Height { get; }

    // Second, you must define the required method containers. You will receive some methods via these containers so, you can modify the state of the property by calling those methods. Please see the comments in class constructor for more info.
    // Keep these as private since you are defining a PrivatelySettablePrivatelyBindableProperty.
    readonly PrivatelySettablePrivatelyBindableProperty<int>.SetDelegates Height_SetMethods;
    readonly PrivatelySettablePrivatelyBindableProperty<int>.BindDelegates Height_BindMethods;

    readonly string Name; // We use the Name field to distinguish between multiple instances of this example class. It's not related to property system.

    public YourClass(string Name)
    {
        this.Name = Name;

        // Third, in your constructor, you must create an instance of bindable property and receive its methods via the method containers.
        // These methods are the only way to control the value of a bindable property. You will call them whenever you need.
        // You can provide an initial value or mark it as empty while creating. For this example, we keep its value as default by not providing those options.
        // Yes, bindable properties system supports empty values. Empty is a special value which is required when implementing inherited properties. It's different than null value.
        Height = new(this, out Height_SetMethods, out Height_BindMethods);

        // Fourth, you must subscribe to the Changed event of your property because even the owner of the property can't read its value directly.
        Height.Changed += Height_Changed;
    }

    void Height_Changed(PrivatelySettablePrivatelyBindableProperty<int> Sender, ChangedEventArgs<int> Args)
    {
        // You can make this same event handler to subscribe to multiple bindable properties with the same value type and you can use the Sender parameter to distinguish between those properties.
        // This is irrelevant for this example so we don't use the Sender parameter now.

        // In fifth step, you get the value of the property here, in this Changed event.
        // The purpose of this event handler is to use the new value of the property and do some work with it.
        // You can store the new value of the property to a private member as a cache but this is optional.
        // For the sake of this example, we will just consume the new value to announce only instead of storing it.
        // IMPORTANT: Don't forget to check the empty value if you plan to use ClearValue method in your class logic to provide empty value support for the bindable property.
        if (Args.IsEmpty) MessageBox.Show($"{Name} says: I don't have a height info");
        else
        {
            var Text = $"{Name} says: My height is {Args.Value}.";
            MessageBox.Show(Text);
        }
    }

    // Since the height property is PrivatelySettablePrivatelyBindableProperty, it can't be set or bound from outside so we implement GrowRandomly, Die, BindHeight and UnbindHeight commands for outside access.
    public void GrowRandomly() => Height_SetMethods.SetValue(new Random().Next(5, 15));
    public void BindHeight(PrivatelySettablePrivatelyBindableProperty<int> Target) => Height_BindMethods.Bind(Target);
    public void UnbindHeight() => Height_BindMethods.Unbind();

    // If you want your bindable property to support empty value, you can simple call ClearValue method according to your class logic.
    // But if you do so, please don't forget to check for empty value in the Changed event handler.
    public void Die() => Height_SetMethods.ClearValue();
}