using System.Reflection;

namespace Atesh.BindableProperties
{
    public class BindableProperty<T> : OwnerControlledBindableProperty<T>
    {
        #region Static
        // ReSharper disable PrivateFieldCanBeConvertedToLocalVariable

        static Delegates TempDelegates;

        // ReSharper restore PrivateFieldCanBeConvertedToLocalVariable
        #endregion

        new Delegates Delegates;

        public BindableProperty(object Owner, bool IsEmpty = false) : base(Owner, out _, out TempDelegates, IsEmpty) => Delegates = TempDelegates;
        public BindableProperty(object Owner, T Value) : base(Owner, out _, out TempDelegates, Value) => Delegates = TempDelegates;

        public void Bind(OwnerControlledProperty<T> Target) => Delegates.Bind(Target);
        public void Unbind(OwnerControlledProperty<T> Target) => Delegates.Unbind(Target);
        public void Bind(object Target, PropertyInfo Property, EventInfo Event) => Delegates.BindRegular(Target, Property, Event);
        public void Unbind(object Target, PropertyInfo Property, EventInfo Event) => Delegates.UnbindRegular(Target, Property, Event);
        public void UnbindAll() => Delegates.UnbindAll();
    }
}