using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wpfPopup.Frame.Local.ViewModels;
using wpfPopup.Frame.UI.Units;
using wpfPopup.Local.Interface;

namespace wpfPopup.Frame.UI.View
{
  
    public class MainWindow : Window, IMainView
    {
        static MainWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MainWindow), new FrameworkPropertyMetadata(typeof(MainWindow)));
        }
        public MainWindow()
        {
            this.DataContext = new MainViewModel(this);
        }
        public bool? ShowPopupWindow()
        {
            FramePopup framePopup = new FramePopup();
            return framePopup.ShowDialog();
        }
    }
}
