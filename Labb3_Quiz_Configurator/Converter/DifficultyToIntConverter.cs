using System;
using System.Globalization;
using System.Windows.Data;
using static Labb3_Quiz_Configurator.ViewModel.QuestionPackViewModel;
using Labb3_Quiz_Configurator.Enums;

namespace Labb3_Quiz_Configurator.Converter
{
    internal class DifficultyToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Difficulty difficulty)
            {
                return (int)difficulty; // Konvertera enum till int
            }
            return 0; // Om värdet inte är en Difficulty, returnera 0 eller hantera det på ett lämpligt sätt
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                // Kontrollera om int-värdet är giltigt för enum
                if (Enum.IsDefined(typeof(Difficulty), intValue))
                {
                    return (Difficulty)intValue; // Konvertera int tillbaka till enum
                }
            }
            throw new InvalidOperationException("Invalid value for Difficulty.");
        }
    }
}
