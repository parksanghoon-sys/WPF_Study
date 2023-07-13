using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace wpfDependencyProperty
{
    /// <summary>
    /// CalculateControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CalculateControl : UserControl, INotifyPropertyChanged
    {
        // Using a DependencyProperty as the backing store for Value1.  This enables animation, styling, binding, etc...
        /// <summary>
        /// 인자값 정의
        /// 1 : 속성의 이름 
        /// 2 : 해당 속성의 타입 
        /// 3 : 이속성이 적용될 클래스의 타입, 
        /// 4 : 프로퍼티 메터데이터 객체는 기본 값, 만약 함수시 의존성 속성이 변경시 이벤트 발생 , 제약사항을 둘수도 있음, 데이터 유효성을 체크
        /// </summary>
        public static readonly DependencyProperty Value1Property =
            DependencyProperty.Register("Value1", typeof(decimal), typeof(CalculateControl), 
                new PropertyMetadata(0m,OnValueChanged, CoreceLimitValue));

        // Using a DependencyProperty as the backing store for Value2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Value2Property =
            DependencyProperty.Register("Value2", typeof(decimal), typeof(CalculateControl), 
                new PropertyMetadata(0m,OnValueChanged, CoreceLimitValue));
        public static readonly DependencyProperty OperatorProperty =
            DependencyProperty.Register("Operator", typeof(string), typeof(CalculateControl), 
                new PropertyMetadata("+", OnValueChanged),new ValidateValueCallback(IsValidOperator));

        // Using a DependencyProperty as the backing store for DegignMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DegignModeProperty =
            DependencyProperty.Register("DegignMode", typeof(DeginMode), typeof(CalculateControl), 
                new PropertyMetadata(DeginMode.White, OnDegingModeChanged));

        public DeginMode DegignMode
        {
            get { return (DeginMode)GetValue(DegignModeProperty); }
            set { SetValue(DegignModeProperty, value); }
        }
        public Brush TextBoxForeGround { get; set; } = Brushes.Black;
        public Brush TextBoxBackGround { get; set; } = Brushes.White;        
        public Brush UserControlBackground { get; set; } = Brushes.White;        

        private void ChangeDegignMode(DeginMode deginMode)
        {
            if(deginMode == DeginMode.White)
            {
                TextBoxForeGround = Brushes.Black;
                TextBoxBackGround = Brushes.White;
                UserControlBackground = Brushes.White;
                
            }else
            {
                TextBoxForeGround = Brushes.White;
                TextBoxBackGround = Brushes.DarkGray;
                UserControlBackground = Brushes.Black;
            }
            OnPropertyChanged(nameof(TextBoxBackGround));
            OnPropertyChanged(nameof(TextBoxForeGround));
            OnPropertyChanged(nameof(UserControlBackground));
        }
        private static void OnDegingModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CalculateControl calculateControl = (CalculateControl)d;
            if(e.NewValue != e.OldValue)
            {
                if(e.NewValue is DeginMode deginMode)
                {
                    calculateControl.ChangeDegignMode(deginMode);
                }
            }
        }

        private static bool IsValidOperator(object value)
        {
            string oper = (string)value;
            return oper switch
            {
                "+" or "-" or "*" or "/" => true,
                _=> false
            };
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CalculateControl calculate = (CalculateControl)d;
            calculate.OnPropertyChanged(nameof(Result));
        }
        private static object CoreceLimitValue(DependencyObject d, object baseValue)
        {
            decimal value = (decimal)baseValue;
            if (value < -999)
            {
                return -999m;
            }
            else if (value > 999)
            {
                return 999m;
            }
            else
                return value;
        }
        public string Operator
        {
            get { return (string)GetValue(OperatorProperty); }
            set { SetValue(OperatorProperty, value); }
        }
       
        /// <summary>
        /// DP 속성 메서드
        /// </summary>
        public decimal Value1
        {
            get { return (decimal)GetValue(Value1Property); }
            set { SetValue(Value1Property, value); }
        }
        public decimal Value2
        {
            get { return (decimal)GetValue(Value2Property); }
            set { SetValue(Value2Property, value); }
        }
        public decimal Result => Operator switch
        {
            "+" => Value1 + Value2,
            "-" => Value1 - Value2,
            "*" => Value1 * Value2,
            "/" => Value2 == 0 ? 0 : Math.Round(Value1 / Value2,2),
            _ => 0m
        };
        public CalculateControl()
        {
            InitializeComponent();
        }
    }
}
