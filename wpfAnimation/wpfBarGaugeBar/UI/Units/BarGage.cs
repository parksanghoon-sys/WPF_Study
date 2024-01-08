using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace wpfBarGaugeBar.UI.Units
{
    [TemplatePart(Name = "LayoutRoot", Type = typeof(Grid))]    
    public class BarGage : ContentControl
    {
        private Grid rootGrid;
        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register("CurrentValue", typeof(double), typeof(BarGage),
                new PropertyMetadata(double.MinValue, new PropertyChangedCallback(BarGage.OnCurrentValuePropertyChanged)));

        private static void OnCurrentValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public static readonly DependencyProperty MajorDivisionsCountProperty =
          DependencyProperty.Register("MajorDivisionsCount", typeof(double), typeof(BarGage), new PropertyMetadata(null));

        public static readonly DependencyProperty MinorDivisionsCountProperty =
            DependencyProperty.Register("MinorDivisionsCount", typeof(double), typeof(BarGage), new PropertyMetadata(null));
        public static readonly DependencyProperty OptimalRangeStartValueProperty =
           DependencyProperty.Register("OptimalRangeStartValue", typeof(double), typeof(BarGage),
               new PropertyMetadata(new PropertyChangedCallback(BarGage.OnRangeIndigatorPropertyChanged)));

        private static void OnRangeIndigatorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public static readonly DependencyProperty OptimalRangeEndValueProperty =
            DependencyProperty.Register("OptimalRangeEndValue", typeof(double), typeof(BarGage),
                new PropertyMetadata(new PropertyChangedCallback(BarGage.OnRangeIndigatorPropertyChanged)));

        public static readonly DependencyProperty WarningRangeStartValueProperty =
            DependencyProperty.Register("WarningRangeStartValue", typeof(double), typeof(BarGage),
                new PropertyMetadata(new PropertyChangedCallback(BarGage.OnRangeIndigatorPropertyChanged)));

        public static readonly DependencyProperty WarningRangeEndValueProperty =
            DependencyProperty.Register("WarningRangeEndValue", typeof(double), typeof(BarGage),
                new PropertyMetadata(new PropertyChangedCallback(BarGage.OnRangeIndigatorPropertyChanged)));

        public static readonly DependencyProperty MajorTickSizeProperty =
            DependencyProperty.Register("MajorTickSize", typeof(Size), typeof(BarGage), null);

        public static readonly DependencyProperty MinorTickSizeProperty =
            DependencyProperty.Register("MinorTickSize", typeof(Size), typeof(BarGage), null);
        public static readonly DependencyProperty ScaleValuePrecisionProperty =
           DependencyProperty.Register("ScaleValuePrecision", typeof(int), typeof(BarGage), null);
        public static readonly DependencyProperty MinValueProperty =
           DependencyProperty.Register("MinValue", typeof(double), typeof(BarGage), new PropertyMetadata(null));

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(BarGage), new PropertyMetadata(null));
        public static readonly DependencyProperty OptimalRangeColorProperty =
            DependencyProperty.Register("OptimalRangeColor", typeof(Color), typeof(BarGage), null);

        public static readonly DependencyProperty AboveOptimalRangeColorProperty =
            DependencyProperty.Register("AboveOptimalRangeColor", typeof(Color), typeof(BarGage), null);
        /// <summary>
        /// Gage Min 값
        /// </summary>
        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }
        /// <summary>
        /// Gauage Max 값
        /// </summary>
        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }
        /// <summary>
        /// 현재 Gauge값 
        /// </summary>
        public double CurrentValue
        {
            get { return (double)GetValue(CurrentValueProperty); }
            set { SetValue(CurrentValueProperty, value); }
        }
        /// <summary>
        /// Green 게이지 끝값
        /// </summary>
        public double OptimalRangeEndValue
        {
            get { return (double)GetValue(OptimalRangeEndValueProperty); }
            set { SetValue(OptimalRangeEndValueProperty, value); }
        }
        /// <summary>
        /// Green 게이지 시작 값
        /// </summary>
        public double OptimalRangeStartValue
        {
            get { return (double)GetValue(OptimalRangeStartValueProperty); }
            set { SetValue(OptimalRangeStartValueProperty, value); }
        }
        /// <summary>
        /// 위험 게이지 시작값
        /// </summary>
        public double WarningRangeEndValue
        {
            get { return (double)GetValue(WarningRangeEndValueProperty); }
            set { SetValue(WarningRangeEndValueProperty, value); }
        }
        /// <summary>
        /// 위험 게이지 끝값
        /// </summary>
        public double WarningRangeStartValue
        {
            get { return (double)GetValue(WarningRangeStartValueProperty); }
            set { SetValue(WarningRangeStartValueProperty, value); }
        }
        /// <summary>
        /// 게이지 메인 라인 인디게이터 총 갯수 
        /// </summary>
        public double MajorDivisionsCount
        {
            get { return (double)GetValue(MajorDivisionsCountProperty); }
            set { SetValue(MajorDivisionsCountProperty, value); }
        }
        /// <summary>
        /// 게이지 서브 라인 인디게이터 총 갯수 
        /// </summary>
        public double MinorDivisionsCount
        {
            get { return (double)GetValue(MinorDivisionsCountProperty); }
            set { SetValue(MinorDivisionsCountProperty, value); }
        }
        /// <summary>
        /// 게이지 큰 라인 구분자 크기
        /// </summary>
        public Size MajorTickSize
        {
            get { return (Size)GetValue(MajorTickSizeProperty); }
            set { SetValue(MajorTickSizeProperty, value); }
        }
        /// <summary>
        /// 게이지 작은 라인 구분자 크기
        /// </summary>
        public Size MinorTickSize
        {
            get { return (Size)GetValue(MinorTickSizeProperty); }
            set { SetValue(MinorTickSizeProperty, value); }
        }
        /// <summary>
        /// Gauge 분해능
        /// </summary>
        public int ScaleValuePrecision
        {
            get { return (int)GetValue(ScaleValuePrecisionProperty); }
            set { SetValue(ScaleValuePrecisionProperty, value); }
        }
        /// <summary>
        /// 안전 게이지 바 색상
        /// </summary>
        public Color OptimalRangeColor
        {
            get { return (Color)GetValue(OptimalRangeColorProperty); }
            set { SetValue(OptimalRangeColorProperty, value); }
        }
        /// <summary>
        /// 위험 게이지 바 색상
        /// </summary>
        public Color AboveOptimalRangeColor
        {
            get { return (Color)GetValue(AboveOptimalRangeColorProperty); }
            set { SetValue(AboveOptimalRangeColorProperty, value); }
        }
        static BarGage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BarGage), new FrameworkPropertyMetadata(typeof(BarGage)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            rootGrid = GetTemplateChild("LayoutRoot") as Grid;
            DrawScale();
        }

        private void DrawScale()
        {
            //DrawMajorTicksRectangle();
        }

        double minvalue;
        private void DrawMajorTicksRectangle()
        {
            minvalue = MinValue;
            var height = this.ActualHeight;
            if(height <= 0 )
            {
                return;
            }
            double majorTicksUnitValue = (MaxValue - MinValue) / MajorDivisionsCount;
            majorTicksUnitValue = Math.Round(majorTicksUnitValue, ScaleValuePrecision);
            var tick = height / MajorDivisionsCount;
            for (double i = height; i >= 0; i = i - tick)
            {
                Rectangle majortickrect = new Rectangle();
                majortickrect.Height = MajorTickSize.Height;
                majortickrect.Width = MajorTickSize.Width;
                majortickrect.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                majortickrect.HorizontalAlignment = HorizontalAlignment.Left;
                majortickrect.VerticalAlignment = VerticalAlignment.Top;


                TextBlock tb = DrawMajorLabel(majorTicksUnitValue);

                TranslateTransform majorscalevaluett = new TranslateTransform();
                majorscalevaluett.X = 0;
                majorscalevaluett.Y = i;
                tb.RenderTransform = majorscalevaluett;
                majortickrect.RenderTransform = majorscalevaluett;

                rootGrid.Children.Add(majortickrect);
                rootGrid.Children.Add(tb);
            }
            

        }

        private TextBlock DrawMajorLabel(double majorTicksUnitValue)
        {
            TextBlock tb = new TextBlock();

            tb.Height = 10;
            tb.Width = 30;
            tb.FontSize = 10;
            tb.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            tb.TextAlignment = TextAlignment.Right;
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.VerticalAlignment = VerticalAlignment.Top;

            if (Math.Round(minvalue, ScaleValuePrecision) <= Math.Round(MaxValue, ScaleValuePrecision))
            {
                minvalue = Math.Round(minvalue, ScaleValuePrecision);
                var text = minvalue.ToString();
                tb.Text = text;
                minvalue = minvalue + majorTicksUnitValue;
                return tb;
            }
            else
            {
                return null;
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            DrawMajorTicksRectangle();
        }
       
    }
}
