using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CirclularGage.Location.UI.Units
{
    public class AirPortSymbol : ContentControl
    {

        // Using a DependencyProperty as the backing store for AirPortSymbolSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AirPortSymbolSizeProperty =
            DependencyProperty.Register("AirPortSymbolSize", typeof(double), typeof(AirPortSymbol), new PropertyMetadata(null));
        /// <summary>
        /// Air Heading 각도
        /// </summary>
        public static readonly DependencyProperty HeadAngeleroperty =
            DependencyProperty.Register("HeadAngeleValue", typeof(double), typeof(AirPortSymbol), new PropertyMetadata(null));

        public double AirPortSymbolSize
        {
            get { return (double)GetValue(AirPortSymbolSizeProperty); }
            set { SetValue(AirPortSymbolSizeProperty, value); }
        }
        public double HeadAngeleValue
        {
            get { return (double)GetValue(HeadAngeleroperty); }
            set { SetValue(HeadAngeleroperty, value); }
        }

        static AirPortSymbol()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AirPortSymbol), new FrameworkPropertyMetadata(typeof(AirPortSymbol)));
        }
    }
}
