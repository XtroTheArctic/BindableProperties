using System;

namespace Atesh.BindableProperties
{
    public class OwnerControlledBindableProperty<T> : OwnerControlledProperty<T>
    {
        OwnerControlledProperty<T> BoundProperty;

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
        }

        void Bind(OwnerControlledProperty<T> Target)
        {
#pragma warning disable IDE0016 // Use 'throw' expression
            if (Target == null) throw new ArgumentNullException(nameof(Target));
#pragma warning restore IDE0016 // Use 'throw' expression

            if (BoundProperty != null) Unbind();

            BoundProperty = Target;
            BoundProperty.Changed += BoundProperty_Changed;
        }

        void Unbind()
        {
            if (BoundProperty == null) return;

            BoundProperty.Changed -= BoundProperty_Changed;
            BoundProperty = null;
        }

        void BoundProperty_Changed(OwnerControlledProperty<T> Sender, ChangedEventArgs<T> Args)
        {
        }

        public delegate void BindToPropertyDelegate(OwnerControlledProperty<T> Target);

        public new struct Delegates
        {
            public BindToPropertyDelegate Bind;
            public Action Unbind;
        }
    }
}