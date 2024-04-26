using System.Windows;

namespace wpfDatePicker.UI.Views
{
    public class DateWindow : Window
    {
        static DateWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateWindow), new FrameworkPropertyMetadata(typeof(DateWindow)));
        }
    }
}
