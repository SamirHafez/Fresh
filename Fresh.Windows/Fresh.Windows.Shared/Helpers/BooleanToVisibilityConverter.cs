using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Fresh.Windows.Shared.Helpers
{
    public class BooleanToVisibilityConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var bValue = (bool)value;

            return bValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var visibility = (Visibility)value;

            return visibility == Visibility.Visible;
        }
    }
}
