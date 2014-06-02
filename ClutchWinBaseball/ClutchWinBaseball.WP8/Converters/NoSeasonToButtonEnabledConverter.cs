using System;
using System.Windows.Data;

namespace ClutchWinBaseball.WP8.Converters
{
    public class NoSeasonToButtonEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string stringValue = value as string;
            if (!string.IsNullOrEmpty(stringValue) && !stringValue.Contains("season"))
            {
                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return true;
        }
    }
}
