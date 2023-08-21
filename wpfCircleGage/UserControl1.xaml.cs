using System;
using System.Collections.Generic;
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

namespace wpfCircleGage
{
    /// <summary>
    /// UserControl1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        const double GaugeStartAngle = 150;
        const double GaugeEndAngle = GaugeStartAngle + 240;
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        "Value", typeof(double), typeof(UserControl1),
        new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender, OnValueChanged));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public UserControl1()
        {
            InitializeComponent();
            
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            DrawGaugeArc();
            DrawNeedleIndicator();
        }

        private void DrawGaugeArc()
        {
            var geometry = new StreamGeometry();
            Point gaugeStartPoint = ComputeGaugePoint(GaugeStartAngle);
            Point gaugeEndPoint = ComputeGaugePoint(GaugeEndAngle);

            using (StreamGeometryContext context = geometry.Open())
            {
                context.BeginFigure(gaugeStartPoint, false, false);
                context.ArcTo(
                    gaugeEndPoint,
                    new Size(ActualWidth / 2, ActualHeight / 2),
                    0,
                    GaugeEndAngle - GaugeStartAngle > 180,
                    SweepDirection.Clockwise,
                    true,
                    false);
            }
            gaugePath.Data = geometry;
        }

        private void DrawNeedleIndicator()
        {
            double needleAngle = GaugeStartAngle + (GaugeEndAngle - GaugeStartAngle) * Value / 100;
            Point needleStartPoint = new Point(ActualWidth / 2, ActualHeight / 2);
            Point needleEndPoint = ComputeGaugePoint(needleAngle);

            var needleGeometry = new LineGeometry(needleStartPoint, needleEndPoint);
            needleIndicator.Data = needleGeometry;
        }

        private Point ComputeGaugePoint(double angle)
        {
            double angleRad = Math.PI * angle / 180.0;
            double x = ActualWidth / 2 + Math.Cos(angleRad) * (ActualWidth / 2 - 10);
            double y = ActualHeight / 2 - Math.Sin(angleRad) * (ActualHeight / 2 - 10);
            return new Point(x, y);
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UserControl1 gauge = (UserControl1)d;
            gauge.DrawGaugeArc();
            gauge.DrawNeedleIndicator();
        }
    }
}
