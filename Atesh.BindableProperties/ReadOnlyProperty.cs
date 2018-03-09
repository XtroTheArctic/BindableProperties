using System.Collections.Generic;

namespace Atesh.BindableProperties
{
    public class ReadOnlyProperty<T>
    {
        public event ChangedEventHandler<T> Changed;

        readonly object Owner;
        T Value;

        public ReadOnlyProperty(object Owner, out SetValueDelegate SetValueDelegate)
        {
            this.Owner = Owner ?? throw new System.ArgumentNullException(nameof(Owner));
            SetValueDelegate = SetValue;
        }

        protected void SetValue(T Value)
        {
            // We use this instead of object.Equals because this avoids boxing of value types including structs.
            if (EqualityComparer<T>.Default.Equals(this.Value, Value)) return;

            this.Value = Value;

            Changed?.Invoke(Owner, Value);
        }

        public delegate void SetValueDelegate(T Value);
    }
}