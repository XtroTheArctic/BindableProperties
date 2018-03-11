using System;
using System.Reflection;

namespace Atesh.BindableProperties
{
    public class OwnerControlledBindableProperty<T> : OwnerControlledProperty<T>
    {
        public OwnerControlledBindableProperty(object Owner, out OwnerControlledProperty<T>.Delegates Delegates, out Delegates BindingDelegates, bool IsEmpty = false) : base(Owner, out Delegates, IsEmpty)
        // ReSharper disable ArrangeConstructorOrDestructorBody
        // We can't convert this to expression body because of a Resharper bug which complains about out parameters not being assigned upon exit.
        {
            SetDelegates(out BindingDelegates);
        }
        // ReSharper restore ArrangeConstructorOrDestructorBody

        public OwnerControlledBindableProperty(object Owner, out OwnerControlledProperty<T>.Delegates Delegates, out Delegates BindingDelegates, T Value) : base(Owner, out Delegates, Value)
        // ReSharper disable ArrangeConstructorOrDestructorBody
        // We can't convert this to expression body because of a Resharper bug which complains about out parameters not being assigned upon exit.
        {
            SetDelegates(out BindingDelegates);
        }
        // ReSharper restore ArrangeConstructorOrDestructorBody

        void SetDelegates(out Delegates BindingDelegates)
        {
            BindingDelegates.Bind = Bind;
            BindingDelegates.Unbind = Unbind;
            BindingDelegates.BindRegular = Bind;
            BindingDelegates.UnbindRegular = Unbind;
            BindingDelegates.UnbindAll = UnbindAll;
        }

        void Bind(OwnerControlledProperty<T> Target)
        {
        }

        void Unbind(OwnerControlledProperty<T> Target)
        {
        }

        void Bind(object Target, PropertyInfo Property, EventInfo Event)
        {
        }

        void Unbind(object Target, PropertyInfo Property, EventInfo Event)
        {
        }

        void UnbindAll()
        {
        }

        public delegate void BindToPropertyDelegate(OwnerControlledProperty<T> Target);
        public delegate void BindToRegularProperty(object Target, PropertyInfo Property, EventInfo Event);

        public new struct Delegates
        {
            public BindToPropertyDelegate Bind;
            public BindToPropertyDelegate Unbind;
            public BindToRegularProperty BindRegular;
            public BindToRegularProperty UnbindRegular;
            public Action UnbindAll;
        }
    }
}