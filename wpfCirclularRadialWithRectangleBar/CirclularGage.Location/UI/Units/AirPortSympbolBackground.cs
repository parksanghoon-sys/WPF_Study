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
        /// 비행체 심볼 Background Width 크기
        /// </summary>
        public static readonly DependencyProperty AirPortSymbolBackgroundWidthProperty =
            DependencyProperty.Register("AirPortSymbolBackgroundWidth", typeof(double), typeof(AirPortSympbolBackground), new PropertyMetadata(null));

        public double AirPortSymbolBackgroundWidth
        {
            get { return (double)GetValue(AirPortSymbolBackgroundWidthProperty); }
            set { SetValue(AirPortSymbolBackgroundWidthProperty, value); }
        }
        /// <summary>
        /// 비행체 심볼 Background Width 크기
        /// </summary>
        public static readonly DependencyProperty AirPortSymbolBackgroundHeightProperty =
            DependencyProperty.Register("AirPortSymbolBackgroundHeight", typeof(double), typeof(AirPortSympbolBackground), new PropertyMetadata(null));

        public double AirPortSymbolBackgroundHeight
        {
            get { return (double)GetValue(AirPortSymbolBackgroundHeightProperty); }
            set { SetValue(AirPortSymbolBackgroundHeightProperty, value); }
        }

        static AirPortSympbolBackground()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AirPortSympbolBackground), new FrameworkPropertyMetadata(typeof(AirPortSympbolBackground)));
        }
    }
}
