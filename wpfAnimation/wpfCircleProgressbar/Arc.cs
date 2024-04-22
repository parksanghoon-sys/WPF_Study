using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Timers;

namespace wpfCircleProgressbar
{

    public partial class Arc : Shape
    {
        /// <summary>
        /// 시작 조정 각도
        /// </summary>
        public static readonly DependencyProperty ShapeRotationAdjustmentProperty =
        DependencyProperty.Register("ShapeRotationAdjustment", typeof(double), typeof(Arc), new PropertyMetadata(0d, UpdateArc));
        /// <summary>
        /// The shape mode use fade property
        /// </summary>
        public static readonly DependencyProperty ShapeModeUseFadeProperty =
            DependencyProperty.Register("ShapeModeUseFade", typeof(bool), typeof(Arc), new PropertyMetadata(true, UpdateArc));

        public static readonly DependencyProperty ShapeModeShapeProperty =
            DependencyProperty.Register("ShapeModeShape", typeof(ArcShape), typeof(Arc), new UIPropertyMetadata(ArcShape.Rectangle, UpdateArc));

        public static readonly DependencyProperty ShapeModeWidthProperty =
            DependencyProperty.Register("ShapeModeWidth", typeof(double), typeof(Arc), new UIPropertyMetadata(1d, UpdateArc));

        public static readonly DependencyProperty ShapeModeStepProperty =
            DependencyProperty.Register("ShapeModeStep", typeof(int), typeof(Arc), new UIPropertyMetadata(3, UpdateArc));

        public static readonly DependencyProperty ArcModeProperty =
            DependencyProperty.Register("ArcMode", typeof(ArcMode), typeof(Arc), new UIPropertyMetadata(ArcMode.Fill, UpdateArc));

        public static readonly DependencyProperty ProgressBorderThicknessProperty =
            DependencyProperty.Register("ProgressBorderThickness", typeof(Thickness), typeof(Arc), new PropertyMetadata(new Thickness(0), UpdatePen));

        public static readonly DependencyProperty ProgressFillBrushProperty =
            DependencyProperty.Register("ProgressFillBrush", typeof(Brush), typeof(Arc), new PropertyMetadata(Brushes.Transparent, UpdateFillBrush));

        // Using a DependencyProperty as the backing store for ProgressBackgroundBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProgressBackgroundBrushProperty =
            DependencyProperty.Register("ProgressBackgroundBrush", typeof(Brush), typeof(Arc), new PropertyMetadata(Brushes.Transparent, UpdateBgArc));

        public static readonly DependencyProperty ProgressBorderBrushProperty =
            DependencyProperty.Register("ProgressBorderBrush", typeof(Brush), typeof(Arc), new UIPropertyMetadata(Brushes.Transparent, UpdatePen));

        // Using a DependencyProperty as the backing store for StartAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(Arc), new UIPropertyMetadata(0.0, UpdateArc));

        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register("EndAngle", typeof(double), typeof(Arc), new UIPropertyMetadata(90.0, UpdateArc));

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(SweepDirection), typeof(Arc), new UIPropertyMetadata(SweepDirection.Clockwise));

        public static readonly DependencyProperty OriginRotationDegreesProperty =
            DependencyProperty.Register("OriginRotationDegrees", typeof(double), typeof(Arc), new UIPropertyMetadata(270d, UpdateArc));

        public static readonly DependencyProperty IsIndeterminateProperty =
           DependencyProperty.Register("IsIndeterminate", typeof(bool), typeof(Arc), new UIPropertyMetadata(true, UpdateIndeterminate));

        public static readonly DependencyProperty IndeterminateSpeedRatioProperty =
           DependencyProperty.Register("IndeterminateSpeedRatio", typeof(double), typeof(Arc), new UIPropertyMetadata(1d, (o, args) => (o as Arc)?.UpdateIndeterminate()));

