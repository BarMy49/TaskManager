using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfTaskManager.Converters
{
    public class BoolToStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isCompleted)
            {
                return isCompleted ? "Ukończone" : "Nieukończone";
            }
            return "Nieznany";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                if (status.Equals("Ukończone", StringComparison.OrdinalIgnoreCase))
                    return true;
                if (status.Equals("Nieukończone", StringComparison.OrdinalIgnoreCase))
                    return false;
            }
            return false;
        }
    }
}
