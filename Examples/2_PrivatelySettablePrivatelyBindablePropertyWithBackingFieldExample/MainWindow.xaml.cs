using System.Windows;
using Atesh.BindableProperties;

namespace PrivatelySettablePrivatelyBindablePropertyWithBackingFieldExample;

public partial class MainWindow
{
    readonly YourClass Apple;
    readonly YourClass Tomato;

    public MainWindow()
    {
        InitializeComponent();

        Tomato = new();
        Apple = new();

        Apple.Height.Changed += AppleHeight_Changed;
        Tomato.Height.Changed += TomatoHeight_Changed;

        ToggleBindingButtons(Apple.Height.BoundProperty is { });
    }

    void AppleHeight_Changed(PrivatelySettablePrivatelyBindableProperty<int> Sender, ChangedEventArgs<int> Args) => AppleHeightLabel.Content = Args.Value;

    void TomatoHeight_Changed(PrivatelySettablePrivatelyBindableProperty<int> Sender, ChangedEventArgs<int> Args) => TomatoHeightLabel.Content = Args.Value;

    void AppleGrowButton_Click(object Sender, RoutedEventArgs E)
    {
        Apple.GrowByOne();

        ToggleBindingButtons(false);
    }

    void AppleDieButton_Click(object Sender, RoutedEventArgs E)
    {
        Apple.Die();

        ToggleBindingButtons(false);
    }

    void TomatoGrowButton_Click(object Sender, RoutedEventArgs E) => Tomato.GrowByOne();

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