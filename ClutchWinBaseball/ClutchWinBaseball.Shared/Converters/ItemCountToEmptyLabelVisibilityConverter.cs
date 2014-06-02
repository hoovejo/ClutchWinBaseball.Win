using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ClutchWinBaseball.Converters
{
    public class ItemCountToEmptyLabelVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int? itemCount = value as int?;
            if (itemCount.HasValue)
            {
                return itemCount > 0 ? Visibility.Collapsed : Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return true;
        }
    }
}
