using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace wpfBarGaugeBar.UI.Units
{
    [TemplatePart(Name = "LayoutRoot", Type = typeof(Grid))]
    [TemplatePart(Name = "Pointer", Type = typeof(Path))]
    public class BarGage : ContentControl
    {
        private Grid rootGrid;
        private Path pointer;
        protected Path optimalRangeIndicator;
        protected Path warningRangeIndicator;
        private enum IndicatorType { OptimalIndicator, WarningIndicator }

        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register("CurrentValue", typeof(double), typeof(BarGage),
                new PropertyMetadata(double.MinValue, new PropertyChangedCallback(BarGage.OnCurrentValuePropertyChanged)));    
        public static readonly DependencyProperty MajorDivisionsCountProperty =
          DependencyProperty.Register("MajorDivisionsCount", typeof(double), typeof(BarGage), new PropertyMetadata(null));

        public static readonly DependencyProperty MinorDivisionsCountProperty =
            DependencyProperty.Register("MinorDivisionsCount", typeof(double), typeof(BarGage), new PropertyMetadata(null));
      
        public static readonly DependencyProperty OptimalRangeStartValueProperty =
            DependencyProperty.Register("OptimalRangeStartValue", typeof(double), typeof(BarGage),
           new PropertyMetadata(new PropertyChangedCallback(BarGage.OnRangeIndigatorPropertyChanged)));
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
        
        public static readonly DependencyProperty MinorTickColorProperty =
            DependencyProperty.Register("MinorTickColor", typeof(Color), typeof(BarGage), new PropertyMetadata(null));

        public static readonly DependencyProperty MajorTickColorProperty =
            DependencyProperty.Register("MajorTickColor", typeof(Color), typeof(BarGage), new PropertyMetadata(null));
        public static readonly DependencyProperty ScaleLabelForegroundProperty =
           DependencyProperty.Register("ScaleLabelForeground", typeof(Color), typeof(BarGage), null);

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
        /// <summary>
        /// MajorTick Rectangle Color
        /// </summary>
        public Color MajorTickColor
        {
            get { return (Color)GetValue(MajorTickColorProperty); }
            set { SetValue(MajorTickColorProperty, value); }
        }
        /// <summary>
        /// Minor Rectangle Color
        /// </summary>
        public Color MinorTickColor
        {
            get { return (Color)GetValue(MinorTickColorProperty); }
            set { SetValue(MinorTickColorProperty, value); }
        }
        /// <summary>
        /// Minor Text Color
        /// </summary>
        public Color ScaleLabelForeground
        {
            get { return (Color)GetValue(ScaleLabelForegroundProperty); }
            set { SetValue(ScaleLabelForegroundProperty, value); }
        }
        static BarGage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BarGage), new FrameworkPropertyMetadata(typeof(BarGage)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            rootGrid = GetTemplateChild("LayoutRoot") as Grid;
            pointer = GetTemplateChild("Pointer") as Path;
            optimalRangeIndicator = new Path();
            warningRangeIndicator = new Path();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            DrawScale();
            RefreshScale();
        }
        private static void OnCurrentValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BarGage gauge = d as BarGage;
            gauge.OnCurrentValueChanged(e);
        }

        private void OnCurrentValueChanged(DependencyPropertyChangedEventArgs e)
        {
            double newValue = (double)e.NewValue;
            double oldValue = (double)e.OldValue;

            if (newValue > this.MaxValue)
            {
                newValue = this.MaxValue;
            }
            else if (newValue < this.MinValue)
            {
                newValue = this.MinValue;
            }

            if (oldValue > this.MaxValue)
            {
                oldValue = this.MaxValue;
            }
            else if (oldValue < this.MinValue)
            {
                oldValue = this.MinValue;
            }
            if (pointer != null)
            {
                AnimaterPointer(newValue, oldValue);
            }
        }
   

        private void AnimaterPointer(double newValue, double oldValue)
        {
            if (pointer != null)
            {
                var height = this.ActualHeight;
                var startHeight = height * 0.1;
                double realworldunit =  - (height - (startHeight * 2)) / (MaxValue - MinValue);
                DoubleAnimation animation = new DoubleAnimation();
                animation.From = oldValue* realworldunit;
                animation.To = newValue* realworldunit;

                double animationDuration = Math.Abs(oldValue - newValue) * 3;
                animation.Duration = new Duration(TimeSpan.FromMilliseconds(animationDuration));

                Storyboard sb = new Storyboard();
                
                sb.Children.Add(animation);
                Storyboard.SetTarget(animation, pointer);
                Storyboard.SetTargetProperty(animation, new PropertyPath("(Path.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)"));

                if (newValue != oldValue)
                {
                    sb.Begin();
                }
            }
        }

        private static void OnRangeIndigatorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BarGage gauge = d as BarGage;
            gauge.RefreshScale();
        }

        private void RefreshScale()
        {            
            rootGrid = GetTemplateChild("LayoutRoot") as Grid;
            if (rootGrid != null)
            {
                for (int i = rootGrid.Children.Count - 1; i >= 0; i--)
                {
                    Path path = rootGrid.Children[i] as Path;
                    if ((path == optimalRangeIndicator) || (path == warningRangeIndicator))
                        rootGrid.Children.Remove(path);
                }
                this.DrawRangIndigator();
            }
        }

        private void DrawRangIndigator()
        {
            DrawRangeIndicator(-2, 1, IndicatorType.WarningIndicator);
            DrawRangeIndicator(1, 2, IndicatorType.OptimalIndicator);
        }

        private void DrawRangeIndicator(double startRange, double endRange, IndicatorType indicatorType)
        {
            //if (startRange == 0 && endRange == 0) return;

            var actualWidth = this.ActualWidth;
            var actualHeight = this.ActualHeight;

            Color indigatorColor;

            if (indicatorType == IndicatorType.OptimalIndicator)
            {
                indigatorColor = OptimalRangeColor;
            }
            else
            {
                indigatorColor = AboveOptimalRangeColor;
            }

            //Point A1 = GetCircumferencePoint(0, startRange, true);
            //Point B1 = GetCircumferencePoint(actualWidth, startRange, false);
            //Point C1 = GetCircumferencePoint(0, endRange, true);
            var points = GetCircumferencePoint(startRange, endRange);

            Point A1 = new Point(0, 0);
            Point B1 = new Point(actualWidth, actualHeight * 0.1);
            Point C1 = new Point(actualWidth, actualHeight * 0.9);
            Point D1 = new Point(0, actualHeight);
            //DrawSegment(A1, B1, C1, D1, indigatorColor, indicatorType);
            DrawSegment(points[0], points[1], points[2], points[3], indigatorColor, indicatorType);
        }

        private void DrawSegment(Point p1, Point p2, Point p3, Point p4, Color indigatorColor, IndicatorType indicatorType)
        {
            PathSegmentCollection segments = new PathSegmentCollection();

            segments.Add(new LineSegment() { Point = p1 });
            segments.Add(new LineSegment() { Point = p2 });
            segments.Add(new LineSegment() { Point = p3 });
            segments.Add(new LineSegment() { Point = p4 });
          
            if (indicatorType == IndicatorType.OptimalIndicator)
            {
                optimalRangeIndicator = CreateIndicatorPath(indigatorColor, segments, p1, 0.65);
                optimalRangeIndicator.SetValue(Canvas.ZIndexProperty, 2);
                rootGrid.Children.Add(optimalRangeIndicator);
            }
            else
            {
                warningRangeIndicator = CreateIndicatorPath(indigatorColor, segments, p1, 0.5);

                warningRangeIndicator.SetValue(Canvas.ZIndexProperty, 1);
                rootGrid.Children.Add(warningRangeIndicator);
            }
        }
        private Path CreateIndicatorPath(Color indigatorColor, PathSegmentCollection segments, Point startPoint, double opacity)
        {
            return new Path()
            {
                StrokeLineJoin = PenLineJoin.Round,
                Stroke = new SolidColorBrush(Colors.White),                
                Fill = new SolidColorBrush(indigatorColor),
                Opacity = opacity,
                StrokeThickness = 0.5,
                Data = new PathGeometry()
                {
                    Figures = new PathFigureCollection()
                        {
                            new PathFigure()
                            {
                                IsClosed = true,
                                StartPoint = startPoint,
                                Segments = segments
                            }
                        }
                }
            };
        }
        private Point[] GetCircumferencePoint(double startValue, double endValue)
        {
            Point[] p = new Point[4];
            Point A1, B1, C1, D1;

            var actualWidth = this.ActualWidth;
            var actualHeight = this.ActualHeight;
            var startHeight = this.ActualHeight * 0.1;
            var endHeight = this.ActualHeight * 0.9;
            var middleHeight = this.ActualHeight * 0.5;
            double realworldunit = -(actualHeight - (startHeight * 2)) / (MaxValue - MinValue);
            var startData = (realworldunit * startValue) + middleHeight;
            var endData = (realworldunit * endValue) + middleHeight;

            if (startValue <= MinValue)
            {
                C1 = new Point(actualWidth, endHeight);
                D1 = new Point(0, actualHeight);
            }
            else
            {
                C1 = new Point(actualWidth, startData);
                D1 = new Point(0, startData);
            }       
            if (endValue >= MaxValue)
            {
                A1 = new Point(0, 0);
                B1 = new Point(actualWidth, startHeight);
            }
            else
            {
                A1 = new Point(0, endData);
                B1 = new Point(actualWidth, endData);
            }
            p[0] = A1;  p[1] = B1; p[2] = C1; p[3] = D1;


            return p;
        }

        private void DrawScale()
        {
            DrawMajorTicksRectangle();
        }

        double minvalue;
        private void DrawMajorTicksRectangle()
        {
            minvalue = MinValue;
            var height = this.ActualHeight;
            var acualHeight = height * 0.1;
            if(height <= 0 )
            {
                return;
            }
            double majorTicksUnitValue = (MaxValue - MinValue) / MajorDivisionsCount;
            majorTicksUnitValue = Math.Round(majorTicksUnitValue, ScaleValuePrecision);
            var tick = (height - (acualHeight * 2)) / MajorDivisionsCount;
            for (double i = ((height - acualHeight) ); i >= acualHeight; i = i - tick)
            {
                Rectangle majortickrect = new Rectangle();
                majortickrect.Height = MajorTickSize.Height;
                majortickrect.Width = MajorTickSize.Width;
                majortickrect.Fill = new SolidColorBrush(MajorTickColor);
                majortickrect.HorizontalAlignment = HorizontalAlignment.Left;
                majortickrect.VerticalAlignment = VerticalAlignment.Top;
                majortickrect.SetValue(Canvas.ZIndexProperty, 3);

                TextBlock tb = DrawMajorLabel(majorTicksUnitValue);

                TranslateTransform majorscalevaluett = new TranslateTransform();
                majorscalevaluett.X = 0;
                majorscalevaluett.Y = i;
                TranslateTransform majorscalevaluettext = new TranslateTransform();
                majorscalevaluettext.X = -40;
                majorscalevaluettext.Y = i;
                tb.RenderTransform = majorscalevaluettext;
                majortickrect.RenderTransform = majorscalevaluett;

                
                rootGrid.Children.Add(majortickrect);
                rootGrid.Children.Add(tb);
                DrawMinorTicksRectangle(i, tick);
            }
            

        }
        private TextBlock DrawMajorLabel(double majorTicksUnitValue)
        {
            TextBlock tb = new TextBlock();

            tb.Height = 13;
            tb.Width = 15;
            tb.FontSize = 10;
            tb.Foreground = new SolidColorBrush(ScaleLabelForeground);
            tb.TextAlignment = TextAlignment.Right;
            tb.HorizontalAlignment = HorizontalAlignment.Right;
            tb.VerticalAlignment = VerticalAlignment.Top;
            tb.SetValue(Canvas.ZIndexProperty, 3);
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
        /// <summary>
        /// MinorRectangle을 그리는 함수
        /// </summary>
        /// <param name="scaleStartAngle"></param>
        /// <param name="majorTickUnitAngle"></param>
        private void DrawMinorTicksRectangle(double startYAxis, double resolution)
        {
            double startY = startYAxis - resolution;
            double oneYAxis = resolution / (MinorDivisionsCount +1);
            if (startY < 0) return;
            for (double mi = startY; mi < (startYAxis ); mi = mi + oneYAxis)
            {
                Rectangle mr = new Rectangle();
                mr.Height = MinorTickSize.Height;
                mr.Width = MinorTickSize.Width;
                mr.Fill = new SolidColorBrush(MinorTickColor);
                mr.HorizontalAlignment = HorizontalAlignment.Left;
                mr.VerticalAlignment = VerticalAlignment.Top;
                mr.SetValue(Canvas.ZIndexProperty, 3);

                TranslateTransform minorticktt = new TranslateTransform();
                minorticktt.X = 0;
                minorticktt.Y = mi;                
                mr.RenderTransform = minorticktt;
                rootGrid.Children.Add(mr);
            }
        }       
       
    }
}
