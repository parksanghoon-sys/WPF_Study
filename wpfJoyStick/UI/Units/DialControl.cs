using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace wpfJoyStick.UI.Units
{
    public class DialControl : ContentControl
    {
        private Point _center;
        private double _radius;
        private bool _isDragging;
        private TranslateTransform _smallCircleTransform;
        public static readonly DependencyProperty AngleProperty =
           DependencyProperty.Register(nameof(Angle), typeof(double), typeof(DialControl),
               new PropertyMetadata(0.0, OnAngleChanged));

        public double Angle
        {
            get => (double)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }
        static DialControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialControl),
          new FrameworkPropertyMetadata(typeof(DialControl)));
        }

        public DialControl()
        {
            Loaded += OnLoaded;
            MouseDown += OnMouseDown;
            MouseMove += OnMouseMove;
            MouseUp += OnMouseUp;
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();            
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // 큰 원의 중심점과 반지름 설정
            if (ActualWidth > 0 && ActualHeight > 0)
            {
                _center = new Point(ActualWidth / 2, ActualHeight / 2);
                _radius = (ActualWidth / 2) - 15; // 큰 원 반지름 - 작은 원 반지름

                // ControlTemplate 내의 TranslateTransform 참조
                var template = Template;
                if (template != null && template.FindName("SmallCircleTransform", this) is TranslateTransform transform)
                {
                    _smallCircleTransform = transform;
                    _smallCircleTransform.X = 0;
                    _smallCircleTransform.Y = -_radius;
                }
            }

        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _isDragging = true;
            CaptureMouse();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging || _smallCircleTransform == null) return;

            var currentPoint = e.GetPosition(this);
            var angle = Math.Atan2(currentPoint.Y - _center.Y, currentPoint.X - _center.X) * (180 / Math.PI);

            // 각도 범위 [0, 360]으로 보정 및 반전
            if (angle < 0) angle += 360;
            Angle = (angle - 180) % 360; // 각도를 반전하여 업데이트

            UpdateSmallCirclePosition(); // 위치 업데이트
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            ReleaseMouseCapture();
        }
        private static void OnAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DialControl control)
            {
                control.UpdateSmallCirclePosition();
            }
        }

        private void UpdateSmallCirclePosition()
        {
            // 현재 각도를 이용하여 작은 원의 위치를 설정
            var radians = Angle * (Math.PI / 180); // 각도를 라디안으로 변환
            _smallCircleTransform.X = _radius * Math.Cos(radians);
            _smallCircleTransform.Y = -_radius * Math.Sin(radians);
        }
    }
}
