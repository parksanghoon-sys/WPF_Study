using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
        public TcasControlView()
        {
            int a = 0;
            this.Loaded += TcasControlView_Loaded;
        }

        private void TcasControlView_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
