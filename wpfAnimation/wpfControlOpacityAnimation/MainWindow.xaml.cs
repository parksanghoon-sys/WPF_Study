using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace wpfControlOpacityAnimation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void OnAnimationCompleted(object sender, EventArgs e)
        {
            var storyboard = new Storyboard();

            // X축으로의 움직임 복구
            var animationX = new DoubleAnimation
            {
                To = 10,
                Duration = TimeSpan.FromSeconds(0)
            };
            Storyboard.SetTarget(animationX, MyButton);
            Storyboard.SetTargetProperty(animationX, new PropertyPath("(Canvas.Left)"));
            storyboard.Children.Add(animationX);

            // Y축으로의 움직임 복구
            var animationY = new DoubleAnimation
            {
                To = 100,
                Duration = TimeSpan.FromSeconds(0)
            };
            Storyboard.SetTarget(animationY, MyButton);
            Storyboard.SetTargetProperty(animationY, new PropertyPath("(Canvas.Top)"));
            storyboard.Children.Add(animationY);

            // 투명도 복구 애니메이션
            var animationOpacity = new DoubleAnimation
            {
                To = 1.0,
                Duration = TimeSpan.FromSeconds(2)
            };
            Storyboard.SetTarget(animationOpacity, MyButton);
            Storyboard.SetTargetProperty(animationOpacity, new PropertyPath("Opacity"));
            storyboard.Children.Add(animationOpacity);

            storyboard.Begin();
        }
    }
}
