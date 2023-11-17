using CommunityToolkit.Mvvm.ComponentModel;
using Custom.Wpf.Global.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace wpfSiderBarMenu.Local.ViewModels
{
    public partial class Navigation2ViewModel : ObservableBase
    {
        [ObservableProperty]
        private string _test = string.Empty;
        public Navigation2ViewModel()
        {
            Test = "Test1";
        }
    }
}
