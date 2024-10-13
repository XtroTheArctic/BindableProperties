using Atesh.BindableProperties;

namespace PrivatelySettablePrivatelyBindablePropertyWithBackingFieldExample;

// IMPORTANT: Please read the first example before this.
class YourClass
{
    // Bindable property definition.
    public PrivatelySettablePrivatelyBindableProperty<int> Height { get; }

    // Purpose of this example is to define a backing field for the bindable property to be able to use it when necessary.
    // Storing the actual value of a bindable property in a backing field is completely OPTIONAL but in most cases you will need to do so.
    // You must define the backing field as private to make sure it's only used by your class.
    int Height_;

    // Method containers to control the bindable property.
    readonly PrivatelySettablePrivatelyBindableProperty<int>.SetDelegates Height_SetMethods;
    readonly PrivatelySettablePrivatelyBindableProperty<int>.BindDelegates Height_BindMethods;

    public YourClass()
    {
        // Create the bindable property and receive the methods via the containers.
        Height = new(this, out Height_SetMethods, out Height_BindMethods);

        // Subscribe to the Changed event of the bindable property.
        Height.Changed += Height_Changed;
    }

    // You don't need to check the empty value here unlike the first example because we don't use ClearValue method and we don't provide empty value support in this example.
    // Just store the new value into the backing field for later use.
    // IMPORTANT: Never, ever assign a value to the backing field outside of this Changed event handler in your class. It's value must come from the Changed event only.
    void Height_Changed(PrivatelySettablePrivatelyBindableProperty<int> Sender, ChangedEventArgs<int> Args) => Height_ = Args.Value;

    // Commands for outside access.
    public void GrowByOne() => Height_SetMethods.SetValue(Height_ + 1);
    public void Die() => Height_SetMethods.SetValue(0);
    public void BindHeight(PrivatelySettablePrivatelyBindableProperty<int> Target) => Height_BindMethods.Bind(Target);
    public void UnbindHeight() => Height_BindMethods.Unbind();
}