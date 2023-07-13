using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace wpfDependencyProperty
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private decimal? _value1;
        [ObservableProperty]
        private decimal? _value2;
        [ObservableProperty]
        private string _operator;
        public MainViewModel()
        {
            Value1 = 10;
            Value2 = 20;
            Operator = "+";

            var timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1),
            };
            timer.Tick += (s, e) =>
            {
                Value1 += 1;
                Value2 += 2;
            };
            timer.Start();
        }

    }
}
