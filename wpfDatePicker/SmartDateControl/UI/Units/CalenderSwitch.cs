using System.Windows;
using System.Windows.Controls.Primitives;

namespace SmartDateControl.UI.Units
{
    public class CalendarSwitch : ToggleButton
    {
        static CalendarSwitch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarSwitch), new FrameworkPropertyMetadata(typeof(CalendarSwitch)));
        }
    }
}