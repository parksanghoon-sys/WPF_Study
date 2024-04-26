using System.Windows.Controls;
using System.Windows;

namespace SmartDateControl.UI.Units
{
    public class CalendarBox : ListBox
    {
        static CalendarBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarBox), new FrameworkPropertyMetadata(typeof(CalendarBox)));
        }
    }
}