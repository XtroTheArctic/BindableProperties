using System.Windows;
using Atesh.BindableProperties;

namespace PrivatelySettablePrivatelyBindablePropertySample
{
    public partial class MainWindow
    {
        readonly YourClass Apple;
        readonly YourClass Tomato;

        public MainWindow()
        {
            InitializeComponent();

            Tomato = new YourClass("Tomato");
            Apple = new YourClass("Apple");

            Apple.Height.Changed += Apple_HeightChanged;

            ToggleBindingButtons(Apple.Height.IsBound);
        }

        void Apple_HeightChanged(PrivatelySettablePrivatelyBindableProperty<int> Sender, ChangedEventArgs<int> Args) => ToggleBindingButtons(Apple.Height.IsBound);

        void AppleGrowButton_Click(object Sender, RoutedEventArgs E) => Apple.Grow();

        void AppleDieButton_Click(object Sender, RoutedEventArgs E) => Apple.Die();

        void TomatoGrowButton_Click(object Sender, RoutedEventArgs E) => Tomato.Grow();

        void TomatoDieButton_Click(object Sender, RoutedEventArgs E) => Tomato.Die();

        void BindButton_Click(object Sender, RoutedEventArgs E)
        {
            Apple.BindHeight(Tomato.Height);

            ToggleBindingButtons(true);
        }

        void UnbindButton_Click(object Sender, RoutedEventArgs E)
        {
            Apple.UnbindHeight();

            ToggleBindingButtons(false);
        }

        void ToggleBindingButtons(bool IsBound)
        {
            BindButton.IsEnabled = !IsBound;
            UnbindButton.IsEnabled = IsBound;
        }
    }
}
