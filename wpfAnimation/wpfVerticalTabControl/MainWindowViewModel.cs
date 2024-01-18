using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace wpfVerticalTabControl
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<TabModel> tabModels = new ObservableCollection<TabModel>();

        public MainWindowViewModel()
        {
            this.TabModels.Add(new TabModel()
            {
                ItemName = "Analytics",
                ItemContent = "AnalyticsContent"
            });
            this.TabModels.Add(new TabModel()
            {
                ItemName = "Reports",
                ItemContent = "ReportsContent"
            });
            this.TabModels.Add(new TabModel()
            {
                ItemName = "Performance",
                ItemContent = "PerformanceContent"
            });
            this.TabModels.Add(new TabModel()
            {
                ItemName = "Funds",
                ItemContent = "FundsContent"
            });
        }
    }
}
