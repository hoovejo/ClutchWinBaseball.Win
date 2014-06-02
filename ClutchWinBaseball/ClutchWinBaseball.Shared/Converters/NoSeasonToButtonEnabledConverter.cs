using System;
using Windows.UI.Xaml.Data;

namespace ClutchWinBaseball.Converters
{
    public class NoSeasonToButtonEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string stringValue = value as string;
            if (!string.IsNullOrEmpty(stringValue) && !stringValue.Contains("season"))
            {
                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return true;
        }
    }
}
