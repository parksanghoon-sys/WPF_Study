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
    public class CircularGaugeControl : ContentControl
    {
        #region Private 변수
        /// <summary>
        /// GauageControl Rander 하는데 필요한 Variables
        /// </summary>
        private Grid rootGrid;
        private Path rangeIndicator;
        private Path pointer;
        private bool isInitialValueSet = false;
        private double arcradius1;
        private double arcradius2;
        private int animatingSpeedFactor = 6;
        #endregion
        #region Dependency Properties

        public static readonly DependencyProperty GaugeBackgroundColorProperty =
            DependencyProperty.Register("GaugeBackgroundColor", typeof(Color), typeof(CircularGaugeControl), null);

        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register("CurrentValue", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(double.MinValue, new PropertyChangedCallback(CircularGaugeControl.OnCurrentValuePropertyChanged)));

        public static readonly DependencyProperty MinValuePropertyProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(null));

        public static readonly DependencyProperty MaxValuePropertyProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(null));

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(null));
        public static readonly DependencyProperty PointerLengthProperty =
            DependencyProperty.Register("PointerLength", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(null));

        public static readonly DependencyProperty PointerThicknessProperty =
            DependencyProperty.Register("PointerThickness", typeof(double), typeof(CircularGaugeControl), null);

        public static readonly DependencyProperty ScaleRadiusProperty =
            DependencyProperty.Register("ScaleRadius", typeof(double), typeof(CircularGaugeControl), null);

        public static readonly DependencyProperty ScaleStartAngleProperty =
            DependencyProperty.Register("ScaleStartAngle", typeof(double), typeof(CircularGaugeControl), null);

        public static readonly DependencyProperty ScaleSweepAngleProperty =
            DependencyProperty.Register("ScaleSweepAngle", typeof(double), typeof(CircularGaugeControl), null);

        public static readonly DependencyProperty MajorDivisionsCountProperty =
            DependencyProperty.Register("MajorDivisionsCount", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(null));

        public static readonly DependencyProperty MinorDivisionsCountProperty =
            DependencyProperty.Register("MinorDivisionsCount", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(null));

        public static readonly DependencyProperty OptimalRangeEndValueProperty =
           DependencyProperty.Register("OptimalRangeEndValue", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnOptimalRangeEndValuePropertyChanged)));

        public static readonly DependencyProperty OptimalRangeStartValueProperty =
           DependencyProperty.Register("OptimalRangeStartValue", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(new PropertyChangedCallback(CircularGaugeControl.OnOptimalRangeStartValuePropertyChanged)));

        public static readonly DependencyProperty RangeIndicatorRadiusProperty =
            DependencyProperty.Register("RangeIndicatorRadius", typeof(double), typeof(CircularGaugeControl), null);

        public static readonly DependencyProperty RangeIndicatorThicknessProperty =
            DependencyProperty.Register("RangeIndicatorThickness", typeof(double), typeof(CircularGaugeControl), null);

        public static readonly DependencyProperty ScaleLabelRadiusProperty =
            DependencyProperty.Register("ScaleLabelRadius", typeof(double), typeof(CircularGaugeControl), new PropertyMetadata(null));

        public static readonly DependencyProperty ScaleLabelSizeProperty =
            DependencyProperty.Register("ScaleLabelSize", typeof(Size), typeof(CircularGaugeControl), new PropertyMetadata(null));

        public static readonly DependencyProperty ScaleLabelFontSizeProperty =
            DependencyProperty.Register("ScaleLabelFontSize", typeof(double), typeof(CircularGaugeControl), null);

        public static readonly DependencyProperty ScaleLabelForegroundProperty =
            DependencyProperty.Register("ScaleLabelForeground", typeof(Color), typeof(CircularGaugeControl), null);

        public static readonly DependencyProperty MajorTickSizeProperty =
       DependencyProperty.Register("MajorTickSize", typeof(Size), typeof(CircularGaugeControl), null);

        public static readonly DependencyProperty MinorTickSizeProperty =
          DependencyProperty.Register("MinorTickSize", typeof(Size), typeof(CircularGaugeControl), null);

        public static readonly DependencyProperty MajorTickColorProperty =
           DependencyProperty.Register("MajorTickColor", typeof(Color), typeof(CircularGaugeControl), null);

        public static readonly DependencyProperty MinorTickColorProperty =
          DependencyProperty.Register("MinorTickColor", typeof(Color), typeof(CircularGaugeControl), null);

        public static readonly DependencyProperty OptimalRangeColorProperty =
            DependencyProperty.Register("OptimalRangeColor", typeof(Color), typeof(CircularGaugeControl), null);

        public static readonly DependencyProperty AboveOptimalRangeColorProperty =
            DependencyProperty.Register("AboveOptimalRangeColor", typeof(Color), typeof(CircularGaugeControl), null);

        public static readonly DependencyProperty ScaleValuePrecisionProperty =
            DependencyProperty.Register("ScaleValuePrecision", typeof(int), typeof(CircularGaugeControl), null);

        public static readonly DependencyProperty ResetPointerOnStartUpProperty =
            DependencyProperty.Register("ResetPointerOnStartUp", typeof(bool), typeof(CircularGaugeControl), new PropertyMetadata(false, null));
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
            get { return (double)GetValue(MinValuePropertyProperty); }
            set { SetValue(MinValuePropertyProperty, value); }
        }
        /// <summary>
        /// Gauage Max 값
        /// </summary>
        public double MaxValue
        {
            get { return (double)GetValue(MaxValuePropertyProperty); }
            set { SetValue(MaxValuePropertyProperty, value); }
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
        static CircularGaugeControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CircularGaugeControl), new FrameworkPropertyMetadata(typeof(CircularGaugeControl)));
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
            DrawRangeIndicator();

            Canvas.SetZIndex(pointer, 100000);
            if (ResetPointerOnStartUp)
                MovePointer(ScaleStartAngle);
        }
        /// <summary>
        /// Indicator 생성 (게이지 바)
        /// </summary>
        private void DrawRangeIndicator()
        {
            if (OptimalRangeStartValue == 0 && OptimalRangeEndValue == 0)
                return;

            double realworldunit = (ScaleSweepAngle / (MaxValue - MinValue));
            double optimalStartAngle;
            double optimalEndAngle;
            double db;
            
            if (OptimalRangeStartValue < 0)
            {
                db = MinValue + Math.Abs(OptimalRangeStartValue);
                optimalStartAngle = ((double)(Math.Abs(db * realworldunit)));
            }
            else
            {
                db = Math.Abs(MinValue) + OptimalRangeStartValue;
                optimalStartAngle = ((double)(db * realworldunit));
            }
            
            if (OptimalRangeEndValue < 0)
            {
                db = MinValue + Math.Abs(OptimalRangeEndValue);
                optimalEndAngle = ((double)(Math.Abs(db * realworldunit)));
            }
            else
            {
                db = Math.Abs(MinValue) + OptimalRangeEndValue;
                optimalEndAngle = ((double)(db * realworldunit));
            }
            
            double optimalStartAngleFromStart = (ScaleStartAngle + optimalStartAngle);            

            double optimalEndAngleFromStart = (ScaleStartAngle + optimalEndAngle);
            
            arcradius1 = (RangeIndicatorRadius + RangeIndicatorThickness);
            arcradius2 = RangeIndicatorRadius;

            double endAngle = ScaleStartAngle + ScaleSweepAngle;

            Point A = GetCircumferencePoint(ScaleStartAngle, arcradius1);
            Point B = GetCircumferencePoint(ScaleStartAngle, arcradius2);
            Point C = GetCircumferencePoint(optimalStartAngleFromStart, arcradius2);
            Point D = GetCircumferencePoint(optimalStartAngleFromStart, arcradius1);

            bool isReflexAngle = Math.Abs(optimalStartAngleFromStart - ScaleStartAngle) > 180.0;
            DrawSegment(A, B, C, D, isReflexAngle, AboveOptimalRangeColor);

            Point A1 = GetCircumferencePoint(optimalStartAngleFromStart, arcradius1);
            Point B1 = GetCircumferencePoint(optimalStartAngleFromStart, arcradius2);
            Point C1 = GetCircumferencePoint(optimalEndAngleFromStart, arcradius2);
            Point D1 = GetCircumferencePoint(optimalEndAngleFromStart, arcradius1);
            bool isReflexAngle1 = Math.Abs(optimalEndAngleFromStart - optimalStartAngleFromStart) > 180.0;
            DrawSegment(A1, B1, C1, D1, isReflexAngle1, OptimalRangeColor);

            Point A2 = GetCircumferencePoint(optimalEndAngleFromStart, arcradius1);
            Point B2 = GetCircumferencePoint(optimalEndAngleFromStart, arcradius2);
            Point C2 = GetCircumferencePoint(endAngle, arcradius2);
            Point D2 = GetCircumferencePoint(endAngle, arcradius1);
            bool isReflexAngle2 = Math.Abs(endAngle - optimalEndAngleFromStart) > 180.0;
            DrawSegment(A2, B2, C2, D2, isReflexAngle2, AboveOptimalRangeColor);
        }
        /// <summary>
        /// Indigate 각도를 계산해주는 함수
        /// </summary>
        /// <param name="angle">시작 각도</param>
        /// <param name="radius"></param>
        /// <returns></returns>
        private Point GetCircumferencePoint(double angle, double radius)
        {
            double angle_radian = (angle * Math.PI) / 180;
            //Radius-- is the Radius of the gauge
            double X = (double)((Radius) + (radius) * Math.Cos(angle_radian));
            double Y = (double)((Radius) + (radius) * Math.Sin(angle_radian));
            Point p = new Point(X, Y);
            return p;
        }
        private void DrawSegment(Point p1, Point p2, Point p3, Point p4, bool reflexangle, Color clr)
        {

            // Segment Geometry
            PathSegmentCollection segments = new PathSegmentCollection();

            // First line segment from pt p1 - pt p2
            segments.Add(new LineSegment() { Point = p2 });

            //Arc drawn from pt p2 - pt p3 with the RangeIndicatorRadius 
            segments.Add(new ArcSegment()
            {
                Size = new Size(arcradius2, arcradius2),
                Point = p3,
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = reflexangle

            });

            // Second line segment from pt p3 - pt p4
            segments.Add(new LineSegment() { Point = p4 });

            //Arc drawn from pt p4 - pt p1 with the Radius of arcradius1 
            segments.Add(new ArcSegment()
            {
                Size = new Size(arcradius1, arcradius1),
                Point = p1,
                SweepDirection = SweepDirection.Counterclockwise,
                IsLargeArc = reflexangle

            });

            // Defining the segment path properties
            Color rangestrokecolor;
            if (clr == Colors.Transparent)
            {
                rangestrokecolor = clr;
            }
            else
                rangestrokecolor = Colors.White;



            rangeIndicator = new Path()
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

            //Set Z index of range indicator
            rangeIndicator.SetValue(Canvas.ZIndexProperty, 150);
            // Adding the segment to the root grid 
            rootGrid.Children.Add(rangeIndicator);

        }
        /// <summary>
        /// Gauge Indigate 생성
        /// </summary>
        private void DrawScale()
        {
            //Calculate one major tick angle 
            double majorTickUnitAngle = ScaleSweepAngle / MajorDivisionsCount;

            //Obtaining One minor tick angle 
            double minorTickUnitAngle = ScaleSweepAngle / MinorDivisionsCount;

            //Obtaining One major ticks value
            double majorTicksUnitValue = (MaxValue - MinValue) / MajorDivisionsCount;
            majorTicksUnitValue = Math.Round(majorTicksUnitValue, ScaleValuePrecision);

            double minvalue = MinValue;

            // Drawing Major scale ticks
            for (double i = ScaleStartAngle; i <= (ScaleStartAngle + ScaleSweepAngle); i = i + majorTickUnitAngle)
            {

                //Majortick is drawn as a rectangle 
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

                //Obtaining the angle in radians for calulating the points
                double i_radian = (i * Math.PI) / 180;
                majortickrt.Angle = i;
                majortickgp.Children.Add(majortickrt);
                TranslateTransform majorticktt = new TranslateTransform();

                //Finding the point on the Scale where the major ticks are drawn
                //here drawing the points with center as (0,0)
                majorticktt.X = (int)((ScaleRadius) * Math.Cos(i_radian));
                majorticktt.Y = (int)((ScaleRadius) * Math.Sin(i_radian));

                //Points for the textblock which hold the scale value
                TranslateTransform majorscalevaluett = new TranslateTransform();
                //here drawing the points with center as (0,0)
                majorscalevaluett.X = (int)((ScaleLabelRadius) * Math.Cos(i_radian));
                majorscalevaluett.Y = (int)((ScaleLabelRadius) * Math.Sin(i_radian));

                //Defining the properties of the scale value textbox
                TextBlock tb = new TextBlock();

                tb.Height = ScaleLabelSize.Height;
                tb.Width = ScaleLabelSize.Width;
                tb.FontSize = ScaleLabelFontSize;
                tb.Foreground = new SolidColorBrush(ScaleLabelForeground);
                tb.TextAlignment = TextAlignment.Center;
                tb.VerticalAlignment = VerticalAlignment.Center;
                tb.HorizontalAlignment = HorizontalAlignment.Center;

                //Writing and appending the scale value

                //checking minvalue < maxvalue w.r.t scale precion value
                if (Math.Round(minvalue, ScaleValuePrecision) <= Math.Round(MaxValue, ScaleValuePrecision))
                {
                    minvalue = Math.Round(minvalue, ScaleValuePrecision);
                    tb.Text = Math.Abs(minvalue).ToString();
                    minvalue = minvalue + majorTicksUnitValue;
               }
                else
                {
                    break;
                }
                majortickgp.Children.Add(majorticktt);
                majortickrect.RenderTransform = majortickgp;
                tb.RenderTransform = majorscalevaluett;
                rootGrid.Children.Add(majortickrect);
                rootGrid.Children.Add(tb);


                //Drawing the minor axis ticks
                double onedegree = ((i + majorTickUnitAngle) - i) / (MinorDivisionsCount);

                if ((i < (ScaleStartAngle + ScaleSweepAngle)) && (Math.Round(minvalue, ScaleValuePrecision) <= Math.Round(MaxValue, ScaleValuePrecision)))
                {
                    //Drawing the minor scale
                    for (double mi = i + onedegree; mi < (i + majorTickUnitAngle); mi = mi + onedegree)
                    {
                        //here the minortick is drawn as a rectangle 
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

                        //Obtaining the angle in radians for calulating the points
                        double mi_radian = (mi * Math.PI) / 180;
                        //Finding the point on the Scale where the minor ticks are drawn
                        minorticktt.X = (int)((ScaleRadius) * Math.Cos(mi_radian));
                        minorticktt.Y = (int)((ScaleRadius) * Math.Sin(mi_radian));

                        minortickgp.Children.Add(minorticktt);
                        mr.RenderTransform = minortickgp;
                        rootGrid.Children.Add(mr);

                    }

                }

            }
        }      
        #endregion

        private static void OnCurrentValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CircularGaugeControl gauge = d as CircularGaugeControl;
            gauge.OnCurrentValueChanged(e);
            gauge.DrawScale();
            gauge.DrawRangeIndicator();
        }
        /// <summary>
        /// 게이지바 안전 상태 시작 설정부분 제한
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>

        private static void OnOptimalRangeStartValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CircularGaugeControl gauge = d as CircularGaugeControl;
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
            CircularGaugeControl gauge = d as CircularGaugeControl;
            if ((double)e.NewValue > gauge.MaxValue)
                gauge.OptimalRangeEndValue = gauge.MaxValue;
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
                double db1 = 0;
                double oldcurr_realworldunit = 0;
                double newcurr_realworldunit = 0;
                double realworldunit = (ScaleSweepAngle / (MaxValue - MinValue));

                if (oldValue == 0 && !isInitialValueSet)
                {
                    oldValue = MinValue;
                    isInitialValueSet = true;
                }
                if (oldValue < 0)
                {
                    db1 = MinValue + Math.Abs(oldValue);
                    oldcurr_realworldunit = ((double)(Math.Abs(db1 * realworldunit)));
                }
                else
                {
                    db1 = Math.Abs(MinValue) + oldValue;
                    oldcurr_realworldunit = ((double)(db1 * realworldunit));
                }
                if (newValue < 0)
                {
                    db1 = MinValue + Math.Abs(newValue);
                    newcurr_realworldunit = ((double)(Math.Abs(db1 * realworldunit)));
                }
                else
                {
                    db1 = Math.Abs(MinValue) + newValue;
                    newcurr_realworldunit = ((double)(db1 * realworldunit));
                }
                double oldCurrentValueAngle = ScaleStartAngle + oldcurr_realworldunit;
                double newCurrentValueAngle = ScaleStartAngle + newcurr_realworldunit;

                AnimaterPointer(oldCurrentValueAngle, newCurrentValueAngle);
            }

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
