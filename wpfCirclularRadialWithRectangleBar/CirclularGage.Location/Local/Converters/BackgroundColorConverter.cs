using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CirclularGage.Location.Local.Converters
{
    public class BackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Color backgroundColor)
            {
                RadialGradientBrush brush = new RadialGradientBrush();
                GradientStop g1 = new GradientStop();
                g1.Offset = 0.94;
                g1.Color = backgroundColor;
                GradientStop g2 = new GradientStop();
                g2.Color = Color.FromArgb(0xFF, 0xAF, 0xB2, 0xB0);
                brush.GradientStops.Add(g1);
                brush.GradientStops.Add(g2);
                return brush;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
