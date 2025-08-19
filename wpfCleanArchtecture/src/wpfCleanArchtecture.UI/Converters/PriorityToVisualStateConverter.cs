using System.Globalization;
using System.Windows.Data;
using wpfCleanArchtecture.Domain;

namespace wpfCleanArchtecture.UI.Converters
{
    public class PriorityToVisualStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Priority priority)
            {
                return priority switch
                {
                    Priority.Low => "LowPriority",
                    Priority.Medium => "MediumPriority",
                    Priority.High => "HighPriority",
                    _ => "MediumPriority"
                };
            }
            return "MediumPriority";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
