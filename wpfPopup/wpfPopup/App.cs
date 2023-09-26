using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using wpfPopup.Frame.Local.ViewModels;
using wpfPopup.Frame.UI.View;

namespace wpfPopup
{
    internal class App : Window
    {
        public App()
        {            
            MainWindow window = new();
            new Bootstrapper();
            //window.DataContext = Ioc.Default.GetService<MainViewModel>();
            window.ShowDialog();
        }

    }
}
