namespace Atesh.BindableProperties
{
    public class ChangedEventArgs<T>
    {
        public readonly bool IsEmpty;
        public readonly T Value;

        public ChangedEventArgs(bool IsEmpty, T Value)
        {
            this.IsEmpty = IsEmpty;
            this.Value = Value;
        }
    }
}