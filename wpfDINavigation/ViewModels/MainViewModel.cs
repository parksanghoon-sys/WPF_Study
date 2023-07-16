using System.ComponentModel;
using wpfDINavigation.Servieces;
using wpfDINavigation.Stores;

namespace wpfDINavigation.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly MainNavigationStore _mainNavigationStore;
        private INotifyPropertyChanged? _currentViewModel;
        public MainViewModel(MainNavigationStore mainNavigationStore, INavigationService navigationService)
        {            
            _mainNavigationStore = mainNavigationStore;
            _mainNavigationStore.CurrentViewModelChanged += CurrentViewModelChanged;

            navigationService.Navigate(NaviType.LoginView);
        }

        private void CurrentViewModelChanged()
        {
            CurrentViewModel = _mainNavigationStore.CurrentViewModel;
        }

        public INotifyPropertyChanged? CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (_currentViewModel != value)
                {
                    _currentViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

    }
}
