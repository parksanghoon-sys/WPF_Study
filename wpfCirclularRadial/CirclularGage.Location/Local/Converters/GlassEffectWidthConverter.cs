using System;
using System.Globalization;
using System.Windows.Data;

namespace CirclularGage.Location.Local.Converters
{
    public class GlassEffectWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {            
            if (value is double width)
                return (width * 2) * 0.94;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
