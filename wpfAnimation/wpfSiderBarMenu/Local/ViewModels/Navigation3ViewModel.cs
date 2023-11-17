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
    public partial class Navigation3ViewModel : ObservableBase
    {
        [ObservableProperty]
        private string _test = string.Empty;
        public Navigation3ViewModel()
        {
            Test = "Test1";
        }
    }
}
