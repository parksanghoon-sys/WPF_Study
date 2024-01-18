using System.Windows.Controls;
using System.Windows;

namespace wpfCautionTextbox
{
    public class YourCustomTextBox : TextBox
    {
        public static readonly DependencyProperty IsHighlightedProperty =
            DependencyProperty.Register("IsHighlighted", typeof(bool), typeof(YourCustomTextBox));

        public bool IsHighlighted
        {
            get { return (bool)GetValue(IsHighlightedProperty); }
            set { SetValue(IsHighlightedProperty, value); }
        }


        public string CautionText
        {
            get { return (string)GetValue(CautionTextProperty); }
            set { SetValue(CautionTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CautionText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CautionTextProperty =
            DependencyProperty.Register("CautionText", typeof(string), typeof(YourCustomTextBox), new PropertyMetadata("",new PropertyChangedCallback(OnCHanged)));

        private static void OnCHanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var data = d as YourCustomTextBox;
            if (data != null)
            {
                if(e.NewValue != null)
                {
                    //data.IsHighlighted = true;
                }
            }
        }
    }
}
