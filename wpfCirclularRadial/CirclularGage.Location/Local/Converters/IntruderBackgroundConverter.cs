
using CirclularGage.Location.Local.Enums;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CirclularGage.Location.Local.Converters
{
    public class IntruderBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var intruderType = (TcasSymbol)value;
            var bgColor = new SolidColorBrush();
            switch (intruderType)
            {
                case TcasSymbol.OtherTraffic:
                    bgColor.Color = Colors.White;
                    break;
                case TcasSymbol.ProximateTraffic:
                    bgColor.Color = Colors.White;
                    break;
                case TcasSymbol.TrafficAdvisory:
                    bgColor.Color = Colors.Yellow;
                    break;
                case TcasSymbol.ResolutionAdvisorty:
                    bgColor.Color = Colors.Red;
                    break;
                default:
                    bgColor.Color = Colors.Blue;
                    break;
            }
            return bgColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
