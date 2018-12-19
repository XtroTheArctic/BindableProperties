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
        readonly YourClass ObjectC;

        readonly Skin Skin1 = new Skin();
        readonly Skin Skin2 = new Skin();

        public MainWindow()
        {
            InitializeComponent();

            ObjectA = new YourClass(RectangleA);
            ObjectB = new YourClass(RectangleB);
            ObjectC = new YourClass(RectangleC);

            ObjectA.Skin.Changed += Skin_Changed;
            ObjectA.BackgroundColor.Changed += BackgroundColor_Changed;

            ObjectB.Parent.Changed += Parent_Changed;
            ObjectB.Skin.Changed += Skin_Changed;
            ObjectB.BackgroundColor.Changed += BackgroundColor_Changed;

            ObjectC.Parent.Changed += Parent_Changed;
            ObjectC.Skin.Changed += Skin_Changed;
            ObjectC.BackgroundColor.Changed += BackgroundColor_Changed;

            Skin1.BackgroundColor.Changed += BackgroundColor_Changed;
            Skin2.BackgroundColor.Changed += BackgroundColor_Changed;
        }

        void Parent_Changed(PrivatelySettablePrivatelyBindableProperty<YourClass> Sender, ChangedEventArgs<YourClass> Args)
        {
            Label ParentLabel = null;

            if (Sender == ObjectB.Parent) ParentLabel = ObjectB_ParentLabel;
            else if (Sender == ObjectC.Parent) ParentLabel = ObjectC_ParentLabel;

            ParentLabel.Content = Args.Value == ObjectA ? "Object A" : Args.Value == ObjectB ? "Object B" : "Null";
        }

        void Skin_Changed(PrivatelySettablePrivatelyBindableProperty<Skin> Sender, ChangedEventArgs<Skin> Args)
        {
            Label SkinLabel = null;

            if (Sender == ObjectA.Skin) SkinLabel = ObjectA_SkinLabel;
            else if (Sender == ObjectB.Skin) SkinLabel = ObjectB_SkinLabel;
            else if (Sender == ObjectC.Skin) SkinLabel = ObjectC_SkinLabel;

            SkinLabel.Content = Args.Value == Skin1 ? nameof(Skin1) : Args.Value == Skin2 ? nameof(Skin2) : "Null";
        }

        void BackgroundColor_Changed(PrivatelySettablePrivatelyBindableProperty<Color> Sender, ChangedEventArgs<Color> Args)
        {
            Label EmptyLabel = null;
            Rectangle BackgroundColorRectangle = null;

            if (Sender == ObjectA.BackgroundColor)
            {
                EmptyLabel = ObjectA_BackgroundColorEmptyLabel;
                BackgroundColorRectangle = ObjectA_BackgroundColorRectangle;
            }
            else if (Sender == ObjectB.BackgroundColor)
            {
                EmptyLabel = ObjectB_BackgroundColorEmptyLabel;
                BackgroundColorRectangle = ObjectB_BackgroundColorRectangle;
            }
            else if (Sender == ObjectC.BackgroundColor)
            {
                EmptyLabel = ObjectC_BackgroundColorEmptyLabel;
                BackgroundColorRectangle = ObjectC_BackgroundColorRectangle;
            }
            else if (Sender == Skin1.BackgroundColor)
            {
                EmptyLabel = Skin1BackgroundColorEmptyLabel;
                BackgroundColorRectangle = Skin1BackgroundColorRectangle;
            }
            else if (Sender == Skin2.BackgroundColor)
            {
                EmptyLabel = Skin2BackgroundColorEmptyLabel;
                BackgroundColorRectangle = Skin2BackgroundColorRectangle;
            }

            if (Sender.BoundProperty != null || Args.IsEmpty)
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

        void ObjectA_Skin1Button_Click(object Sender, RoutedEventArgs E) => ObjectA.Skin.SetValue(Skin1);
        void ObjectA_Skin2Button_Click(object Sender, RoutedEventArgs E) => ObjectA.Skin.SetValue(Skin2);
        void ObjectA_SkinNullButton_Click(object Sender, RoutedEventArgs E) => ObjectA.Skin.SetValue(null);
        void ObjectA_BackgroundColorOrangeButton_Click(object Sender, RoutedEventArgs E) => ObjectA.BackgroundColor.SetValue(Colors.Orange);
        void ObjectA_BackgroundColorPurpleButton_Click(object Sender, RoutedEventArgs E) => ObjectA.BackgroundColor.SetValue(Colors.Purple);
        void ObjectA_BackgroundColorEmptyButton_Click(object Sender, RoutedEventArgs E) => ObjectA.BackgroundColor.ClearValue();

        void ObjectB_ParentA_Button_Click(object Sender, RoutedEventArgs E) => ObjectB.Parent.SetValue(ObjectA);
        void ObjectB_ParentNullButton_Click(object Sender, RoutedEventArgs E) => ObjectB.Parent.SetValue(null);
        void ObjectB_Skin1Button_Click(object Sender, RoutedEventArgs E) => ObjectB.Skin.SetValue(Skin1);
        void ObjectB_Skin2Button_Click(object Sender, RoutedEventArgs E) => ObjectB.Skin.SetValue(Skin2);
        void ObjectB_SkinNullButton_Click(object Sender, RoutedEventArgs E) => ObjectB.Skin.SetValue(null);
        void ObjectB_BackgroundColorWhiteButton_Click(object Sender, RoutedEventArgs E) => ObjectB.BackgroundColor.SetValue(Colors.White);
        void ObjectB_BackgroundColorAquaButton_Click(object Sender, RoutedEventArgs E) => ObjectB.BackgroundColor.SetValue(Colors.Aqua);
        void ObjectB_BackgroundColorEmptyButton_Click(object Sender, RoutedEventArgs E) => ObjectB.BackgroundColor.ClearValue();

        void ObjectC_ParentA_Button_Click(object Sender, RoutedEventArgs E) => ObjectC.Parent.SetValue(ObjectA);
        void ObjectC_ParentB_Button_Click(object Sender, RoutedEventArgs E) => ObjectC.Parent.SetValue(ObjectB);
        void ObjectC_ParentNullButton_Click(object Sender, RoutedEventArgs E) => ObjectC.Parent.SetValue(null);
        void ObjectC_Skin1Button_Click(object Sender, RoutedEventArgs E) => ObjectC.Skin.SetValue(Skin1);
        void ObjectC_Skin2Button_Click(object Sender, RoutedEventArgs E) => ObjectC.Skin.SetValue(Skin2);
        void ObjectC_SkinNullButton_Click(object Sender, RoutedEventArgs E) => ObjectC.Skin.SetValue(null);
        void ObjectC_BackgroundColorFuchsiaButton_Click(object Sender, RoutedEventArgs E) => ObjectC.BackgroundColor.SetValue(Colors.Fuchsia);
        void ObjectC_BackgroundColorMaroonButton_Click(object Sender, RoutedEventArgs E) => ObjectC.BackgroundColor.SetValue(Colors.Maroon);
        void ObjectC_BackgroundColorEmptyButton_Click(object Sender, RoutedEventArgs E) => ObjectC.BackgroundColor.ClearValue();

        void Skin1BackgroundColorBlueButton_Click(object Sender, RoutedEventArgs E) => Skin1.BackgroundColor.SetValue(Colors.Blue);
        void Skin1BackgroundColorRedButton_Click(object Sender, RoutedEventArgs E) => Skin1.BackgroundColor.SetValue(Colors.Red);
        void Skin1BackgroundColorEmptyButton_Click(object Sender, RoutedEventArgs E) => Skin1.BackgroundColor.ClearValue();

        void Skin2BackgroundColorGreenButton_Click(object Sender, RoutedEventArgs E) => Skin2.BackgroundColor.SetValue(Colors.Green);
        void Skin2BackgroundColorYellowButton_Click(object Sender, RoutedEventArgs E) => Skin2.BackgroundColor.SetValue(Colors.Yellow);
        void Skin2BackgroundColorEmptyButton_Click(object Sender, RoutedEventArgs E) => Skin2.BackgroundColor.ClearValue();
    }
}
