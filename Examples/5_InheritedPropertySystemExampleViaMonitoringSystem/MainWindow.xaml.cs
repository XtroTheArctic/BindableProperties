using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Atesh.BindableProperties;

namespace InheritedPropertySystemExampleViaMonitoringSystem
{
    public partial class MainWindow
    {
        readonly YourClass ObjectA;
        readonly YourClass ObjectB;

        readonly Skin Skin1 = new Skin();
        readonly Skin Skin2 = new Skin();

        public MainWindow()
        {
            InitializeComponent();

            ObjectA = new YourClass(RectangleA);
            ObjectB = new YourClass(RectangleB);

            Skin1.BackgroundColor.Changed += BackgroundColor_Changed;
            Skin2.BackgroundColor.Changed += BackgroundColor_Changed;
        }

        void BackgroundColor_Changed(PrivatelySettablePrivatelyBindableProperty<Color> Sender, ChangedEventArgs<Color> Args)
        {
            Label EmptyLabel = null;
            Rectangle BackgroundColorRectangle = null;

            if (Sender == Skin1.BackgroundColor)
            {
                EmptyLabel = Skin1BackgroundColorEmptyLabel;
                BackgroundColorRectangle = Skin1BackgroundColorRectangle;
            }
            else if (Sender == Skin2.BackgroundColor)
            {
                EmptyLabel = Skin2BackgroundColorEmptyLabel;
                BackgroundColorRectangle = Skin2BackgroundColorRectangle;
            }

            if (Args.IsEmpty)
            {
                EmptyLabel.Visibility = Visibility.Visible;
                BackgroundColorRectangle.Visibility = Visibility.Hidden;
            }
            else
            {
                EmptyLabel.Visibility = Visibility.Hidden;
                BackgroundColorRectangle.Visibility = Visibility.Visible;
                BackgroundColorRectangle.Fill = new SolidColorBrush(Args.Value);
            }
        }

        void Skin1BackgroundColorBlueButton_Click(object Sender, RoutedEventArgs E) => Skin1.BackgroundColor.SetValue(Colors.Blue);
        void Skin1BackgroundColorRedButton_Click(object Sender, RoutedEventArgs E) => Skin1.BackgroundColor.SetValue(Colors.Red);
        void Skin1BackgroundColorEmptyButton_Click(object Sender, RoutedEventArgs E) => Skin1.BackgroundColor.ClearValue();
        void Skin2BackgroundColorGreenButton_Click(object Sender, RoutedEventArgs E) => Skin2.BackgroundColor.SetValue(Colors.Green);
        void Skin2BackgroundColorYellowButton_Click(object Sender, RoutedEventArgs E) => Skin2.BackgroundColor.SetValue(Colors.Yellow);
        void Skin2BackgroundColorEmptyButton_Click(object Sender, RoutedEventArgs E) => Skin2.BackgroundColor.ClearValue();
    }
}
