using System.Windows.Media;
using Atesh.BindableProperties;

namespace InheritedPropertySystemExampleViaMonitoringSystem
{
    class Skin
    {
        //todo: BackgroundColor property doesn't need to be a bindable property but we need it as a monitoring target until regular property targeting support gets implemented.
        public readonly PrivatelyBindablePropertyWithEmptyValue<Color> BackgroundColor;

        public bool BackgroundColorIsEmpty { get; private set; }

        public Skin()
        {
            BackgroundColor = new PrivatelyBindablePropertyWithEmptyValue<Color>(this, out _, true);

            BackgroundColor.Changed += BackgroundColor_Changed;
        }

        void BackgroundColor_Changed(PrivatelySettablePrivatelyBindableProperty<Color> Sender, ChangedEventArgs<Color> Args) => BackgroundColorIsEmpty = Args.IsEmpty;
    }
}