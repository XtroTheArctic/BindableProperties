namespace Atesh.BindableProperties
{
    public class Property<T> : ReadOnlyProperty<T>
    {
        static SetValueDelegate TempDelegate;

        public readonly SetValueDelegate SetValue;

        public Property(object Owner) : base(Owner, out TempDelegate) => SetValue = TempDelegate;
    }
}