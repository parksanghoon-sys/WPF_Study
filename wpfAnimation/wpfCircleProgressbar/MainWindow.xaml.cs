﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpfCircleProgressbar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private double _value1;
        private double _value2;
        private double _value3;
        private double _value4;

        public double Value1
        {
            get => _value1;
            set { _value1 = value; OnPropertyChanged(); }
        }

        public double Value2
        {
            get => _value2;
            set { _value2 = value; OnPropertyChanged(); }
        }

        public double Value3
        {
            get => _value3;
            set { _value3 = value; OnPropertyChanged(); }
        }

        public double Value4
        {
            get => _value4;
            set { _value4 = value; OnPropertyChanged(); }
        }


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += (sender, args) => InitializeTimers();
        }
        private void InitializeTimers()
        {
            var timer1 = new System.Timers.Timer(100);
            timer1.Elapsed += (sender, args) =>
            {
                Value1 += 1;
                if (Value1 >= 100)
                    Value1 = 0;
            };
            timer1.Start();
            var timer2 = new System.Timers.Timer(150);
            timer2.Elapsed += (sender, args) =>
            {
                Value2 += 2;
                if (Value2 >= 100)
                    Value2 = 0;
            };
            timer2.Start();
            var timer3 = new System.Timers.Timer(200);
            timer3.Elapsed += (sender, args) =>
            {
                Value3 += 2;
                if (Value3 >= 100)
                    Value3 = 0;
            };
            timer3.Start();
            var timer4 = new System.Timers.Timer(100);
            timer4.Elapsed += (sender, args) =>
            {
                Value4 += 3;
                if (Value4 >= 100)
                    Value4 = 0;
            };
            timer4.Start();
        }
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}