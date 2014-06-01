using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ClutchWinBaseball.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Boolean)
            {
                var test = (bool)value;
                return test ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return true;
        }
    }
}
