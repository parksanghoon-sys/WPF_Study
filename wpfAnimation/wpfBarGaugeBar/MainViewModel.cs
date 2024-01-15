using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
        public MainViewModel()
        {
            
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
