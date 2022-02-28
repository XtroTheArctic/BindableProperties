namespace Atesh.BindableProperties
{
    public class CoerceValueDelegateArgs<T>
    {
        public bool IsEmpty { get; set; }
        public T Value { get; set; }
    }
}