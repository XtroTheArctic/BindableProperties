namespace Atesh.BindableProperties
{
    public class Property<T> : ReadOnlyProperty<T>
    {
        public Property(object Owner, out SetValueDelegate SetValueDelegate) : base(Owner, out SetValueDelegate) { }

        public new void SetValue(T Value)
        {
            if (IsSameValue(Value)) return;

            base.SetValue(Value);
        }
    }
}