using System.Windows;
using System.Windows.Controls.Primitives;

namespace Custom.Wpf.Global.Controls
{
    public class RepeatColorChangeToggleButton : ToggleButton
    {
        static RepeatColorChangeToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RepeatColorChangeToggleButton), new FrameworkPropertyMetadata(typeof(RepeatColorChangeToggleButton)));
        }
    }
}
