using CommunityToolkit.Mvvm.ComponentModel;
using Custom.Wpf.Global.Mvvm;

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
