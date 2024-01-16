using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace wpfBarGaugeBar
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private double _score;

        public double Score
        {
            get { return _score; }
            set { _score = value;  OnPropertyChanged(); }
        }
        private bool _led1Status;

        public bool Led1Status
        {
            get { return _led1Status; }
            set { _led1Status = value;  OnPropertyChanged(); }
        }
        private bool _led2Status;

        public bool Led2Status
        {
            get { return _led2Status; }
            set { _led2Status = value; OnPropertyChanged(); }
        }
        private ICommand _ledChangedCommand;        

        public ICommand LedChangedCommand
        {
            get { return _ledChangedCommand; }
            set { _ledChangedCommand = value; }
        }

        private void OnClick()
        {
            Led2Status = !Led2Status;
            Led1Status = !Led1Status;
        }

        public MainViewModel()
        {
            _ledChangedCommand = new RelayCommand(new Action(OnClick));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public interface IComand
    {
    }
}
