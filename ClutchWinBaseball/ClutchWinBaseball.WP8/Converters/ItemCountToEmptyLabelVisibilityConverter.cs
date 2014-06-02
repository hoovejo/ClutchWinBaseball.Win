using System;
using System.Windows;
using System.Windows.Data;

namespace ClutchWinBaseball.WP8.Converters
{
    public class ItemCountToEmptyLabelVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int? itemCount = value as int?;
            if (itemCount.HasValue)
            {
                return itemCount > 0 ? Visibility.Collapsed : Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return true;
        }
    }
}
