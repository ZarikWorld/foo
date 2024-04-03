using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using wpfIssues.Model;

namespace wpfIssues.Converter
{
    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                if(intValue == 0)
                    return string.Empty;
                else
                    return intValue.ToString();
            }
            return string.Empty;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                if (int.TryParse(stringValue, out int intValue))
                {
                    return intValue;
                }
            }
            return 0;
        }
    }
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((DateTime)value == new DateTime(01, 01, 0001))
            {
                return "";
            }
            else if (value is DateTime dateTime)
            {
                return dateTime.ToString("dd.MM.yyyy");
            }
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class UserRoleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var user = value as Mitarbeiter;
            if (user == null || user.role != enumRole.admin)
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MitarbeiterIdToNameMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int mitarbeiterId && values[1] is ObservableCollection<Mitarbeiter> mitarbeiters)
            {
                var mitarbeiter = mitarbeiters.FirstOrDefault(m => m.id == mitarbeiterId);
                return mitarbeiter?.name ?? "Not F";
            }
            return "";
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
