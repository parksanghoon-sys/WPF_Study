using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using wpfPopup.Frame.UI.View;
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
