using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace wpfThreadExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Dispatcher 클래스
    /// 스레드에 대한 작업 항목 큐를 관리 하는 서비스를 제공한다.
    /// </summary>
    public partial class MainWindow : Window
    {
        public delegate void NextPrimeDelegate();
        private bool _continueCalculating;
        private bool _notAPrime;
        //Current number to check 
        private long _num = 3;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void StartOrStop(object sender, EventArgs e)
        {
            if(_continueCalculating)
            {
                _continueCalculating = false;
                startStopButton.Content = "Resum";
            }
            else
            {
                _continueCalculating = true;
                startStopButton.Content = "Stop";
                startStopButton.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new NextPrimeDelegate(CheckNextNumber));
            }
        }
        public void CheckNextNumber()
        {
            var x = new Stopwatch();
            x.Start();
            for (long i = 3; i <= Math.Sqrt(_num); i++)
            {
                if(_num % i == 0)
                {
                    _notAPrime = true;
                    break;
                }
            }
            if(!_notAPrime)
            {
                x.Stop();
                elapsed.Text = x.ElapsedMilliseconds.ToString();
                bigPrime.Text = _num.ToString();
            }
            _num += 2;
            if(_continueCalculating)
            {
                startStopButton.Dispatcher.BeginInvoke(
                        DispatcherPriority.SystemIdle,
                        new NextPrimeDelegate(CheckNextNumber));
            }
        }
    }
}
