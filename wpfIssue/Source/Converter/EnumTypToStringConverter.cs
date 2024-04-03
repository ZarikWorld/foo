using System;
using System.Globalization;
using System.Windows.Data;
using wpfIssues.Model;

namespace wpfIssues.Converter
{
    public class EnumTypToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is enumTyp enumValue)
            {
                if (Enum.IsDefined(typeof(enumTyp), enumValue))
                {
                    enumTyp enumType = (enumTyp)enumValue;
                    string ts =  CultureInfo.CurrentCulture.TextInfo.ToTitleCase(enumType.ToString().ToLower());

                    if (ts.Length > 5)
                    { 
                      ts = ts.Substring(0, 5);   
                    }


                    return ts;
                }
                else
                {
                    return "Invalid Enum Value";
                }
            }
            return "Binding Error"; // or some default value
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
