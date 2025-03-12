using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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

namespace wpfIffUI.Control
{
    public partial class UpdownFrequencyControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Frequency Textbox 글자색상
        /// </summary>
        public static readonly DependencyProperty TextForegroundProperty =
            DependencyProperty.Register("TextForeground", typeof(Brush), typeof(UpdownFrequencyControl), new PropertyMetadata(Brushes.White, TextForegroundChanged));
        /// <summary>
        /// Up, Down Button 클릭 시 증감소 량
        /// </summary>
        public static readonly DependencyProperty SpinIncrementProperty =
            DependencyProperty.Register("SpinIncrement", typeof(double), typeof(UpdownFrequencyControl), new PropertyMetadata(0.1));
        /// <summary>
        /// 최소 값
        /// </summary>
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(UpdownFrequencyControl), new PropertyMetadata(double.MinValue));
        /// <summary>
        /// 최대 값
        /// </summary>
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(UpdownFrequencyControl), new PropertyMetadata(double.MaxValue));
        /// <summary>
        /// 두번 쨰 최소 값
        /// </summary>
        public static readonly DependencyProperty SecondMinValueProperty =
            DependencyProperty.Register("SecondMinValue", typeof(double), typeof(UpdownFrequencyControl), new PropertyMetadata(0.0));
        /// <summary>
        /// 두번 째 최대 값
        /// </summary>
        public static readonly DependencyProperty SecondMaxValueProperty =
            DependencyProperty.Register("SecondMaxValue", typeof(double), typeof(UpdownFrequencyControl), new PropertyMetadata(100.0));
        /// <summary>
        /// 두번째 최대 최소 값을 현재 값에 적용 여부
        /// </summary>
        public static readonly DependencyProperty SecondIsEnabledProperty =
            DependencyProperty.Register("SecondIsEnabled", typeof(bool), typeof(UpdownFrequencyControl), new PropertyMetadata(false));
        /// <summary>
        /// 현재 값
        /// </summary>
        public static readonly DependencyProperty FrequencyValueProperty =
            DependencyProperty.Register("FrequencyValue", typeof(FrequencyModel), typeof(UpdownFrequencyControl), new FrameworkPropertyMetadata()
            {
                DefaultValue = new FrequencyModel(),
                BindsTwoWayByDefault = true,
                //PropertyChangedCallback = OnFrequencyValueChanged,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
        /// <summary>
        /// 현재 값 정상 여부 및 WarningText 노출 여부
        /// 정상 : false
        /// 오류 : true
        /// </summary>
        public static readonly DependencyProperty IsWarningProperty =
            DependencyProperty.Register("IsWarning", typeof(bool), typeof(UpdownFrequencyControl), new PropertyMetadata(false));
        /// <summary>
        /// 경고시 ToolTip 문구
        /// </summary>
        public static readonly DependencyProperty WarningTextProperty =
            DependencyProperty.Register("WarningText", typeof(string), typeof(UpdownFrequencyControl));

        public Brush TextForeground
        {
            get { return (Brush)GetValue(TextForegroundProperty); }
            set { SetValue(TextForegroundProperty, value); }
        }
        public double SpinIncrement
        {
            get { return (double)GetValue(SpinIncrementProperty); }
            set { SetValue(SpinIncrementProperty, value); }
        }
        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }
        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }
        public double SecondMinValue
        {
            get { return (double)GetValue(SecondMinValueProperty); }
            set { SetValue(SecondMinValueProperty, value); }
        }
        public double SecondMaxValue
        {
            get { return (double)GetValue(SecondMaxValueProperty); }
            set { SetValue(SecondMaxValueProperty, value); }
        }

        public bool SecondIsEnabled
        {
            get { return (bool)GetValue(SecondIsEnabledProperty); }
            set { SetValue(SecondIsEnabledProperty, value); }
        }
        public FrequencyModel FrequencyValue
        {
            get { return (FrequencyModel)GetValue(FrequencyValueProperty); }
            set { SetValue(FrequencyValueProperty, value); }
        }
        public bool IsWarning
        {
            get { return (bool)GetValue(IsWarningProperty); }
            set { SetValue(IsWarningProperty, value); }
        }
        public string WarningText
        {
            get { return (string)GetValue(WarningTextProperty); }
            set { SetValue(WarningTextProperty, value); }
        }
        public UpdownFrequencyControl()
        {
            InitializeComponent();
            this.DataContextChanged += UpdownFrequencyControl_DataContextChanged;

            tbFrequency.MouseDoubleClick += tbFrequency_MouseDoubleClick;
            tbFrequency.PreviewKeyDown += tbFrequency_PreviewKeyDown;
            tbFrequency.KeyDown += tbFrequency_KeyDown;
            tbFrequency.Foreground = new SolidColorBrush(Colors.White);
            tbFrequency.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(OnLostKeyboardFocus);
        }

        #region DP_CallbackMethods
        private static void TextForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var myObject = d as UpdownFrequencyControl;

            if (myObject != null)
            {
                myObject.tbFrequency.Foreground = (Brush)e.NewValue;
            }
        }
        #endregion

        #region ViewEventMethods
        private void UpdownFrequencyControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.DataContext is UpDwonFrequencyViewModel)
            {
                var datacontext = DataContext as UpDwonFrequencyViewModel;

                PropertyBinding("SpinIncrement", this, SpinIncrementProperty, BindingMode.TwoWay, datacontext);
                PropertyBinding("MinValue", this, MinValueProperty, BindingMode.TwoWay, datacontext);
                PropertyBinding("MaxValue", this, MaxValueProperty, BindingMode.TwoWay, datacontext);
                PropertyBinding("FrequencyValue", this, FrequencyValueProperty, BindingMode.TwoWay, datacontext);
                PropertyBinding("IsEnabled", this, IsEnabledProperty, BindingMode.TwoWay, datacontext);

                PropertyBinding("SecondMinValue", this, SecondMinValueProperty, BindingMode.TwoWay, datacontext);
                PropertyBinding("SecondMaxValue", this, SecondMaxValueProperty, BindingMode.TwoWay, datacontext);
                PropertyBinding("SecondIsEnabled", this, SecondIsEnabledProperty, BindingMode.TwoWay, datacontext);

                PropertyBinding("IsWarning", this, IsWarningProperty, BindingMode.TwoWay, datacontext);
                PropertyBinding("WarningText", this, WarningTextProperty, BindingMode.TwoWay, datacontext);

                FrequencyValue.SetFreqencyModel(MinValue.ToString());
            }
        }
        private void tbFrequency_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void tbFrequency_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            tbFrequency.SelectAll();
        }
        private void tbFrequency_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null) return;

            // 현재 입력된 텍스트
            string currentText = textBox.Text;
            int selectionStart = textBox.SelectionStart;
            int selectionLength = textBox.SelectionLength;

            // 새 입력이 들어왔을 때 전체 선택된 경우 처리
            if (selectionLength > 0)
            {
                // 기존 텍스트를 지우고 새 입력을 적용
                currentText = e.Text;
            }
            else
            {
                // 기존 텍스트에 새 입력을 삽입
                currentText = currentText.Insert(selectionStart, e.Text);
            }

            // 정규식 패턴: 맨 앞에 'A'|'B' 혹은 'a'| 'b'가 올 수 있고, 그 뒤에 소수점 3자리까지의 숫자만 허용. 소수점 다음에 숫자가 없는 경우도 허용.
            Regex regex = new Regex("^[abAB]?[0-9]*([.][0-9]{0,3})?$");

            bool isValid = regex.IsMatch(currentText);
            bool isFull = currentText.Length > 7; // 최대 7글자 제한

            if (!isValid || isFull)
            {
                e.Handled = true;
                WarningText = "Invalid Frequency";
            }
            else
            {
                e.Handled = false;
                WarningText = string.Empty;
            }

        }
        private void tbFrequency_KeyDown(object sender, KeyEventArgs e)
        {         
            if (e.Key == Key.Tab)
            {
                this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
            else if (e.Key == Key.Enter)
            {
                FrequencyValue.SetFreqencyModel(tbFrequency.Text);
            }
        }
        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            FrequencyValue.IncreaseFreqeuncy(SpinIncrement);
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            FrequencyValue.ReductionFreqeuncy(SpinIncrement);
        }
        private void OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var frequencyControl = sender as TextBox;
            if(frequencyControl != null)
            {
                FrequencyValue.InputFrequency = frequencyControl.Text;
            }      
        }
        #endregion

        #region private Methods
        protected void PropertyBinding(string propertyPath, DependencyObject control, DependencyProperty property, BindingMode mode, object source)
        {
            Binding myBinding = new Binding();
            myBinding.Source = source;
            myBinding.Path = new PropertyPath(propertyPath);
            myBinding.Mode = mode;
            myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(control, property, myBinding);
        }
  
        #endregion
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
