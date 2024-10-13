using System.Windows;
using Atesh.BindableProperties;

namespace PrivatelyBindablePropertyExample;

// IMPORTANT: Please read the first example before this.
class YourClass
{
    // In this example, the height property is defined as a PrivatelyBindableProperty.
    // As the name of PrivatelyBindableProperty class implies, only its owner can bind it to another target. Its value can be set by any source publicly.
    public PrivatelyBindableProperty<int> Height { get; }

    // Method containers to control the bindable property.
    // Unlike the first example, we don't need SetMethods container here because PrivatelyBindableProperty has its own public SetValue method.
    readonly PrivatelyBindableProperty<int>.BindMethods Height_BindMethods;

    readonly string Name; // Just the name of the YourClass instance.

    public YourClass(string Name)
    {
        this.Name = Name;

        // Create the bindable property and receive the methods via the containers.
        Height = new(this, out Height_BindMethods);

        // Subscribe to the Changed event of the bindable property.
        Height.Changed += Height_Changed;
    }

    void Height_Changed(PrivatelySettablePrivatelyBindableProperty<int> Sender, ChangedEventArgs<int> Args)
    {
        // For the sake of this example, we will just consume the new value to announce only instead of storing it.
        var Text = $"{Name} says: My height is {Args.Value}.";
        MessageBox.Show(Text);
    }

    // Since the height property is PrivatelyBindableProperty, it can't be bound from outside so, we implement BindHeight and UnbindHeight commands for outside access.
    public void BindHeight(PrivatelyBindableProperty<int> Target) => Height_BindMethods.Bind(Target);
    public void UnbindHeight() => Height_BindMethods.Unbind();
}