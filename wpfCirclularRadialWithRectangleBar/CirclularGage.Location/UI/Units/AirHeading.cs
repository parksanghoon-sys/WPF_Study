using System.Windows;
using System.Windows.Controls;

namespace CirclularGage.Location.UI.Units
{
    public class AirHeading : ContentControl
    {
        /// <summary>
        /// Air Heading 각도
        /// </summary>
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
