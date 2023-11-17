using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace wpfSiderBarMenu.UI.Units
{
    public class Navigation3 : ContentControl
    {
        static Navigation3()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Navigation3), new FrameworkPropertyMetadata(typeof(Navigation3)));
        }
    }
}
