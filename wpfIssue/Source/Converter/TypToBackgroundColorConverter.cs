using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using wpfIssues.Model;

namespace wpfIssues.Converter
{
    public class TypToBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is enumTyp)
            {
                switch (value)
                {
                    case enumTyp.Story:
                        return System.Windows.Media.Brushes.LightBlue;
                    case enumTyp.Bug:
                        return System.Windows.Media.Brushes.Salmon;
                    case enumTyp.Performance:
                        return System.Windows.Media.Brushes.LightYellow;
                    case enumTyp.Luxus:
                        return System.Windows.Media.Brushes.LightGreen;
                    default:
                        return DependencyProperty.UnsetValue;
                }
            }
            return DependencyProperty.UnsetValue;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}