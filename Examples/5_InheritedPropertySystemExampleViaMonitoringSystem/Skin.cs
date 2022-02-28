using System.Windows.Media;
using Atesh.BindableProperties;

namespace InheritedPropertySystemExampleViaMonitoringSystem
{
    public class Skin
    {
        public bool BackgroundColorIsEmpty { get; private set; }

        public PrivatelyBindablePropertyWithEmptyValue<Color> BackgroundColor { get; }

        public Skin()
        {
            BackgroundColor = new PrivatelyBindablePropertyWithEmptyValue<Color>(this, out _, true);

            BackgroundColor.Changed += BackgroundColor_Changed;
        }

        void BackgroundColor_Changed(PrivatelySettablePrivatelyBindableProperty<Color> Sender, ChangedEventArgs<Color> Args) => BackgroundColorIsEmpty = Args.IsEmpty;
    }
}