using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;


namespace wpfIssues.Converter
{
    public class StyleRedRowConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            var brush = Brushes.Transparent;

            var deadline = values[1] as DateTime?;
            if (deadline != null && deadline < DateTime.Now)
            {
                brush = Brushes.LightSalmon;
            }
            return brush;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}