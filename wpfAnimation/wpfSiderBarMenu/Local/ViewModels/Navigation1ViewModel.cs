using CommunityToolkit.Mvvm.ComponentModel;
using Custom.Wpf.Global.Mvvm;

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
