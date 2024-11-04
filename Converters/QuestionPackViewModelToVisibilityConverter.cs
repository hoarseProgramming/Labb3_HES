using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Labb3_HES.Converters
{
    class QuestionPackViewModelToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value == null ? Visibility.Hidden : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
