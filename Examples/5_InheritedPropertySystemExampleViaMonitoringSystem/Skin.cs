using System.Windows.Media;
using Atesh.BindableProperties;

namespace InheritedPropertySystemExampleViaMonitoringSystem
{
    public class Skin
    {
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