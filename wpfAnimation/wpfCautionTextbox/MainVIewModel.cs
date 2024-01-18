using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Custom.Wpf.Global.Mvvm;
using System.Windows.Input;

namespace wpfCautionTextbox
{
    public partial class  MainVIewModel : ObservableBase
    {
        [ObservableProperty]
        private bool _isCaution;
        
        private string? _cautionText;
        public string CautionText
        {
            get => _cautionText!;
            set
            {                
                SetProperty(ref _cautionText, value);
            }
        }
        public ICommand SaveCommand { get; init; }
        [RelayCommand]
        private void OnBtnClick()
        {
            IsCaution = true;
            CautionText = "경고 입력";
            
        }

        private void Save()
        {
            IsCaution = !IsCaution;
            CautionText = "경고 입력";
        }

        public MainVIewModel()
        {
            IsCaution = false;
            SaveCommand = new RelayCommand(Save);
          
        }

    }
}
