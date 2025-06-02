using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

public class DueDateToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime dueDate)
        {
            var now = DateTime.Now.Date;
            if (dueDate < now)
                return System.Windows.Media.Brushes.Red;
            if ((dueDate - now).TotalDays <= 7)
                return System.Windows.Media.Brushes.Orange;
        }
        return System.Windows.Media.Brushes.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
}