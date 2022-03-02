using System.Windows;

namespace PrivatelySettablePrivatelyBindablePropertyExample;

public partial class MainWindow
{
    readonly YourClass Apple;
    readonly YourClass Tomato;

    public MainWindow()
    {
        InitializeComponent();

        Tomato = new(nameof(Tomato));
        Apple = new(nameof(Apple));

        ToggleBindingButtons(Apple.Height.BoundProperty != null);
    }

    void AppleGrowButton_Click(object Sender, RoutedEventArgs E)
    {
        Apple.GrowRandomly();

        ToggleBindingButtons(false);
    }

    void AppleDieButton_Click(object Sender, RoutedEventArgs E)
    {
        Apple.Die();

        ToggleBindingButtons(false);
    }

    void TomatoGrowButton_Click(object Sender, RoutedEventArgs E) => Tomato.GrowRandomly();

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