using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace wpfIssues.Converter
{
    public class DeadlineToBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var brush = Brushes.Transparent;
            DateTime? deadline = value as DateTime?;
            if (deadline != null && deadline < DateTime.Now)
            {
                brush = Brushes.LightSalmon;
            }
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
