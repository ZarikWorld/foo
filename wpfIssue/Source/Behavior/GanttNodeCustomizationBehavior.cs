using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using wpfIssues.Model;

namespace wpfIssues.Behavior
{
    public class GanttNodeCustomizationBehavior : Behavior<Border>
    {
        public static string[] nodeColors = new string[]
        {
            "#FF2F0064", "#FFA500AA", "#FF00888D", "#FF638B11", "#FF853500",
            "#FFC6419D", "#FFB75D00", "#FF1261A7", "#FFC71400", "#FF216A21",
            "#FF234F62", "#FF6A3088", "#FF506500", "#FF313F6F", "#FF8A2212",
            "#FF9323A3", "#FF943F37", "#FF8E6D00", "#FF7E330D", "#FF702375",
            "#FF215A5B", "#FF224545", "#FF3E545F", "#FF2F5F57", "#FF3C8B22",
            "#FF447915", "#FF4A8B1E", "#FF1E4F8B", "#FF395675", "#FF8B3D3D",
            "#FF7A4315", "#FF8B3211", "#FF8B2448", "#FF621E75", "#FF4E1D87",
            "#FF601E8B", "#FF691C7A", "#FF5C1E3D", "#FF501E63", "#FF8B3E6E",
            "#FF5E7317", "#FF7F3D11", "#FF60208B", "#FF8B2355", "#FF8B1E37",
            "#FF391F5E", "#FF746C12", "#FF8B1E74", "#FF3C4D6E", "#FF4F8B55"
        };

        void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            var currentTask = this.AssociatedObject.DataContext as JxTask;
            if (currentTask != null && sender != null)
            {
                Border? node = sender as Border;
                SolidColorBrush brush;

                switch (currentTask.status)
                {
                    case enumStatus.opened:
                        brush = Brushes.Blue;
                        break;
                    case enumStatus.imTest:
                        brush = new SolidColorBrush(Colors.Green);
                        break;
                    case enumStatus.inBearbeitung:
                        brush = new SolidColorBrush(Colors.LightBlue);
                        break;
                    case enumStatus.klaerungsbedarf:
                        brush = new SolidColorBrush(Colors.Gray);
                        break;
                    default:
                        brush = new SolidColorBrush(Colors.Transparent);
                        break;
                }

                if (node != null)
                {
                    node.Background = brush;
                    node.BorderBrush = brush;
                }
            }
        }
        protected override void OnAttached()
        {
            this.AssociatedObject.Loaded += new RoutedEventHandler(AssociatedObject_Loaded);
        }
        protected override void OnDetaching()
        {
            this.AssociatedObject.Loaded -= new RoutedEventHandler(AssociatedObject_Loaded);
        }
        public static string getMitarbeiterColor(int mitarbeiterIndex)
        {
            //int index = mitarbeiterIndex % 10;
            return nodeColors[mitarbeiterIndex];
        }
    }
}
