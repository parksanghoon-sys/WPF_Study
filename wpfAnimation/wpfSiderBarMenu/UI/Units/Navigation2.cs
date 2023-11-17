using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace wpfSiderBarMenu.UI.Units
{
    public class Navigation2 : ContentControl
    {
        static Navigation2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Navigation2), new FrameworkPropertyMetadata(typeof(Navigation2)));
        }
    }
}
