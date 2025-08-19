using System.Windows;
using System.Windows.Controls;
using wpfCleanArchtecture.Presentation.ViewModels;

namespace wpfCleanArchtecture.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            //DataContext = mainViewModel;
            UpdateVisualStates();
        }
        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(MainViewModel.IsLoading):
                    UpdateLoadingState();
                    break;
                case nameof(MainViewModel.FilteredTodos):
                    UpdateDataState();
                    break;
            }
        }

        private void UpdateVisualStates()
        {
            UpdateLoadingState();
            UpdateDataState();
        }

        private void UpdateLoadingState()
        {
            string stateName = _viewModel.IsLoading ? "Loading" : "NotLoading";
            VisualStateManager.GoToElementState(this, stateName, true);
        }

        private void UpdateDataState()
        {
            string stateName = _viewModel.FilteredTodos.Count == 0 ? "NoData" : "HasData";
            VisualStateManager.GoToElementState(this, stateName, true);
        }

        protected override void OnClosed(EventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
            }
            base.OnClosed(e);
        }
    }

    public interface IVisualStateService
    {
        void GoToState(FrameworkElement element, string stateName, bool useTransitions = true);
        void RegisterStateGroup(FrameworkElement element, string groupName, IEnumerable<string> stateNames);
        void UpdateStatesForProperty(FrameworkElement element, string propertyName, object value);
    }

    public class VisualStateService : IVisualStateService
    {
        private readonly Dictionary<FrameworkElement, Dictionary<string, List<string>>> _stateGroups = new();
        private readonly Dictionary<string, Func<object, string>> _propertyToStateMappers = new();

        public void GoToState(FrameworkElement element, string stateName, bool useTransitions = true)
        {
            if (element is Control control)
            {
                VisualStateManager.GoToState(control, stateName, useTransitions);
            }
            else
            {
                VisualStateManager.GoToElementState(element, stateName, useTransitions);
            }
        }

        public void RegisterStateGroup(FrameworkElement element, string groupName, IEnumerable<string> stateNames)
        {
            if (!_stateGroups.ContainsKey(element))
            {
                _stateGroups[element] = new Dictionary<string, List<string>>();
            }

            _stateGroups[element][groupName] = new List<string>(stateNames);
        }

        public void UpdateStatesForProperty(FrameworkElement element, string propertyName, object value)
        {
            if (_propertyToStateMappers.TryGetValue(propertyName, out var mapper))
            {
                string stateName = mapper(value);
                GoToState(element, stateName);
            }
        }

        public void RegisterPropertyMapper(string propertyName, Func<object, string> mapper)
        {
            _propertyToStateMappers[propertyName] = mapper;
        }
    }
}