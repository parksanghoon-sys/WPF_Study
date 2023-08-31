using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace CirclularGage.Location.UI.Units
{
    public class TcasIntruderItem : ContentControl
    {
        static TcasIntruderItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TcasIntruderItem), new FrameworkPropertyMetadata(typeof(TcasIntruderItem)));
        }
    }
}
