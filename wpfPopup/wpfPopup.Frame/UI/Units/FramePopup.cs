using System.Windows;
using wpfPopup.Local.Interface;

namespace wpfPopup.Frame.UI.Units
{
    public class FramePopup : Window, IView
    {
        static FramePopup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FramePopup), new FrameworkPropertyMetadata(typeof(FramePopup)));
        }
    }
}
