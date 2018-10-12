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

            Apple.Height.Changed += Height_Changed;
        }

        void Height_Changed(PrivatelySettablePrivatelyBindableProperty<int> Sender, ChangedEventArgs<int> Args)
        {
            var IsBound = Sender.IsBound;

            BindButton.IsEnabled = !IsBound;
            UnbindButton.IsEnabled = IsBound;
        }

        void AppleGrowButton_Click(object Sender, RoutedEventArgs E) => Apple.Grow();

        void AppleDieButton_Click(object Sender, RoutedEventArgs E) => Apple.Die();

        void TomatoGrowButton_Click(object Sender, RoutedEventArgs E) => Tomato.Grow();

        void TomatoDieButton_Click(object Sender, RoutedEventArgs E) => Tomato.Die();

        void BindButton_Click(object Sender, RoutedEventArgs E) => Apple.BindHeight(Tomato.Height);

        void UnbindButton_Click(object Sender, RoutedEventArgs E) => Apple.UnbindHeight();
    }
}
