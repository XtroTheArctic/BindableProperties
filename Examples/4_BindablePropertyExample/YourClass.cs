using System.Windows;
using Atesh.BindableProperties;

namespace BindablePropertyExample
{
    // IMPORTANT: Please read the first example before this.
    class YourClass
    {
        // This is the simplest example because BindableProperty class doesn't have any restriction on setting a value or binding to another target.
        public BindableProperty<int> Height { get; }

        readonly string Name; // Just the name of the YourClass instance.

        public YourClass(string Name)
        {
            this.Name = Name;

            // There is no need for any delegates.
            Height = new BindableProperty<int>(this);

            // Subscribe to the Changed event of the bindable property.
            Height.Changed += Height_Changed;
        }

        void Height_Changed(PrivatelySettablePrivatelyBindableProperty<int> Sender, ChangedEventArgs<int> Args)
        {
            // For the sake of this example, we will just consume the new value to announce only instead of storing it.
            var Text = $"{Name} says: My height is {Args.Value}.";
            MessageBox.Show(Text);
        }
    }
}