using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace wpfBarGaugeBar
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private double _score;
        private double _startSafeZoon;
        private double _endtSafeZoon;
        private double _startWarningZoon;
        private double _endWarningZoon;
        public double StartSafeZoon
        {
            get { return _startSafeZoon; }
            set { _startSafeZoon = value; OnPropertyChanged(); }
        }
        public double EndSafeZoon
        {
            get { return _endtSafeZoon; }
            set { _endtSafeZoon = value; OnPropertyChanged(); }
        }
        public double StartWarningZoon
        {
            get { return _startWarningZoon; }
            set { _startWarningZoon = value; OnPropertyChanged(); }
        }
        public double EndWarningZoon
        {
            get { return _endWarningZoon; }
            set { _endWarningZoon = value; OnPropertyChanged(); }
        }
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
            var random = new Random();
            Score = random.Next(-2, 2) + random.NextDouble();
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
