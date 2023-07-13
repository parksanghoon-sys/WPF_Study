using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using wpfControls.Controls.SliderPanel_;
using wpfControls.Utilites;

namespace wpfControls.Controls
{
    /// <summary>
    /// SlidePanel.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SlidePanel : UserControl
    {

        private Border? _slider;
        private SlideState _slideState = SlideState.Closed;
        private readonly Lazy<AnimationManager> _slideAnimationLazy;
        private readonly Lazy<AnimationManager> _leftAnimationLazy;

        public AnimationManager SlideAnimation => _slideAnimationLazy.Value;
        public AnimationManager LeftAnimation => _leftAnimationLazy.Value;
        // Using a DependencyProperty as the backing store for SliderWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SliderWidthProperty =
            DependencyProperty.Register("SliderWidth", typeof(double), typeof(SlidePanel), new PropertyMetadata(300d, OnSliderChanged));
        public static readonly DependencyProperty SliderLocationProperty =
            DependencyProperty.Register("SliderLocation", typeof(SliderLocation), typeof(SlidePanel), new PropertyMetadata(SliderLocation.Left, OnSliderChanged));
        private static void OnSliderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SlidePanel slidePanel)
            {
                slidePanel.ChangeSlideLeft();
            }
        }
        public double ParantActualWidth
        {
            get
            {
                var parent = Parent as FrameworkElement;
                return parent != null ? parent.ActualWidth : 0;
            }
        }
        public SliderLocation SliderLocation
        {
            get { return (SliderLocation)GetValue(SliderLocationProperty); }
            set { SetValue(SliderLocationProperty, value); }
        }
         
        public double SliderWidth
        {
            get { return (double)GetValue(SliderWidthProperty); }
            set { SetValue(SliderWidthProperty, value); }
        }
        public double AnimationSpeed { get; set; } = 300d;

        private void ChangeSlideLeft()
        {
            if (_slider is not null)
            {
                double left = SliderLocation == SliderLocation.Left ? -SliderWidth : ParantActualWidth;
                Canvas.SetLeft(_slider, left);
            }
        }
        public SlidePanel()
        {
            InitializeComponent();
            Loaded += SlidePanel_Load;
            SizeChanged += SlidePanel_SizeChanged;

            _slideAnimationLazy = new Lazy<AnimationManager>(() =>
            {
                var animationManaget = new AnimationManager(_slider!);
                animationManaget.Storyboard.Completed += Storyboard_Completed;
                return animationManaget;
            });
            _leftAnimationLazy = new Lazy<AnimationManager>(() => new AnimationManager(_slider!));
        }


        private void SlidePanel_Load(object sender, RoutedEventArgs e)
        {
            this.SendToBack();
            ChangeSlideLeft();
        }
        private void SlidePanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(SliderLocation == SliderLocation.Right)
            {
                if(_slideState == SlideState.Opened || _slideState == SlideState.Opening)
                {
                    LeftAnimation.SetLeftProperty(from: OpenedLeft, to: OpenedLeft, ms: 0);
                    LeftAnimation.Begin();
                }
            }
        }

        // 새 템플릿이 적용될떄마다 동작하는 메소드
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _slider = (Border)GetTemplateChild("slider");
        }
        private double OpenedLeft => SliderLocation == SliderLocation.Left ? 0 : ParantActualWidth - SliderWidth;
        private double ClosedLeft => SliderLocation == SliderLocation.Left ? -SliderWidth : ParantActualWidth;
        private (double from, double to) GetLeftRange()
        {
            if (_slideState == SlideState.Opening)
            {
                return (from: ClosedLeft, to: OpenedLeft);
            }
            else
            {
                return (from: OpenedLeft, to: ClosedLeft);
            }
        }
        private void BeginAnimation()
        {
            var leftRange = GetLeftRange();
            SlideAnimation.SetLeftProperty(from: leftRange.from, to: leftRange.to, ms: AnimationSpeed);
            SlideAnimation.Begin();

        }
        public void Open()
        {
            if (_slideState != SlideState.Closed) return;
            _slideState = SlideState.Opening;
            BeginAnimation();
            this.BringToFront();
        }
        public void Close()
        {
            if (_slideState != SlideState.Opened) return;
            _slideState = SlideState.Closing;
            BeginAnimation();
            this.BringToFront();
        }

        private void Storyboard_Completed(object? sender, EventArgs e)
        {
            if (_slideState == SlideState.Opening)
            {
                _slideState = SlideState.Opened;
            }
            else
            {
                _slideState = SlideState.Closed;
                this.SendToBack();
            }

        }

        private void opacityGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
