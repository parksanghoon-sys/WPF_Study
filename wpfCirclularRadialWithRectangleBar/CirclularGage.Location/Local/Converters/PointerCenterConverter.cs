using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;


namespace CirclularGage.Location.Local.Converters
{
    [ValueConversion(typeof(string),typeof(TransformGroup))]
    internal class PointerCenterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is double pointerLengh)
            {
                TransformGroup tg = new TransformGroup();
                RotateTransform rt = new RotateTransform();
                TranslateTransform tt = new TranslateTransform();

                tt.X = pointerLengh / 2;
                tg.Children.Add(rt);
                tg.Children.Add(tt);
                return tg;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
