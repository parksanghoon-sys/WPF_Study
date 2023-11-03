using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
