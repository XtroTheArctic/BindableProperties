using System.ComponentModel;
using System.Windows;
using Atesh.BindableProperties;

namespace BindablePropertyExample;

public partial class MainWindow : INotifyPropertyChanged
{
    public int AppleHeightTextBoxValue
    {
        get => _AppleHeightTextBoxValue;
        set
        {
            _AppleHeightTextBoxValue = value;

            OnPropertyChanged(nameof(AppleHeightTextBoxValue));
        }
    }

    public int TomatoHeightTextBoxValue { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    readonly YourClass Apple;
    readonly YourClass Tomato;

    int _AppleHeightTextBoxValue;

    public MainWindow()
    {
        InitializeComponent();

        Tomato = new(nameof(Tomato));
        Apple = new(nameof(Apple));

        Apple.Height.Changed += AppleHeight_Changed;
        Tomato.Height.Changed += TomatoHeight_Changed;

        ToggleBindingButtons(Apple.Height.BoundProperty != null);
    }

    void AppleHeight_Changed(PrivatelySettablePrivatelyBindableProperty<int> Sender, ChangedEventArgs<int> Args) => AppleHeightTextBoxValue = Args.Value;

    void TomatoHeight_Changed(PrivatelySettablePrivatelyBindableProperty<int> Sender, ChangedEventArgs<int> Args) => TomatoHeightTextBoxValue = Args.Value;

    void AppleSetHeightButton_Click(object Sender, RoutedEventArgs E)
    {
        Apple.Height.SetValue(AppleHeightTextBoxValue);

        ToggleBindingButtons(false);
    }

    void TomatoSetHeightButton_Click(object Sender, RoutedEventArgs E) => Tomato.Height.SetValue(TomatoHeightTextBoxValue);

    void BindButton_Click(object Sender, RoutedEventArgs E)
    {
        Apple.Height.Bind(Tomato.Height);

        ToggleBindingButtons(true);
    }

    void UnbindButton_Click(object Sender, RoutedEventArgs E)
    {
        Apple.Height.Unbind();

        ToggleBindingButtons(false);
    }

    void ToggleBindingButtons(bool IsBound)
    {
        BindButton.IsEnabled = !IsBound;
        UnbindButton.IsEnabled = IsBound;
    }

    protected virtual void OnPropertyChanged(string PropertyName) => PropertyChanged?.Invoke(this, new(PropertyName));
}