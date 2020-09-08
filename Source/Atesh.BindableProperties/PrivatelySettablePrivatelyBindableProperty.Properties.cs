namespace Atesh.BindableProperties
{
    public partial class PrivatelySettablePrivatelyBindableProperty<T>
    {
        public PrivatelySettablePrivatelyBindableProperty<T> BoundProperty { get; private set; }
        public bool IsMonitoringWithoutBinding { get; private set; }
    }
}