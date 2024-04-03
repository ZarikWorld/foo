using System;
using System.Globalization;
using System.Windows.Data;
using wpfIssues.Model;

namespace wpfIssues.Converter
{
    public class EnumTypToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is enumTyp enumValue)
            {
                return (int)enumValue;
            }
            return 0; // default or you could return DependencyProperty.UnsetValue;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                return (enumTyp)intValue;
            }
            return enumTyp.Story; // default value or any other appropriate default
        }
    }
}

