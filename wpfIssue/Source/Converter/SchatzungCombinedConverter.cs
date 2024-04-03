using System;
using System.Globalization;
using System.Windows.Data;
namespace wpfIssues.Converter
{
    public class SchatzungCombinedConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string schaetzungString = "";
            if (values.Length == 2)
            {
                if (values[0] is int && (int)values[0] != 0)
                {
                    schaetzungString += $"{(int)values[0]}";
                }

                if (values[1] is int && (int)values[1] != 0)
                {
                    schaetzungString += $"/{(int)values[1]}";
                }

            }
            return schaetzungString;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Converting back is not supported.");
        }
    }
}