using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfMVVMLib;

namespace wpfDP_ModelPropertyChanged
{
    public class FrequencyModel : ViewModelBase
    {
        private double _freqyency;
        private bool _isActive;
        public double Frequency 
        { 
            get {  return _freqyency; } 
            set { _freqyency = value; OnPropertyChanged(); }
        }
        public bool IsActived 
        {
            get { return _isActive; }
            set { _isActive = value; OnPropertyChanged(); } 
        }
        public FrequencyModel()
        {
            Frequency = 0;
            IsActived = false;
        }
    }
}
