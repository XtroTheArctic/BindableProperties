namespace Atesh.BindableProperties
{
    public partial class PrivatelySettablePrivatelyBindableProperty<T>
    {
        public new event ChangedEventHandler<T> Changed
        {
            add
            {
                _Changed += value;

                value(this, new ChangedEventArgs<T>(IsEmpty, Value));
            }
            remove => _Changed -= value;
        }

        event ChangedEventHandler<T> _Changed;
    }
}