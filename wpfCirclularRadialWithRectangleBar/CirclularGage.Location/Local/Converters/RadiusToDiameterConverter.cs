using CirclularGage.Location.Local.Common;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CirclularGage.Location.Local.Converters
{
    [ValueConversion(typeof(double), typeof(double))]
    public class RadiusToDiameterConverter : ConverterMarkupExtension<RadiusToDiameterConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double radios)
            {
                var result = radios * GetParameter(parameter);
                return result;

            }
            return 0;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// double 형 매개 변수 구하기
        /// </summary>
        /// <param name="parameter">매개 변수</param>
        /// <returns>매개 변수</returns>
        private double GetParameter(object parameter)
        {
            if (parameter is float)
            {
                return (float)parameter;
            }
            else if (parameter is int)
            {
                return (int)parameter;
            }
            else if (parameter is string)
            {
                return float.Parse((string)parameter);
            }

            return 1;
        }
    }
}
