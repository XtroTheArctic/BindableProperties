namespace Atesh.BindableProperties
{
    public class DependantProperty<T> : Property<T>
    {
        public DependantProperty(object Owner) : base(Owner) { }
    }
}