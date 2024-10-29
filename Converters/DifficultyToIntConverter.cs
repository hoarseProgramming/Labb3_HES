using Labb3_HES.Model;
using System.Globalization;
using System.Windows.Data;

namespace Labb3_HES.Converters
{
    class DifficultyToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Difficulty difficulty = (Difficulty)value;

            return difficulty switch
            {
                Difficulty.Easy => 0,
                Difficulty.Medium => 1,
                Difficulty.Hard => 2,
                _ => 3
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int difficultyInt = (int)value;

            return difficultyInt switch
            {
                0 => Difficulty.Easy,
                1 => Difficulty.Medium,
                2 => Difficulty.Hard,
                _ => Difficulty.Medium
            };
        }
    }
}
