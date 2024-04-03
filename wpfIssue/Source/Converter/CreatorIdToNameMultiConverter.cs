using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using wpfIssues.Model;

namespace wpfIssues.Converter
{
    public class CreatorIdToNameMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int creator_id && values[1] is ObservableCollection<Mitarbeiter> mitarbeiters)    
            {
                var mitarbeiter = mitarbeiters.FirstOrDefault(m => m.id == creator_id);
                return mitarbeiter?.name ?? "Not Found";
            }
            return "Binding Error"; // or some default value
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class CreatorIdToShortNameMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int creator_id && values[1] is ObservableCollection<Mitarbeiter> mitarbeiters)
            {
                var mitarbeiter = mitarbeiters.FirstOrDefault(m => m.id == creator_id);
                string name = "Not Found";

                if (mitarbeiter != null)
                {
                    string v = mitarbeiter.name.Split(" ").First();
                    string n = mitarbeiter.name.Split(" ").Last();

                    name = v.Substring(0,3) + " " +  n.Substring(0, 1);
                }

                return name;
            }
            return ""; // or some default value
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
