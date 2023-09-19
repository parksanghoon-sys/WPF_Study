using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CirclularGage.Location.UI.Units
{
    public class AirPortSympbolBackground : ContentControl
    {

        /// <summary>
        /// 비행체 심볼 Background 크기
        /// </summary>
        public static readonly DependencyProperty AirPortSymbolBackgroundSizeProperty =
            DependencyProperty.Register("AirPortSymbolBackgroundSize", typeof(double), typeof(AirPortSympbolBackground), new PropertyMetadata(null));

        public double AirPortSymbolBackgroundSize
        {
            get { return (double)GetValue(AirPortSymbolBackgroundSizeProperty); }
            set { SetValue(AirPortSymbolBackgroundSizeProperty, value); }
        }

        static AirPortSympbolBackground()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AirPortSympbolBackground), new FrameworkPropertyMetadata(typeof(AirPortSympbolBackground)));
        }
    }
}
