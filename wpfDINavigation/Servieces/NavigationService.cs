using System.ComponentModel;
using wpfDINavigation.Stores;
using wpfDINavigation.ViewModels;
using wpfDINavigation.ViewModels.MainControlViewModels;

namespace wpfDINavigation.Servieces
{
    public class NavigationService : INavigationService
    {
        private readonly MainNavigationStore _mainNavigationStore;
        private INotifyPropertyChanged? CurrentViewModel
        {
            set => _mainNavigationStore.CurrentViewModel = value;
        }
        public NavigationService(MainNavigationStore mainNavigationStore)
        {
            _mainNavigationStore = mainNavigationStore;
        }
        public void Navigate(NaviType naviType)
        {
            switch (naviType)
            {
                case NaviType.LoginView:
                    CurrentViewModel = (ViewModelBase)App.Current.Services.GetService(typeof(LoginViewModel))!;
                    break;
                case NaviType.SignupView:
                    CurrentViewModel = (ViewModelBase)App.Current.Services.GetService(typeof(SignupViewModel))!;
                    break;
                case NaviType.TestView:
                    CurrentViewModel = (ViewModelBase)App.Current.Services.GetService(typeof(TestViewModel))!;
                    break;
                default:
                    return;
            }
        }
    }
}
