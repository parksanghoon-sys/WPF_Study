using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace wpfSiderBarMenu.UI.Units
{
    public class Navigation1 : ContentControl
    {
        static Navigation1()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Navigation1), new FrameworkPropertyMetadata(typeof(Navigation1)));
        }
       
    }
}
