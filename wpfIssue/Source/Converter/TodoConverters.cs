using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using wpfIssues.Model;

namespace wpfIssues.Converter
{
    public class TodoRowStyleMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var brush = new SolidColorBrush(Colors.Transparent);

            var ende = values[1] as DateTime?;
            var deadline = values[2] as DateTime?;

            enumStatus? status;
            if (values.Length > 3)
            {
                status = values[3] as enumStatus?;
            }
            else
            {
                status = enumStatus.opened;
            }

            if (status == enumStatus.imTest) brush = new SolidColorBrush(Colors.Green);
            else if (status == enumStatus.klaerungsbedarf) brush = new SolidColorBrush(Colors.Gray);
            else if (status == enumStatus.inBearbeitung) brush = new SolidColorBrush(Colors.LightBlue);

            brush.Opacity = 0.5;
            if (parameter is not null && parameter as string == "PrioColumn") brush.Opacity = 1;



            if (ende == null)
                ende = DateTime.Today;

            if (deadline != DateTime.MinValue/* && ende != null*/ && deadline != null)
            {
                bool red = deadline < ende;
                bool orange = ende.Value >= deadline.Value.AddDays(-2);

                if (red)
                {
                    if ((status != enumStatus.opened))
                    {
                        if (parameter is not null && parameter as string == "Deadline")
                        {
                            brush.Opacity = 1;
                            brush = new SolidColorBrush(Colors.Red);
                            brush.Opacity = 0.5;
                        }
                    }
                    else
                    {
                        brush.Opacity = 1;
                        brush = new SolidColorBrush(Colors.Red);
                        brush.Opacity = 0.5;
                    }

                    if (parameter is not null && parameter as string == "PrioColumn")
                    {
                        brush.Opacity = 1;
                    }
                }
                else if (orange)
                {
                    if ((status != enumStatus.opened))
                    {
                        if (parameter is not null && parameter as string == "Deadline")
                        {
                            brush = new SolidColorBrush(Colors.Orange);
                            brush.Opacity = 0.5;
                        }
                    }
                    else
                    {
                        brush.Opacity = 1;
                        brush = new SolidColorBrush(Colors.Orange);
                        brush.Opacity = 0.5;
                    }

                    if (parameter is not null && parameter as string == "PrioColumn")
                    {
                        brush.Opacity = 1;
                    }
                }
            }
            return brush;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class PrefixConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string prefix = parameter?.ToString() ?? "";
            return $"{prefix}{value}";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
