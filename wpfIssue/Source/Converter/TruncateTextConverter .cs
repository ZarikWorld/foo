using System;
using System.Globalization;
using System.Windows.Data;

namespace wpfIssues.Converter
{
    public class TruncateTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text && text.Length > 75)
            {
                return text.Substring(0, 75) + "...";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
