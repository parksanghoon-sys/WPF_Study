
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
            var intruderType = (TcasIntruderSymbol)value;
            var bgColor = new SolidColorBrush();
            switch (intruderType)
            {
                case TcasIntruderSymbol.OtherTraffic:
                    bgColor.Color = Color.FromRgb(64,224,208);
                    break;
                case TcasIntruderSymbol.ProximateTraffic:
                    bgColor.Color = Color.FromRgb(64, 224, 208);
                    break;
                case TcasIntruderSymbol.TrafficAdvisory:
                    bgColor.Color = Colors.Yellow;
                    break;
                case TcasIntruderSymbol.ResolutionAdvisorty:
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
