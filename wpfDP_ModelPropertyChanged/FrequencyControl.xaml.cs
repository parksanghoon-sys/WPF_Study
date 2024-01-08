using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using WpfMVVMLib;

namespace wpfDP_ModelPropertyChanged
{
    /// <summary>
    /// FrequencyControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FrequencyControl : UserControl, INotifyPropertyChanged
    {
        public FrequencyModel FrequencyValue
        {
            get { return (FrequencyModel)GetValue(FrequencyValueProperty); }
            set { SetValue(FrequencyValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FrequencyValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FrequencyValueProperty =
            DependencyProperty.Register("FrequencyValue", typeof(FrequencyModel), typeof(FrequencyControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnFrequencyModelChanged)));

        private static void OnFrequencyModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as FrequencyControl;
            var model = (FrequencyModel)e.NewValue;
            model.PropertyChanged += (sender, args) =>
            {
                var data = sender as FrequencyModel;
                if (args.PropertyName == nameof(FrequencyModel.Frequency))
                {
                    
                    control.txtFrequency.Text = data.Frequency.ToString();
                }
                if (args.PropertyName == nameof(FrequencyModel.IsActived))
                {
                    if (data.IsActived == true)
                        control.txtFrequency.Text += 'A';
                    else
                        control.TxtFrequency = control.TxtFrequency.Replace("A","");
                }

            };
        }

        public FrequencyControl()
        {
            InitializeComponent();            
            _txtFrequency = "Test";
        }
        private string _txtFrequency;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string parameter = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(parameter));
        }

        public string TxtFrequency
        {
            get { return _txtFrequency; }
            set { _txtFrequency = value; OnPropertyChanged(); }
        }
            
        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            FrequencyValue.Frequency += 0.5;
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            FrequencyValue.Frequency -= 0.5;
        }
    }
}
