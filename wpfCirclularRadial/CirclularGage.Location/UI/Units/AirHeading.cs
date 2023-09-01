using System.Windows;
using System.Windows.Controls;

namespace CirclularGage.Location.UI.Units
{
    public class AirHeading : ContentControl
    {
        // Using a DependencyProperty as the backing store for Angele.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeadAngeleroperty =
            DependencyProperty.Register("HeadAngeleValue", typeof(double), typeof(AirHeading),new PropertyMetadata(null));
        public double HeadAngeleValue
        {
            get { return (double)GetValue(HeadAngeleroperty); }
            set { SetValue(HeadAngeleroperty, value); }
        }
        static AirHeading()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AirHeading), new FrameworkPropertyMetadata(typeof(AirHeading)));
        }
  
    }
}
