using System.Collections.Generic;

namespace Atesh.BindableProperties
{
    public class ReadOnlyProperty<T> : IProperty<T>
    {
        public event ChangedEventHandler<T> Changed;

        readonly object Owner;
        T Value;

        public ReadOnlyProperty(object Owner, out SetValueDelegate SetValueDelegate)
        {
            this.Owner = Owner ?? throw new System.ArgumentNullException(nameof(Owner));
            SetValueDelegate = SetValue;
        }

        void SetValue(T Value)
        {
            if (IsSameValue(Value)) return;

            this.Value = Value;

            Changed?.Invoke(Owner, Value);
        }

        // We use EqualityComparer instead of object.Equals because it avoids boxing of value types including structs.
        protected bool IsSameValue(T Value) => EqualityComparer<T>.Default.Equals(this.Value, Value);

        public delegate void SetValueDelegate(T Value);
    }
}