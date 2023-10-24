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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
