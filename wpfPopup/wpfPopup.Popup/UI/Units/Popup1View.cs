using System.Windows;
using System.Windows.Controls;


namespace wpfPopup.Popup.UI.Units
{
    public class Popup1View : ContentControl
    {
        static Popup1View()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Popup1View), new FrameworkPropertyMetadata(typeof(Popup1View)));
        }
    }
}
