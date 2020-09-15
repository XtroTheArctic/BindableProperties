using System.Windows.Media;
using InheritedPropertySystemExampleViaMonitoringSystem;

namespace Atesh.BindableProperties.Test
{
    public partial class InheritedPropertySystemExampleTests
    {
        static Color GetRectangleColor(YourClass Object) => ((SolidColorBrush)Object.Rectangle.Fill).Color;
    }
}