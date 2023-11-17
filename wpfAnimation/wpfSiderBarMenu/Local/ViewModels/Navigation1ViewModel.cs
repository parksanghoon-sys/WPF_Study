using CommunityToolkit.Mvvm.ComponentModel;
using Custom.Wpf.Global.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfSiderBarMenu.Local.ViewModels
{
    public partial class Navigation1ViewModel : ObservableBase
    {
        [ObservableProperty]
        private string _test = string.Empty;
        public Navigation1ViewModel()
        {
            Test = "Test1";
        }
    }
}
