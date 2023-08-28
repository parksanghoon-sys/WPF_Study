using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace CirclularGage.Local.Converters
{
    public class PointerCenterConverter : IValueConverter
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
