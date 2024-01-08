using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfMVVMLib;

namespace wpfDP_ModelPropertyChanged
{
    public class MainViewModel : ViewModelBase
    {
        private ICommand _onBtn1ClickCommand;
        public ICommand OnBtn1ClickCommand
        {
            get { return _onBtn1ClickCommand; }
            set { _onBtn1ClickCommand = value;}
        }
        private FrequencyModel _frequencyModel;
        public FrequencyModel FrequencyModel
        {
            get { return _frequencyModel; }
            set { _frequencyModel = value; OnPropertyChanged();}
        }
        public MainViewModel()
        {
            OnBtn1ClickCommand = new RelayCommand(OnBtnClick);
            _frequencyModel = new FrequencyModel();
        }

        private void OnBtnClick()
        {
            var random = new Random();
            FrequencyModel.Frequency = random.Next(100, 300) + Math.Round(random.NextDouble(), 4);
            FrequencyModel.IsActived = !FrequencyModel.IsActived;
        }
    }
}
