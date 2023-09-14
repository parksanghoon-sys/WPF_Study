﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace CirclularGage.Location.Local.Converters
{
    public class RadiusToDiameterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {            
            if(value is double radios)
                return radios * 2;
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
