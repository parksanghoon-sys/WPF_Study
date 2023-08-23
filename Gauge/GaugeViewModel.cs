using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gauge
{
    public class GaugeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            if(this.PropertyChanged!=null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        /// <summary>
        /// 角度属性
        /// </summary>
        private double _angle;
        public double Angle
        {
            get { return _angle; }
           private set { _angle = value;this.NotifyPropertyChanged("Angle"); }
        }
        /// <summary>
        /// slider的值属性
        /// </summary>
        private double _value;
        public double Value
        {
            get { return _value; }
            
            set {
                if(value>0 && value<360)
                {
                    _value = value;
                    this.NotifyPropertyChanged("Value");
                    this.Angle = value - 75;
                }
            }
        }


    }
}
