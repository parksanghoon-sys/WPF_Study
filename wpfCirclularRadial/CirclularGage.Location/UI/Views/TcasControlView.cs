using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CirclularGage.Location.Themes.Views
{
    public class TcasControlView : ContentControl
    {
        static TcasControlView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TcasControlView), new FrameworkPropertyMetadata(typeof(TcasControlView)));
        }

    }
}
