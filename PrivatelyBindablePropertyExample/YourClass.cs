using System.Windows;
using Atesh.BindableProperties;

namespace PrivatelyBindablePropertyExample
{
    // IMPORTANT: Please read the first example before this. More info is available in ReadMe.txt of this example project.
    class YourClass
    {
        // In this example, the height property is defined as a PrivatelyBindableProperty.
        // As the name of PrivatelyBindableProperty class implies, only its owner can bind it to another target. Its value can be set by any source publicly.
        public readonly PrivatelyBindableProperty<int> Height;

        // Delegates to control the bindable property.
        // Unlike the first example, we don't need SetDelegates here because PrivatelyBindableProperty has its own public Set/Clear methods.
        PrivatelyBindableProperty<int>.BindDelegates BindHeightDelegates;

        readonly string Name; // Just the name of the YourClass instance.

        public YourClass(string Name)
        {
            this.Name = Name;

            // Create the bindable property and get the delegates back.
            Height = new PrivatelyBindableProperty<int>(this, out BindHeightDelegates);

            // Subscribe to the Changed event of the bindable property.
            Height.Changed += Height_Changed;
        }

        void Height_Changed(PrivatelySettablePrivatelyBindableProperty<int> Sender, ChangedEventArgs<int> Args)
        {
            // For the sake of this example, we will just consume the new value to announce only instead of storing it.
            // IMPORTANT: Don't forget to check the empty value because the outside sources can call ClearValue at any time.
            if (Args.IsEmpty) MessageBox.Show($"{Name} says: I don't have a height info");
            else
            {
                var Text = $"{Name} says: My height is {Args.Value}.";
                MessageBox.Show(Text);
            }
        }

        // Since the height property is PrivatelyBindableProperty, it can't be bound from outside so we implement BindHeight and UnbindHeight commands for outside access.
        public void BindHeight(PrivatelyBindableProperty<int> Target) => BindHeightDelegates.Bind(Target);
        public void UnbindHeight() => BindHeightDelegates.Unbind();
    }
}