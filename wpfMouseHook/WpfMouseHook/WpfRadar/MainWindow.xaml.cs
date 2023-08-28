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
using System.Windows.Threading;

namespace WpfRadar
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        // 네 각의 GPS 정보를 저장할 클래스 정의
        public class GpsPoint
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        private GpsPoint _tlCorner;
        private GpsPoint _trCorner;
        private GpsPoint _blCorner;
        private GpsPoint _brCorner;

        private const int RadarWidth = 750;
        private const int RadarHeight = 400;
        private DispatcherTimer _inactivityTimer;
        public MainWindow()
        {
            InitializeComponent();

            // 레이더 네 각의 GPS 정보를 초기화
            _tlCorner = new GpsPoint { Latitude = 37.0000, Longitude = 126.0000 };
            _trCorner = new GpsPoint { Latitude = 37.0000, Longitude = 126.0100 };
            _blCorner = new GpsPoint { Latitude = 37.0100, Longitude = 126.0000 };
            _brCorner = new GpsPoint { Latitude = 37.0100, Longitude = 126.0100 };

            // 예제 GPS 데이터 위치
            var sampleData = new GpsPoint { Latitude = 37.0040, Longitude = 126.0050 };

            _inactivityTimer = new DispatcherTimer();
            _inactivityTimer.Interval = TimeSpan.FromMinutes(1);
            //_inactivityTimer.Tick += CheckInactivity;
            _inactivityTimer.Start();

            // 레이더 뷰에 빨간 점으로 표시
            DrawGpsPointOnRadar(sampleData);
        }

        private void DrawGpsPointOnRadar(GpsPoint gpsData)
        {
            // GPS 데이터를 X, Y 좌표로 변환
            var gpsPoint = GpsToRadarCoordinates(gpsData);

            // 캔버스에 빨간 점 그리기
            Ellipse point = new Ellipse
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.Red
            };
            Canvas.SetLeft(point, gpsPoint.X - 5);
            Canvas.SetTop(point, gpsPoint.Y - 5);
            RadarView.Children.Add(point);
        }

        private Point GpsToRadarCoordinates(GpsPoint gpsData)
        {
            Point point = new Point();
            double xFactor = (gpsData.Longitude - _tlCorner.Longitude) / (_trCorner.Longitude - _tlCorner.Longitude);
            double yFactor = (_tlCorner.Latitude - gpsData.Latitude) / (_tlCorner.Latitude - _blCorner.Latitude);

            double x = RadarWidth * xFactor;
            double y = RadarHeight * yFactor;
            
            point.X = x;
            point.Y = y;
            return point;
        }
    }
}
