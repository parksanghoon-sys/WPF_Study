using CommunityToolkit.Mvvm.ComponentModel;
using wpfPopup.Global;

namespace wpfPopup.Popup.Local.ViewModels
{
    public partial class Popup1ViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string? _text;
        public Popup1ViewModel()
        {
            Text = "TESETETETETETET";
        }
    }
}