        public static readonly DependencyProperty ArcBackgroundWidthProperty =
         DependencyProperty.Register("ArcBackgroundWidth", typeof(double), typeof(Arc), new UIPropertyMetadata(0d, UpdateBgArc));
        public double ShapeRotationAdjustment
        {
            get { return (double)GetValue(ShapeRotationAdjustmentProperty); }
            set { SetValue(ShapeRotationAdjustmentProperty, value); }
        }
        public bool ShapeModeUseFade
        {
            get { return (bool)GetValue(ShapeModeUseFadeProperty); }
            set { SetValue(ShapeModeUseFadeProperty, value); }
        }
        // 모양
        public ArcShape ShapeModeShape
        {
            get => (ArcShape)GetValue(ShapeModeShapeProperty);
            set => SetValue(ShapeModeShapeProperty, value);
        }
        /// <summary>
        /// 네모 간격
        /// </summary>
        public double ShapeModeWidth
        {
            get => (double)GetValue(ShapeModeWidthProperty);
            set => SetValue(ShapeModeWidthProperty, value);
        }
        // 각 스텝 간격
        public int ShapeModeStep
        {
            get => (int)GetValue(ShapeModeStepProperty);
            set => SetValue(ShapeModeStepProperty, value);
        }
        // 프로그레스 채울 모양
        public ArcMode ArcMode
        {
            get => (ArcMode)GetValue(ArcModeProperty);
            set => SetValue(ArcModeProperty, value);
        }
        // Progress 두께
        public Thickness ProgressBorderThickness
        {
            get { return (Thickness)GetValue(ProgressBorderThicknessProperty); }
            set { SetValue(ProgressBorderThicknessProperty, value); }
        }
        // Progress 색상
        public Brush ProgressFillBrush
        {
            get { return (Brush)GetValue(ProgressFillBrushProperty); }
            set { SetValue(ProgressFillBrushProperty, value); }
        }
        // 프로그레스바 배경 색
        public Brush ProgressBackgroundBrush
        {
            get { return (Brush)GetValue(ProgressBackgroundBrushProperty); }
            set { SetValue(ProgressBackgroundBrushProperty, value); }
        }
        // 프로그레스바 테두리 색상
        public Brush ProgressBorderBrush
        {
            get => (Brush)GetValue(ProgressBorderBrushProperty);
            set => SetValue(ProgressBorderBrushProperty, value);
        }
        // 시작 각도
        public double StartAngle
        {
            get => (double)GetValue(StartAngleProperty);
            set => SetValue(StartAngleProperty, value);
        }
        // 끝 각도
        public double EndAngle
        {
            get => (double)GetValue(EndAngleProperty);
            set => SetValue(EndAngleProperty, value);
        }

        // 회전 방향
        public SweepDirection Direction
        {
            get => (SweepDirection)GetValue(DirectionProperty);
            set => SetValue(DirectionProperty, value);
        }

        public double OriginRotationDegrees
        {
            get => (double)GetValue(OriginRotationDegreesProperty);
            set => SetValue(OriginRotationDegreesProperty, value);
        }

        public bool IsIndeterminate
        {
            get => (bool)GetValue(IsIndeterminateProperty);
            set => SetValue(IsIndeterminateProperty, value);
        }
       
