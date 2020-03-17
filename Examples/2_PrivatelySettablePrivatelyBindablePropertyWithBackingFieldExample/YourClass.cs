using Atesh.BindableProperties;

namespace PrivatelySettablePrivatelyBindablePropertyWithBackingFieldExample
{
    // IMPORTANT: Please read the first example before this.
    class YourClass
    {
        // Purpose of this example is to define a backing field for the bindable property to be able to use it when necessary.
        // Storing the actual value of a bindable property in a backing field is completely OPTIONAL but in most cases you will need to do so.
        // You must define the backing field as private to make sure it's only used by your class.
        int _Height;

        // Bindable property definition.
        public readonly PrivatelySettablePrivatelyBindableProperty<int> Height;

        // Delegates to control the bindable property.
        readonly PrivatelySettablePrivatelyBindableProperty<int>.SetDelegates SetHeightDelegates;
        readonly PrivatelySettablePrivatelyBindableProperty<int>.BindDelegates BindHeightDelegates;

        public YourClass()
        {
            // Create the bindable property and get the delegates back.
            Height = new PrivatelySettablePrivatelyBindableProperty<int>(this, out SetHeightDelegates, out BindHeightDelegates);

            // Subscribe to the Changed event of the bindable property.
            Height.Changed += Height_Changed;
        }

        // You don't need to check the empty value here unlike the first example because we don't use ClearValue method and we don't provide empty value support in this example.
        // Just store the new value into the backing field for later use.
        // IMPORTANT: Never, ever assign a value to the backing field outside of this Changed event handler in your class. It's value must come from the Changed event only.
        void Height_Changed(PrivatelySettablePrivatelyBindableProperty<int> Sender, ChangedEventArgs<int> Args) => _Height = Args.Value;

        // Commands for outside access.
        public void GrowByOne() => SetHeightDelegates.SetValue(_Height + 1);
        public void Die() => SetHeightDelegates.SetValue(0);
        public void BindHeight(PrivatelySettablePrivatelyBindableProperty<int> Target) => BindHeightDelegates.Bind(Target);
        public void UnbindHeight() => BindHeightDelegates.Unbind();
    }
}