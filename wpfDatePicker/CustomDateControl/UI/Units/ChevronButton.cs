using System.Windows.Controls;
using System.Windows;

namespace CustomDateControl.UI.Units
{
    public class ChevronButton : Button
    {
        static ChevronButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChevronButton), new FrameworkPropertyMetadata(typeof(ChevronButton)));
        }
    }
}