        public double IndeterminateSpeedRatio
        {
            get => (double)GetValue(IndeterminateSpeedRatioProperty);
            set => SetValue(IndeterminateSpeedRatioProperty, value);
        }
        public double ArcBackgroundWidth
        {
            get => (double)GetValue(ArcBackgroundWidthProperty);
            set => SetValue(ArcBackgroundWidthProperty, value);
        }
        public Arc()
        {
            Loaded += (sender, oargs) =>
            {
                UpdateIndeterminate();
            };
        }
        private List<AngleData> _data;
        private Pen _pen;
        private Pen _clipPen;
        private Pen _bgClipPen;
        private Brush _semiBrush1;
        private Brush _semiBrush2;
        private Brush _semiBrush3;        
        protected override Geometry DefiningGeometry => GetArcGeometry();
        protected override void OnRender(DrawingContext drawingContext)
        {
            if(_pen == null)
            {
                UpdatePen();
                UpdateBackgroundPen();
            }
            if(ArcMode == ArcMode.Fill|| ArcMode == ArcMode.Pie)
            {
                var radiusX = RenderSize.Width / 2;
                var radiusY = RenderSize.Height / 2;
                var centerPoint = new Point(radiusX, radiusY);

                if(ProgressBackgroundBrush != Brushes.Transparent)
                {
                    var clipb = GetArcGeometry(true).GetWidenedPathGeometry(_bgClipPen);
                    if(clipb.CanFreeze)
                        clipb.Freeze();

                    drawingContext.PushClip(clipb);

                    drawingContext.DrawEllipse(ProgressBackgroundBrush, null, centerPoint, radiusX, radiusY);
                    drawingContext.Pop();
                }

                var clip = GetArcGeometry().GetWidenedPathGeometry(_clipPen);
                if(clip.CanFreeze)
                    clip.Freeze();

                drawingContext.PushClip(clip);
                drawingContext.DrawEllipse(ProgressFillBrush, null, centerPoint, radiusX, radiusY);
                if (ProgressBorderBrush != Brushes.Transparent && ProgressBorderThickness.Top > 0)
                    drawingContext.DrawGeometry(null, _pen, clip);
                drawingContext.Pop();
            }
            else
            {
                //SHAPE MODE
                //update brushes on first pass
                if (_semiBrush1 == null)
                {
                    UpdateInternalBrushes();
                    //precalculate all angles for performance reasons
                    _data = GetAngleData();
                }

                //var minAngle = Math.Min(StartAngle, EndAngle);
                var maxAngle = Math.Max(StartAngle, EndAngle);
                //get number of draw passes to process
                var max = Math.Round(maxAngle / ShapeModeStep);
                //_data = GetAngleData(ShapeModeStep);

                for (int i = 0; i < max; i++)
                {
                    var data = _data[i];
                    ///////////////////////////
                    /* drawingContext.DrawEllipse(Brushes.Red, _pen, data.StartPoint, 2,2);
                     var radiusX = RenderSize.Width / 2;
                     var radiusY = RenderSize.Height / 2;
                     var centerPoint = new Point(radiusX, radiusY);
                     drawingContext.DrawEllipse(Brushes.Red, _pen, centerPoint, 2,2);*/
                    ///////////////////////////////////

                    var half = ShapeModeWidth * .5;
                    var rect = new Rect(data.StartPoint.X - half, data.StartPoint.Y, ShapeModeWidth, StrokeThickness);
                    //apply rotation to context
                    drawingContext.PushTransform(new RotateTransform(data.Angle, data.StartPoint.X, data.StartPoint.Y));
                    var brush = ProgressFillBrush;
                    //apply transparency
                    if (ShapeModeUseFade)
                    {
                        if (i == max - 2)
                            brush = _semiBrush1;
                        if (i == max - 1)
                            brush = _semiBrush2;
                        else if (i == max)
                            brush = _semiBrush3;
                    }
                    //draw shape
                    drawingContext.DrawRectangle(brush, _pen, rect);
                    drawingContext.Pop();
                }
            }
        }

        private List<AngleData> GetAngleData()
        {
            var minAngle = Math.Min(StartAngle, EndAngle);
            var maxAngle = 359.999;

            var dic = new List<AngleData>();
            var startAngle = minAngle;
            var radiusX = RenderSize.Width / 2;
            var radiusY = RenderSize.Height / 2;
            var centerPoint = new Point(radiusX, radiusY);

            while (true)
            {
                if (startAngle > maxAngle)
                    break;
                var a = (Direction == SweepDirection.Clockwise ? -1 : 1) * (startAngle + OriginRotationDegrees) * (Math.PI / 180);

                var pt = new Point
                {
                    Y = centerPoint.Y - radiusY * Math.Sin(a),
                    X = centerPoint.X + radiusX * Math.Cos(a)
                };
                var a2 = GetAngleBetweenPoints(pt, centerPoint);
                dic.Add(new AngleData { StartPoint = pt, Angle = a2 });
                startAngle += ShapeModeStep;
            }

            return dic;
        }

        private double GetAngleBetweenPoints(Point one, Point two)
        {
            var xDiff = two.X - one.X;
            var yDiff = two.Y - one.Y;
            var angle = Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI;
            return angle + ShapeRotationAdjustment - 90;
        }

        private void UpdateInternalBrushes()
        {
            if (ShapeModeUseFade && ProgressFillBrush != null)
            {
                _semiBrush1 = ProgressFillBrush.Clone();
                _semiBrush1.Opacity = .7;
                if (_semiBrush1.CanFreeze)
                    _semiBrush1.Freeze();
                _semiBrush2 = ProgressFillBrush.Clone();
                _semiBrush2.Opacity = .5;
                if (_semiBrush2.CanFreeze)
                    _semiBrush2.Freeze();
                _semiBrush3 = ProgressFillBrush.Clone();
                _semiBrush3.Opacity = .3;
                if (_semiBrush3.CanFreeze)
                    _semiBrush3.Freeze();
            }
        }

