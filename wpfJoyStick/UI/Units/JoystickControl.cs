using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using wpfJoyStick.Local.Models;

namespace wpfJoyStick.UI.Units
{
    [TemplatePart(Name = "PART_InnerCircle", Type = typeof(Ellipse))]
    [TemplatePart(Name = "PART_JoystickContainer", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_JoystickGrid", Type = typeof(Grid))]
    public class JoystickControl : ContentControl
    {
        private const double DEFAULT_MAX_VALUE = 100.0;

        public static readonly DependencyProperty MaxValueProperty =
                    DependencyProperty.Register("MaxValue", typeof(double), typeof(JoystickControl), new PropertyMetadata(DEFAULT_MAX_VALUE));

        public static readonly DependencyProperty CurrentXValueProperty =
            DependencyProperty.Register("CurrentXValue", typeof(double), typeof(JoystickControl), new PropertyMetadata(0.0));

        public static readonly DependencyProperty CurrentYValueProperty =
            DependencyProperty.Register("CurrentYValue", typeof(double), typeof(JoystickControl), new PropertyMetadata(0.0));
        public static readonly DependencyProperty JoystickMovedCommandProperty =
                    DependencyProperty.Register(nameof(JoystickMovedCommand), typeof(ICommand), typeof(JoystickControl), new PropertyMetadata(null));
        public ICommand JoystickMovedCommand
        {
            get => (ICommand)GetValue(JoystickMovedCommandProperty);
            set => SetValue(JoystickMovedCommandProperty, value);
        }

        public double MaxValue
        {
            get => (double)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        public double CurrentXValue
        {
            get => (double)GetValue(CurrentXValueProperty);
            private set => SetValue(CurrentXValueProperty, value);
        }

        public double CurrentYValue
        {
            get => (double)GetValue(CurrentYValueProperty);
            private set => SetValue(CurrentYValueProperty, value);
        }

        public static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<Point>), typeof(JoystickControl));

        public event RoutedPropertyChangedEventHandler<Point> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        static JoystickControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(JoystickControl),
               new FrameworkPropertyMetadata(typeof(JoystickControl)));
        }

        private Ellipse outerCircle;
        private Grid joystickGrid;
        private Canvas joystickContainer;
        private Point centerPoint;
        private double radius;
        private bool isDragging;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            outerCircle = GetTemplateChild("PART_OutCircle") as Ellipse;
            joystickGrid = GetTemplateChild("PART_JoystickGrid") as Grid;
            joystickContainer = GetTemplateChild("PART_JoystickContainer") as Canvas;

            if (joystickContainer != null)
            {
                this.MouseLeftButtonDown += OnMouseLeftButtonDown;
                this.MouseLeftButtonUp += OnMouseLeftButtonUp;
                this.MouseMove += OnMouseMove;
                this.SizeChanged += OnSizeChanged;
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            radius = Math.Min(ActualWidth, ActualHeight) / 2 - 20; // margin 고려
            centerPoint = new Point(ActualWidth / 2, ActualHeight / 2);

            if (outerCircle != null)
            {
                outerCircle.Width = radius * 2;
                outerCircle.Height = radius * 2;
            }

            ResetJoystickPosition();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                UpdateJoystickPosition(e.GetPosition(this));
            }
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            CaptureMouse();
            UpdateJoystickPosition(e.GetPosition(this));
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            ReleaseMouseCapture();
            ResetJoystickPosition();
        }

        private void UpdateJoystickPosition(Point mousePosition)
        {
            Vector vector = Point.Subtract(mousePosition, centerPoint);
            double distance = vector.Length;

            if (distance > radius)
            {
                vector = vector * (radius / distance);
                distance = radius;
            }

            Point newPosition = Point.Add(centerPoint, vector);

            if (joystickGrid != null)
            {
                Canvas.SetLeft(joystickGrid, newPosition.X - joystickGrid.ActualWidth / 2);
                Canvas.SetTop(joystickGrid, newPosition.Y - joystickGrid.ActualHeight / 2);
            }

            CurrentXValue = Math.Round((vector.X / radius) * MaxValue, 1);
            CurrentYValue = Math.Round((-vector.Y / radius) * MaxValue, 1);  // Y축은 반전

            if (JoystickMovedCommand != null && JoystickMovedCommand.CanExecute(null))
            {
                var joystickPosition = new JoystickPosition(CurrentXValue, CurrentYValue);
                JoystickMovedCommand.Execute(joystickPosition);
            }

            RaiseEvent(new RoutedPropertyChangedEventArgs<Point>(
                new Point(0, 0),
                new Point(CurrentXValue, CurrentYValue),
                ValueChangedEvent));
        }

        private void ResetJoystickPosition()
        {
            if (joystickGrid != null)
            {
                Canvas.SetLeft(joystickGrid, centerPoint.X - joystickGrid.ActualWidth / 2);
                Canvas.SetTop(joystickGrid, centerPoint.Y - joystickGrid.ActualHeight / 2);
            }

            CurrentXValue = 0;
            CurrentYValue = 0;

            RaiseEvent(new RoutedPropertyChangedEventArgs<Point>(
                new Point(CurrentXValue, CurrentYValue),
                new Point(0, 0),
                ValueChangedEvent));
        }
    }
}