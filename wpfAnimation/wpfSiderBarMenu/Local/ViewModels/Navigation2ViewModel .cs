using CommunityToolkit.Mvvm.ComponentModel;
using Custom.Wpf.Global.Mvvm;

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