        private Geometry GetArcGeometry(bool full = false)
        {
            var width = ArcMode == ArcMode.Pie ? RenderSize.Width * .5 : StrokeThickness;
            var startPoint = PointAtAngle(full ? 0d : Math.Min(StartAngle, EndAngle), Direction);
            var endPoint = PointAtAngle(full ? 359.999d : Math.Max(StartAngle, EndAngle), Direction);

            var arcSize = new Size(Math.Max(0, (RenderSize.Width - width)) / 2, Math.Max(0, (RenderSize.Height - width) / 2));
            var isLargeArc = full || Math.Abs(EndAngle - StartAngle) > 180;
            var geom = new StreamGeometry();
            using(var context = geom.Open())
            {
                context.BeginFigure(startPoint, false, false);
                context.ArcTo(endPoint, arcSize, 0, isLargeArc, Direction, true, false);
            }
            geom.Transform = new TranslateTransform(width / 2, width / 2);
            return geom;
        }

        private Point PointAtAngle(double angle, SweepDirection direction)
        {
            var width = ArcMode == ArcMode.Pie ? RenderSize.Width * .5 : StrokeThickness;

            var translatedAngle = angle + OriginRotationDegrees;
            var radAngle = translatedAngle * (Math.PI / 180);
            var xr = (RenderSize.Width - width) / 2;
            var yr = (RenderSize.Height - width) / 2;

            var x = xr + xr * Math.Cos(radAngle);
            var y = yr * Math.Sin(radAngle);

            if (direction == SweepDirection.Counterclockwise)
            {
                y = yr - y;
            }
            else
            {
                y = yr + y;
            }

            return new Point(x, y);

        }

        private void UpdateBackgroundPen()
        {
            var width = ArcBackgroundWidth == 0 ? RenderSize.Width * .5 : ArcBackgroundWidth;
            var m = PresentationSource.FromVisual(this)
                .CompositionTarget.TransformToDevice;
            var dpiFactor = 1 / m.M11;

            _bgClipPen = new Pen(Brushes.White, width * dpiFactor);
            if (_bgClipPen.CanFreeze)
                _bgClipPen.Freeze();
        }

        private void UpdatePen()
        {
            var width = ArcMode == ArcMode.Pie ? RenderSize.Width * .5 : StrokeThickness;

            var m = PresentationSource.FromVisual(this)
                .CompositionTarget.TransformToDevice;
            var dpiFactor = 1 / m.M11;
            _pen = new Pen(ProgressBorderBrush, ProgressBorderThickness.Top * dpiFactor);
            if (_pen.CanFreeze)
                _pen.Freeze();
            _clipPen = new Pen(Brushes.White, width * dpiFactor);
            if (_clipPen.CanFreeze)
                _clipPen.Freeze();
        }
        private System.Timers.Timer _inTimer;
        private volatile bool _inEnd;
        private void UpdateIndeterminate()
        {
            if (!IsIndeterminate)
            {
                _inTimer?.Stop();
                _inTimer?.Dispose();
                if (_inTimer != null)
                    SetCurrentValue(EndAngleProperty, StartAngle);
                _inTimer = null;
            }
            else
            {
                _inTimer?.Dispose();
                _inTimer = new System.Timers.Timer(10 * IndeterminateSpeedRatio);
                _inTimer.Elapsed += async (sender, args) =>
                {
                    try
                    {
                        await Dispatcher.InvokeAsync(() =>
                        {
                            var value = (double)GetValue(EndAngleProperty) + 9;
                            if (_inEnd)
                            {
                                _inEnd = false;
                                value = 0;
                                SetCurrentValue(EndAngleProperty, value);
                            }
                            else if (value >= 360)
                            {
                                value = 359.999;
                                SetCurrentValue(EndAngleProperty, value);
                                _inEnd = true;
                            }
                            else SetCurrentValue(EndAngleProperty, value);
                        });
                    }
                    catch
                    {
                        // ignore
                    }
                };
                _inTimer.Start();
            }
        }
        private static void UpdateIndeterminate(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Arc)d).UpdateIndeterminate();

            UpdateArc(d, e);
        }
    

        private static void UpdatePen(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Arc)d).UpdatePen();
            UpdateArc(d, e);
        }

        private static void UpdateBgArc(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Arc)d).UpdateBackgroundPen();
            ((Arc)d).InvalidateVisual();
        }

        private static void UpdateFillBrush(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var arc = d as Arc;
            arc?.UpdateInternalBrushes();
            UpdateArc(d, e);
        }

        private static void UpdateArc(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Arc)d)?.InvalidateVisual();
        }
        internal void RecalculateShapes()
        {
            _data = GetAngleData();
        }
    }
}
