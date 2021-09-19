using System.Threading;
using System.Windows.Media;
using NUnit.Framework;
using InheritedPropertySystemExampleViaMonitoringSystem;

namespace Atesh.BindableProperties.Test
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class InheritedPropertySystemExampleTests
    {
        MainWindow MainWindow;

        Color GetRectangleColor(YourClass Object) => ((SolidColorBrush)Object.Rectangle.Fill).Color;

        [SetUp]
        public void SetUp() => MainWindow = new MainWindow();

        [Test]
        public void Test1()
        {
            MainWindow.ObjectA_BackgroundColorOrangeButton_Click(null, null);
            Assert.AreEqual(Colors.Orange, GetRectangleColor(MainWindow.ObjectA));
        }

        [Test]
        public void Test2()
        {
            MainWindow.ObjectA_BackgroundColorOrangeButton_Click(null, null);
            MainWindow.ObjectB_ParentA_Button_Click(null, null);
            Assert.AreEqual(Colors.Orange, GetRectangleColor(MainWindow.ObjectB));
        }

        [Test]
        public void Test3()
        {
            MainWindow.Skin1BackgroundColorBlueButton_Click(null, null);
            MainWindow.ObjectA_BackgroundColorOrangeButton_Click(null, null);
            MainWindow.ObjectB_Skin1Button_Click(null, null);
            MainWindow.Skin1BackgroundColorEmptyButton_Click(null, null);
            MainWindow.ObjectB_ParentA_Button_Click(null, null);
            Assert.AreEqual(Colors.Orange, GetRectangleColor(MainWindow.ObjectB));
        }

        [Test]
        public void Test4()
        {
            MainWindow.ObjectA_BackgroundColorOrangeButton_Click(null, null);
            MainWindow.ObjectB_ParentA_Button_Click(null, null);
            MainWindow.ObjectC_ParentB_Button_Click(null, null);
            MainWindow.ObjectB_BackgroundColorAquaButton_Click(null, null);
            MainWindow.ObjectB_BackgroundColorEmptyButton_Click(null, null);
            Assert.AreEqual(Colors.Orange, GetRectangleColor(MainWindow.ObjectC));
        }
    }
}