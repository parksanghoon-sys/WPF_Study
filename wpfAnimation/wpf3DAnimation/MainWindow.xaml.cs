using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpf3DAnimation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point lastMousePosition;

        public MainWindow()
        {
            InitializeComponent();
        }

        // 마우스 클릭 이벤트 처리기
        private void MainGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // 마우스 클릭 위치 얻기
            Point mousePos = e.GetPosition(mainGrid);

            // Viewport에서의 상대적 위치 계산
            double xMove = (mousePos.X - (mainGrid.ActualWidth / 2)) / (mainGrid.ActualWidth / 2);
            double yMove = -(mousePos.Y - (mainGrid.ActualHeight / 2)) / (mainGrid.ActualHeight / 2);

            // 객체를 새로운 위치로 이동
            translateTransform.OffsetX = xMove * 2;  // X축 이동
            translateTransform.OffsetY = yMove * 2;  // Y축 이동
        }

        // 마우스 휠 이벤트 처리기
        private void MainGrid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double scale = e.Delta > 0 ? 1.1 : 0.9;

            // 확대/축소 적용
            scaleTransform.ScaleX *= scale;
            scaleTransform.ScaleY *= scale;
            scaleTransform.ScaleZ *= scale;
        }
    }
}