using System;
using System.Globalization;
using System.Windows.Data;
using TaskManager.Localization;

namespace WpfTaskManager.Converters
{
    public class BoolToStatusConverter : IValueConverter
    {
        public static ILocalizer Localizer { get; set; } = new ResourceLocalizer();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isCompleted)
            {
                return isCompleted ? Localizer.GetString("Completed") : Localizer.GetString("NotCompleted");
            }
            return "Nieznany";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                if (status.Equals(Localizer.GetString("Completed"), StringComparison.OrdinalIgnoreCase))
                    return true;
                if (status.Equals(Localizer.GetString("NotCompleted"), StringComparison.OrdinalIgnoreCase))
                    return false;
            }
            return false;
        }
    }
}
