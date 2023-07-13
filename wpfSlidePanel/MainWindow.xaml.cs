using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using wpfSlidePanel.ViewModels;

namespace wpfSlidePanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Panel.SetZIndex(slidePanel, int.MaxValue);
            //var storyboard = (Storyboard)this.Resources["OpenStroyboard"];
            //storyboard.Begin();
            sliderPanel.Open();
        }

        private void opacityGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var storyboard = (Storyboard)this.Resources["CloseStroyboard"];
            // slide 이벤트가 끝날시 동작하는 이벤트 등록z
            storyboard.Completed += Storyboard_Completed;
            storyboard.Begin();
            //0.3초를 채우기전에 화면상에 보이지 않음            
        }

        private void Storyboard_Completed(object? sender, EventArgs e)
        {
            //Panel.SetZIndex(slidePanel, int.MinValue);
        }
    }
}
