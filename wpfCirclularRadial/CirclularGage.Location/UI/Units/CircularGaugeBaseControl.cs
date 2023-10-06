using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CirclularGage.Location.UI.Units
{
    [TemplatePart(Name = "LayoutRoot", Type = typeof(Grid))]
    [TemplatePart(Name = "Pointer", Type = typeof(Path))]
    public abstract class CircularGaugeBaseControl : ContentControl
    {
        protected enum IndicatorType { OptimalIndicator, WarningIndicator }
        #region Private 변수
        /// <summary>
        /// GauageControl Rander 하는데 필요한 Variables
        /// </summary>
        protected Grid rootGrid;
        protected Path optimalRangeIndicator;
        protected Path warningRangeIndicator;
        protected Path pointer;
        protected double minvalue;
        
        protected bool isInitialValueSet = false;
        protected double arcradius1;
        protected double arcradius2;
        private int animatingSpeedFactor = 6;
        #endregion
        #region Dependency Properties

        public static readonly DependencyProperty GaugeBackgroundColorProperty =
            DependencyProperty.Register("GaugeBackgroundColor", typeof(Color), typeof(CircularGaugeBaseControl), null);

        public static readonly DependencyProperty GaugePointColorProperty =
            DependencyProperty.Register("GaugePointColor", typeof(Color), typeof(CircularGaugeBaseControl), null);

        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register("CurrentValue", typeof(double), typeof(CircularGaugeBaseControl),
                new PropertyMetadata(double.MinValue, new PropertyChangedCallback(CircularGaugeBaseControl.OnCurrentValuePropertyChanged)));

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(CircularGaugeBaseControl), new PropertyMetadata(null));

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(CircularGaugeBaseControl), new PropertyMetadata(null));

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(CircularGaugeBaseControl), new PropertyMetadata(null));
        public static readonly DependencyProperty PointerLengthProperty =
            DependencyProperty.Register("PointerLength", typeof(double), typeof(CircularGaugeBaseControl), new PropertyMetadata(null));

        public static readonly DependencyProperty PointerThicknessProperty =
            DependencyProperty.Register("PointerThickness", typeof(double), typeof(CircularGaugeBaseControl), null);

        public static readonly DependencyProperty ScaleRadiusProperty =
            DependencyProperty.Register("ScaleRadius", typeof(double), typeof(CircularGaugeBaseControl), null);

        public static readonly DependencyProperty ScaleStartAngleProperty =
            DependencyProperty.Register("ScaleStartAngle", typeof(double), typeof(CircularGaugeBaseControl), null);

        public static readonly DependencyProperty ScaleSweepAngleProperty =
            DependencyProperty.Register("ScaleSweepAngle", typeof(double), typeof(CircularGaugeBaseControl), null);

        public static readonly DependencyProperty MajorDivisionsCountProperty =
            DependencyProperty.Register("MajorDivisionsCount", typeof(double), typeof(CircularGaugeBaseControl), new PropertyMetadata(null));

        public static readonly DependencyProperty MinorDivisionsCountProperty =
            DependencyProperty.Register("MinorDivisionsCount", typeof(double), typeof(CircularGaugeBaseControl), new PropertyMetadata(null));

        public static readonly DependencyProperty OptimalRangeStartValueProperty =
           DependencyProperty.Register("OptimalRangeStartValue", typeof(double), typeof(CircularGaugeBaseControl),
               new PropertyMetadata(new PropertyChangedCallback(CircularGaugeBaseControl.OnOptimalRangeStartValuePropertyChanged)));

        public static readonly DependencyProperty OptimalRangeEndValueProperty =
            DependencyProperty.Register("OptimalRangeEndValue", typeof(double), typeof(CircularGaugeBaseControl),
       new PropertyMetadata(new PropertyChangedCallback(CircularGaugeBaseControl.OnOptimalRangeEndValuePropertyChanged)));

        public static readonly DependencyProperty WarningRangeStartValueProperty =
            DependencyProperty.Register("WarningRangeStartValue", typeof(double), typeof(CircularGaugeBaseControl),
                new PropertyMetadata(new PropertyChangedCallback(CircularGaugeBaseControl.OnWarningRangeStartValuePropertyChanged)));

        public static readonly DependencyProperty WarningRangeEndValueProperty =
            DependencyProperty.Register("WarningRangeEndValue", typeof(double), typeof(CircularGaugeBaseControl),
                new PropertyMetadata(new PropertyChangedCallback(CircularGaugeBaseControl.OnWarningRangeEndValuePropertyChanged)));

        public static readonly DependencyProperty RangeIndicatorRadiusProperty =
            DependencyProperty.Register("RangeIndicatorRadius", typeof(double), typeof(CircularGaugeBaseControl), null);

        public static readonly DependencyProperty RangeIndicatorThicknessProperty =
            DependencyProperty.Register("RangeIndicatorThickness", typeof(double), typeof(CircularGaugeBaseControl), null);

        public static readonly DependencyProperty ScaleLabelRadiusProperty =
            DependencyProperty.Register("ScaleLabelRadius", typeof(double), typeof(CircularGaugeBaseControl), new PropertyMetadata(null));

        public static readonly DependencyProperty ScaleLabelSizeProperty =
            DependencyProperty.Register("ScaleLabelSize", typeof(Size), typeof(CircularGaugeBaseControl), new PropertyMetadata(null));

        public static readonly DependencyProperty ScaleLabelFontSizeProperty =
            DependencyProperty.Register("ScaleLabelFontSize", typeof(double), typeof(CircularGaugeBaseControl), null);

        public static readonly DependencyProperty ScaleLabelForegroundProperty =
            DependencyProperty.Register("ScaleLabelForeground", typeof(Color), typeof(CircularGaugeBaseControl), null);

        public static readonly DependencyProperty MajorTickSizeProperty =
       DependencyProperty.Register("MajorTickSize", typeof(Size), typeof(CircularGaugeBaseControl), null);

        public static readonly DependencyProperty MinorTickSizeProperty =
          DependencyProperty.Register("MinorTickSize", typeof(Size), typeof(CircularGaugeBaseControl), null);

        public static readonly DependencyProperty MajorTickColorProperty =
           DependencyProperty.Register("MajorTickColor", typeof(Color), typeof(CircularGaugeBaseControl), null);

        public static readonly DependencyProperty MinorTickColorProperty =
          DependencyProperty.Register("MinorTickColor", typeof(Color), typeof(CircularGaugeBaseControl), null);

        public static readonly DependencyProperty OptimalRangeColorProperty =
            DependencyProperty.Register("OptimalRangeColor", typeof(Color), typeof(CircularGaugeBaseControl), null);

        public static readonly DependencyProperty AboveOptimalRangeColorProperty =
            DependencyProperty.Register("AboveOptimalRangeColor", typeof(Color), typeof(CircularGaugeBaseControl), null);

        public static readonly DependencyProperty ScaleValuePrecisionProperty =
            DependencyProperty.Register("ScaleValuePrecision", typeof(int), typeof(CircularGaugeBaseControl), null);

        public static readonly DependencyProperty ResetPointerOnStartUpProperty =
            DependencyProperty.Register("ResetPointerOnStartUp", typeof(bool), typeof(CircularGaugeBaseControl), new PropertyMetadata(false, null));
        #endregion

        #region Dependcy Variables
        /// <summary>
        /// Gauge 배경색
        /// </summary>
        public Color GaugeBackgroundColor
        {
            get { return (Color)GetValue(GaugeBackgroundColorProperty); }
            set { SetValue(GaugeBackgroundColorProperty, value); }
        }
        /// <summary>
        /// Gauge 배경색
        /// </summary>
        public Color GaugePointColor
        {
            get { return (Color)GetValue(GaugePointColorProperty); }
            set { SetValue(GaugePointColorProperty, value); }
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
        /// Guasge 반지름 값
        /// </summary>
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        /// <summary>
        /// Guage Point 두께
        /// </summary>
        public double PointerThickness
        {
            get { return (double)GetValue(PointerThicknessProperty); }
            set { SetValue(PointerThicknessProperty, value); }
        }
        /// <summary>
        /// Guage Point Length
        /// </summary>
        public double PointerLength
        {
            get { return (double)GetValue(PointerLengthProperty); }
            set { SetValue(PointerLengthProperty, value); }
        }
        /// <summary>
        /// Gauge 라인 표시 반지름
        /// </summary>
        public double ScaleRadius
        {
            get { return (double)GetValue(ScaleRadiusProperty); }
            set { SetValue(ScaleRadiusProperty, value); }
        }
        /// <summary>
        /// Gauge 표시 각도 (최대 360도 )
        /// </summary>
        public double ScaleSweepAngle
        {
            get { return (double)GetValue(ScaleSweepAngleProperty); }
            set { SetValue(ScaleSweepAngleProperty, value); }
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
        /// 게이지 시작 각도
        /// </summary>
        public double ScaleStartAngle
        {
            get { return (double)GetValue(ScaleStartAngleProperty); }
            set { SetValue(ScaleStartAngleProperty, value); }
        }
        /// <summary>
        /// 게이지 라벨 반지름
        /// </summary>
        public double ScaleLabelRadius
        {
            get { return (double)GetValue(ScaleLabelRadiusProperty); }
            set { SetValue(ScaleLabelRadiusProperty, value); }
        }
        /// <summary>
        /// 게이지 라벨 위치
        /// </summary>
        public Size ScaleLabelSize
        {
            get { return (Size)GetValue(ScaleLabelSizeProperty); }
            set { SetValue(ScaleLabelSizeProperty, value); }
        }
        /// <summary>
        /// 게이지 라벨 글자 크기
        /// </summary>
        public double ScaleLabelFontSize
        {
            get { return (double)GetValue(ScaleLabelFontSizeProperty); }
            set { SetValue(ScaleLabelFontSizeProperty, value); }
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
        /// Guage Indicator 반지름 값
        /// </summary>
        public double RangeIndicatorRadius
        {
            get { return (double)GetValue(RangeIndicatorRadiusProperty); }
            set { SetValue(RangeIndicatorRadiusProperty, value); }
        }
        /// <summary>
        /// Guage Indicator 두께
        /// </summary>
        public double RangeIndicatorThickness
        {
            get { return (double)GetValue(RangeIndicatorThicknessProperty); }
            set { SetValue(RangeIndicatorThicknessProperty, value); }
        }
        /// <summary>
        /// 게이지 라벨 글자 색상
        /// </summary>
        public Color ScaleLabelForeground
        {
            get { return (Color)GetValue(ScaleLabelForegroundProperty); }
            set { SetValue(ScaleLabelForegroundProperty, value); }
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
        /// 게이지 큰 라인 구분자 색상
        /// </summary>
        public Color MajorTickColor
        {
            get { return (Color)GetValue(MajorTickColorProperty); }
            set { SetValue(MajorTickColorProperty, value); }
        }
        /// <summary>
        /// 게이지 작은 라인 구분자 색상
        /// </summary>
        public Color MinorTickColor
        {
            get { return (Color)GetValue(MinorTickColorProperty); }
            set { SetValue(MinorTickColorProperty, value); }
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
        /// Gauge 분해능
        /// </summary>
        public int ScaleValuePrecision
        {
            get { return (int)GetValue(ScaleValuePrecisionProperty); }
            set { SetValue(ScaleValuePrecisionProperty, value); }
        }
        /// <summary>
        /// Gauge 초기 생성 시 원점 여부 불필요?
        /// </summary>
        public bool ResetPointerOnStartUp
        {
            get { return (bool)GetValue(ResetPointerOnStartUpProperty); }
            set { SetValue(ResetPointerOnStartUpProperty, value); }
        }
        #endregion
        static CircularGaugeBaseControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CircularGaugeBaseControl), new FrameworkPropertyMetadata(typeof(CircularGaugeBaseControl)));
        }
        public CircularGaugeBaseControl()
        {

        }
        #region Draw
        /// <summary>
        /// Control 생성시 만들어주는 Override
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            // View의 Property 가져오기
            rootGrid = GetTemplateChild("LayoutRoot") as Grid;
            pointer = GetTemplateChild("Pointer") as Path;

            DrawScale();
            DrawRangeIndicator(OptimalRangeStartValue, OptimalRangeEndValue, IndicatorType.OptimalIndicator);
            DrawRangeIndicator(WarningRangeStartValue, WarningRangeEndValue, IndicatorType.WarningIndicator);

            Canvas.SetZIndex(pointer, 100000);
            if (ResetPointerOnStartUp)
                MovePointer(ScaleStartAngle);
        }
        /// <summary>
        /// Indicator 생성 (게이지 바)
        /// </summary>
        protected virtual void DrawRangeIndicator(double startRange, double endRange, IndicatorType indicatorType)
        {
            if (startRange == 0 && endRange == 0) return;
            double realworldunit = (ScaleSweepAngle / (MaxValue - MinValue));
            double startAngle;
            double endAngle;
            double db;

            if (startRange < 0)
            {
                db = MinValue + Math.Abs(startRange);
                startAngle = ((double)(Math.Abs(db * realworldunit)));
            }
            else
            {
                db = Math.Abs(MinValue) + startRange;
                startAngle = ((double)(db * realworldunit));
            }
            startAngle = CalculationStartAngel(realworldunit, startRange);

            endAngle = CalculationEndAngel(realworldunit, endRange);

            double startAngleFromStart = (ScaleStartAngle + startAngle);

            double endAngleFromStart = (ScaleStartAngle + endAngle);

            arcradius1 = (RangeIndicatorRadius + RangeIndicatorThickness);
            arcradius2 = RangeIndicatorRadius;

            double rangeEndAngle = ScaleStartAngle + ScaleSweepAngle;

            Color indigatorColor;

            if (indicatorType == IndicatorType.OptimalIndicator)
            {
                indigatorColor = OptimalRangeColor;
            }
            else
            {
                indigatorColor = AboveOptimalRangeColor;
            }
            Point A1 = GetCircumferencePoint(startAngleFromStart, arcradius1);
            Point B1 = GetCircumferencePoint(startAngleFromStart, arcradius2);
            Point C1 = GetCircumferencePoint(endAngleFromStart, arcradius2);
            Point D1 = GetCircumferencePoint(endAngleFromStart, arcradius1);
            bool isReflexAngle1 = Math.Abs(endAngleFromStart - startAngleFromStart) > 180.0;
            DrawSegment(A1, B1, C1, D1, isReflexAngle1, indigatorColor, indicatorType);
        }


        /// <summary>
        /// Indigate 좌표를 계산해주는 함수
        /// </summary>
        /// <param name="angle">시작 각도</param>
        /// <param name="radius"></param>
        /// <returns></returns>
        protected Point GetCircumferencePoint(double angle, double radius)
        {
            double angle_radian = (angle * Math.PI) / 180;
            //Radius-- is the Radius of the gauge
            double X = (double)((Radius) + (radius) * Math.Cos(angle_radian));
            double Y = (double)((Radius) + (radius) * Math.Sin(angle_radian));
            Point p = new Point(X, Y);
            return p;
        }
        /// <summary>
        /// IndigatorType에 따라 경고 혹은 안전 경고 Indigator Circle 생성
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="p4"></param>
        /// <param name="reflexangle">역 원형 여부</param>
        /// <param name="clr">칠할 색상</param>
        /// <param name="indicatorType">Indigator 타입</param>
        protected void DrawSegment(Point p1, Point p2, Point p3, Point p4, bool reflexangle, Color clr, IndicatorType indicatorType)
        {
            if (rootGrid is null)
                return;
            switch (indicatorType)
            {
                case IndicatorType.OptimalIndicator:
                    {
                        if((optimalRangeIndicator != null)&& (rootGrid.Children.Contains(optimalRangeIndicator)))
                            rootGrid.Children.Remove(optimalRangeIndicator);
                    }
                    
                    break;
                case IndicatorType.WarningIndicator:
                    {
                        if((warningRangeIndicator != null) &&(rootGrid.Children.Contains(warningRangeIndicator)))
                            rootGrid.Children.Remove(warningRangeIndicator);                        
                    }                    
                    break;
            }

            PathSegmentCollection segments = new PathSegmentCollection();

            segments.Add(new LineSegment() { Point = p2 });

            segments.Add(new ArcSegment()
            {
                Size = new Size(arcradius2, arcradius2),
                Point = p3,
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = reflexangle

            });

            segments.Add(new LineSegment() { Point = p4 });

            segments.Add(new ArcSegment()
            {
                Size = new Size(arcradius1, arcradius1),
                Point = p1,
                SweepDirection = SweepDirection.Counterclockwise,
                IsLargeArc = reflexangle

            });

            Color rangestrokecolor;
            if (clr == Colors.Transparent)
            {
                rangestrokecolor = clr;
            }
            else
                rangestrokecolor = Colors.White;


            if (indicatorType == IndicatorType.OptimalIndicator)
            {
                optimalRangeIndicator = new Path()
                {
                    StrokeLineJoin = PenLineJoin.Round,
                    Stroke = new SolidColorBrush(rangestrokecolor),
                    Fill = new SolidColorBrush(clr),
                    Opacity = 0.65,
                    StrokeThickness = 0.25,
                    Data = new PathGeometry()
                    {
                        Figures = new PathFigureCollection()
                     {
                        new PathFigure()
                        {
                            IsClosed = true,
                            StartPoint = p1,
                            Segments = segments
                        }
                    }
                    }
                };

                optimalRangeIndicator.SetValue(Canvas.ZIndexProperty, 150);
                rootGrid.Children.Add(optimalRangeIndicator);
            }
            else
            {
                warningRangeIndicator = new Path()
                {
                    StrokeLineJoin = PenLineJoin.Round,
                    Stroke = new SolidColorBrush(rangestrokecolor),
                    //Color.FromArgb(0xFF, 0xF5, 0x9A, 0x86)
                    Fill = new SolidColorBrush(clr),
                    Opacity = 0.65,
                    StrokeThickness = 0.25,
                    Data = new PathGeometry()
                    {
                        Figures = new PathFigureCollection()
                     {
                        new PathFigure()
                        {
                            IsClosed = true,
                            StartPoint = p1,
                            Segments = segments
                        }
                    }
                    }
                };

                warningRangeIndicator.SetValue(Canvas.ZIndexProperty, 150);
                rootGrid.Children.Add(warningRangeIndicator);
            }

        }
        #endregion
        /// <summary>
        /// 현재 포인터가 데이터가 변경시 Event
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnCurrentValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CircularGaugeBaseControl gauge = d as CircularGaugeBaseControl;
            gauge.OnCurrentValueChanged(e);
            //gauge.DrawScale();                       
        }
        /// <summary>
        /// 게이지바 안전 상태 시작 설정부분 제한
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>

        private static void OnOptimalRangeStartValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CircularGaugeBaseControl gauge = d as CircularGaugeBaseControl;
            if ((double)e.NewValue < gauge.MinValue)
                gauge.OptimalRangeStartValue = gauge.MinValue;

        }
        /// <summary>
        /// 게이지바 안전 상태 끝 설정부분 제한
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnOptimalRangeEndValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CircularGaugeBaseControl gauge = d as CircularGaugeBaseControl;
            if ((double)e.NewValue > gauge.MaxValue)
                gauge.OptimalRangeEndValue = gauge.MaxValue;
            gauge.DrawRangeIndicator(gauge.OptimalRangeStartValue, gauge.OptimalRangeEndValue, IndicatorType.OptimalIndicator);
            //gauge.DrawOptimalRankkgeIndicator();
        }
        /// <summary>
        /// 위험 게이지 시작 설정부분 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnWarningRangeStartValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CircularGaugeBaseControl gauge = d as CircularGaugeBaseControl;
            if ((double)e.NewValue < gauge.MinValue)
                gauge.OptimalRangeStartValue = gauge.MinValue;

        }
        /// <summary>
        /// 위험 게이지 끝 설정 부분
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnWarningRangeEndValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CircularGaugeBaseControl gauge = d as CircularGaugeBaseControl;
            if ((double)e.NewValue > gauge.MaxValue)
                gauge.OptimalRangeEndValue = gauge.MaxValue;
            gauge.DrawRangeIndicator(gauge.WarningRangeStartValue, gauge.WarningRangeEndValue, IndicatorType.WarningIndicator);
            //gauge.DrawWarningRangeIndicator();
        }
        /// <summary>
        /// Gauge 현재 상태 ChagneEvent
        /// </summary>
        /// <param name="e"></param>
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
                double oldcurr_realworldunit = 0;
                double newcurr_realworldunit = 0;
                double realworldunit = (ScaleSweepAngle / (MaxValue - MinValue));

                if (oldValue == MinValue && !isInitialValueSet)
                {
                    oldValue = MinValue;
                    isInitialValueSet = true;
                }

                oldcurr_realworldunit = CalculationStartAngel(realworldunit, oldValue);
                newcurr_realworldunit = CalculationEndAngel(realworldunit, newValue);

                double oldCurrentValueAngle = ScaleStartAngle + oldcurr_realworldunit;
                double newCurrentValueAngle = ScaleStartAngle + newcurr_realworldunit;

                AnimaterPointer(oldCurrentValueAngle, newCurrentValueAngle);
            }

        }
        /// <summary>
        /// MajorRectengle을 그리는 함수 내부에 Lable 과 MinorRect그리는 함수가 포함
        /// </summary>
        /// <param name="majorTickUnitAngle"></param>
        protected virtual void DrawMajorTicksRectangle(double majorTickUnitAngle)
        {
            minvalue = MinValue;
            double majorTicksUnitValue = (MaxValue - MinValue) / MajorDivisionsCount;
            majorTicksUnitValue = Math.Round(majorTicksUnitValue, ScaleValuePrecision);

            for (double i = ScaleStartAngle; i <= (ScaleStartAngle + ScaleSweepAngle); i = i + majorTickUnitAngle)
            {
                Rectangle majortickrect = new Rectangle();
                majortickrect.Height = MajorTickSize.Height;
                majortickrect.Width = MajorTickSize.Width;
                majortickrect.Fill = new SolidColorBrush(MajorTickColor);
                Point p = new Point(0.5, 0.5);
                majortickrect.RenderTransformOrigin = p;
                majortickrect.HorizontalAlignment = HorizontalAlignment.Center;
                majortickrect.VerticalAlignment = VerticalAlignment.Center;

                TransformGroup majortickgp = new TransformGroup();
                RotateTransform majortickrt = new RotateTransform();

                double i_radian = (i * Math.PI) / 180;
                majortickrt.Angle = i;
                majortickgp.Children.Add(majortickrt);
                TranslateTransform majorticktt = new TranslateTransform();

                majorticktt.X = (int)((ScaleRadius) * Math.Cos(i_radian));
                majorticktt.Y = (int)((ScaleRadius) * Math.Sin(i_radian));

                TranslateTransform majorscalevaluett = new TranslateTransform();
                majorscalevaluett.X = (int)((ScaleLabelRadius) * Math.Cos(i_radian));
                majorscalevaluett.Y = (int)((ScaleLabelRadius) * Math.Sin(i_radian));

                TextBlock tb = DrawMajorLabel(majorTicksUnitValue);

                if (tb is null)
                    break;
                majortickgp.Children.Add(majorticktt);
                majortickrect.RenderTransform = majortickgp;
                tb.RenderTransform = majorscalevaluett;
                rootGrid.Children.Add(majortickrect);
                rootGrid.Children.Add(tb);

                DrawMinorTicksRectangle(i, majorTickUnitAngle);
            }
        }
        /// <summary>
        /// MinorRectangle을 그리는 함수
        /// </summary>
        /// <param name="scaleStartAngle"></param>
        /// <param name="majorTickUnitAngle"></param>
        protected virtual void DrawMinorTicksRectangle(double scaleStartAngle, double majorTickUnitAngle)
        {
            //Drawing the minor axis ticks
            double onedegree = ((scaleStartAngle + majorTickUnitAngle) - scaleStartAngle) / (MinorDivisionsCount);

            if ((scaleStartAngle < (ScaleStartAngle + ScaleSweepAngle)) && (Math.Round(minvalue, ScaleValuePrecision) <= Math.Round(MaxValue, ScaleValuePrecision)))
            {
                for (double mi = scaleStartAngle + onedegree; mi < (scaleStartAngle + majorTickUnitAngle); mi = mi + onedegree)
                {
                    Rectangle mr = new Rectangle();
                    mr.Height = MinorTickSize.Height;
                    mr.Width = MinorTickSize.Width;
                    mr.Fill = new SolidColorBrush(MinorTickColor);
                    mr.HorizontalAlignment = HorizontalAlignment.Center;
                    mr.VerticalAlignment = VerticalAlignment.Center;
                    Point p1 = new Point(0.5, 0.5);
                    mr.RenderTransformOrigin = p1;

                    TransformGroup minortickgp = new TransformGroup();
                    RotateTransform minortickrt = new RotateTransform();
                    minortickrt.Angle = mi;
                    minortickgp.Children.Add(minortickrt);
                    TranslateTransform minorticktt = new TranslateTransform();

                    double mi_radian = (mi * Math.PI) / 180;
                    minorticktt.X = (int)((ScaleRadius) * Math.Cos(mi_radian));
                    minorticktt.Y = (int)((ScaleRadius) * Math.Sin(mi_radian));

                    minortickgp.Children.Add(minorticktt);
                    mr.RenderTransform = minortickgp;
                    rootGrid.Children.Add(mr);

                }

            }
        }
        /// <summary>
        /// Gauge Label을 그리는 Method
        /// </summary>
        /// <param name="majorTicksUnitValue">이전 값에서 추가될 값</param>
        /// <returns></returns>
        protected virtual TextBlock DrawMajorLabel(double majorTicksUnitValue)
        {
            // Indigate Label TextBlock                
            TextBlock tb = new TextBlock();

            tb.Height = ScaleLabelSize.Height;
            tb.Width = ScaleLabelSize.Width;
            tb.FontSize = ScaleLabelFontSize;
            tb.Foreground = new SolidColorBrush(ScaleLabelForeground);
            tb.TextAlignment = TextAlignment.Center;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.HorizontalAlignment = HorizontalAlignment.Center;

            if (Math.Round(minvalue, ScaleValuePrecision) <= Math.Round(MaxValue, ScaleValuePrecision))
            {
                minvalue = Math.Round(minvalue, ScaleValuePrecision);
                tb.Text = Math.Abs(minvalue).ToString();
                minvalue = minvalue + majorTicksUnitValue;
                return tb;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Pointer, Indigate 목표 각도 계산
        /// </summary>
        /// <param name="realworldunit">1도당 각도</param>
        /// <param name="startRange">시작 각도</param>
        /// <returns></returns>
        protected virtual double CalculationStartAngel(double realworldunit, double startRange)
        {
            double db;
            double startAngle;
            if (startRange < 0)
            {
                db = MinValue + Math.Abs(startRange);
                startAngle = ((double)(Math.Abs(db * realworldunit)));
            }
            else
            {
                db = Math.Abs(MinValue) + startRange;
                startAngle = ((double)(db * realworldunit));
            }
            return startAngle;
        }
        /// <summary>
        /// Pointer, Indigate 목표 각도 계산
        /// </summary>
        /// <param name="realworldunit">1도당 각도</param>
        /// <param name="endRange">목표 각도</param>
        /// <returns></returns>
        protected virtual double CalculationEndAngel(double realworldunit, double endRange)
        {
            double db;
            double endAngle;
            if (endRange < 0)
            {
                db = MinValue + Math.Abs(endRange);
                endAngle = ((double)(Math.Abs(db * realworldunit)));
            }
            else
            {
                db = Math.Abs(MinValue) + endRange;
                endAngle = ((double)(db * realworldunit));
            }
            return endAngle;
        }

        /// <summary>
        /// Gauge Indigate 생성
        /// </summary>
        private void DrawScale()
        {
            double majorTickUnitAngle = ScaleSweepAngle / MajorDivisionsCount;
            DrawMajorTicksRectangle(majorTickUnitAngle);
        }

        /// <summary>
        /// Gauge Pointer 이동 Animation
        /// </summary>
        /// <param name="oldCurrentValueAngle">이전 각도</param>
        /// <param name="newCurrentValueAngle">이동할 각도</param>
        /// <exception cref="NotImplementedException"></exception>
        private void AnimaterPointer(double oldCurrentValueAngle, double newCurrentValueAngle)
        {
            if (pointer != null)
            {
                DoubleAnimation animation = new DoubleAnimation();
                animation.From = oldCurrentValueAngle;
                animation.To = newCurrentValueAngle;

                double animationDuration = Math.Abs(oldCurrentValueAngle - newCurrentValueAngle) * animatingSpeedFactor;
                animation.Duration = new Duration(TimeSpan.FromMilliseconds(animationDuration));

                Storyboard sb = new Storyboard();
                //sb.Completed += new EventHandler(AnimaitonCompleted); // Aniamation 종료 이벤트
                sb.Children.Add(animation);
                Storyboard.SetTarget(animation, pointer);
                Storyboard.SetTargetProperty(animation, new PropertyPath("(Path.RenderTransform).(TransformGroup.Children)[0].(RotateTransform.Angle)"));

                if (newCurrentValueAngle != oldCurrentValueAngle)
                {
                    sb.Begin();
                }
            }
        }
        /// <summary>
        /// 초기 Pointer를 0으로 세팅
        /// </summary>
        /// <param name="angleValue"></param>
        private void MovePointer(double angleValue)
        {
            if (pointer != null)
            {
                TransformGroup tg = pointer.RenderTransform as TransformGroup;
                RotateTransform rt = tg.Children[0] as RotateTransform;
                rt.Angle = angleValue;
            }
        }

    }
}
