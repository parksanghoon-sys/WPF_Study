using System;
using System.Globalization;
using System.Windows.Data;

namespace CirclularGage.Location.Local.Converters
{
    public class IntruderAltitudeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is double altitude)
            {
                if(altitude > 0)
                    return "+" + altitude.ToString();
                else 
                    return altitude.ToString();
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class NegativePositiveConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double altitude)
            {
                if (altitude > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
