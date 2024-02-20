using CirclularGage.Location.Local.Enums;
using CirclularGage.Main.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace CirclularGage.Main.Helpers
{
    public class IntruderModelMonitoring
    {
        private readonly IntruderModel _intruderModel;
        private readonly double _centerAdjustmentYAxis = 40.71126761;
        //private readonly double _centerAdjustmentYAxis = 0;
        private readonly double _airportSymbolSize = 30;
        private readonly double _bearingNotValidXAdjustmentXAxis = -19;
        private readonly double _IVSIGauageSize = 96;

        public double _intruderSymbolCenterAdjustX;
        public double _intruderSymbolCenterAdjustY;
        public IntruderModel IntruderModel
        {
            get => _intruderModel;
        }        
        public IntruderModelMonitoring(IntruderModel intruderModel)
        {
            _intruderModel = intruderModel;            
        }
        public Point MoniterPoint => CalculateIntruderPoint();

        private Point CalculateIntruderPoint()
        {
            Point point = new Point();
            var angle = _intruderModel.Bearing - 90;
            var radianAngle = (angle * Math.PI) / 180;     
            var yPoint = _centerAdjustmentYAxis + _intruderModel.Range * Math.Sin(radianAngle) * TcasRangeDisplayRatitoY(_intruderModel.TcasDisplayRange);
            yPoint += _intruderSymbolCenterAdjustY;
            point.Y = yPoint;

            var xPoint = Math.Cos(radianAngle);
            xPoint *= _intruderModel.Range;
            xPoint *= TcasRangeDisplayRatitoX(_intruderModel.TcasDisplayRange);
            xPoint += _intruderSymbolCenterAdjustX;

            point.X = xPoint;
            if (IsOverCheckIVSI(point) == true)
                point = OverPointIVSI(point);
            return point;
        }
        private double TcasRangeDisplayRatitoY(TcasDisplayRange tcasDisplayRange)
        {
            double ratito;
            switch (tcasDisplayRange)
            {
                case TcasDisplayRange.Rate5:
                    ratito = 27.04225352;
                    break;
                case TcasDisplayRange.Rate10:
                    ratito = 13.632;
                    break;
                case TcasDisplayRange.Rate20:
                    ratito = 6.76056338;
                    break;
                case TcasDisplayRange.Rate40:
                    ratito = 3.38028169;
                    break;
                default:
                    ratito = 1;
                    break;
            }
            return ratito;
        }
        private double TcasRangeDisplayRatitoX(TcasDisplayRange tcasDisplayRange)
        {
            double ratito;
            switch (tcasDisplayRange)
            {
                case TcasDisplayRange.Rate5:
                    ratito = 25.03626185;
                    break;
                case TcasDisplayRange.Rate10:
                    ratito = 13.71428571;
                    break;
                case TcasDisplayRange.Rate20:
                    ratito = 6.857142857;
                    break;
                case TcasDisplayRange.Rate40:
                    ratito = 3.428571429;
                    break;
                default:
                    ratito = 1;
                    break;
            }
            return ratito;
        }
        private bool IsOverCheckIVSI(Point point)
        {
            var xdata = Math.Pow(point.X, 2);
            var ydata = Math.Pow(point.Y, 2);
            var range = Math.Pow(_IVSIGauageSize, 2);
            if (xdata + ydata > range)
                return true;
            return false;
        }
        private Point OverPointIVSI(Point point2)
        {
            Point point = new Point();

            var pointX = point2.X;
            var pointY = point2.Y;
            var centerX = 0;
            var centerY = _centerAdjustmentYAxis;
       

            // 주어진 점과 원의 교차점을 계산
            double dx = pointX - centerX;
            double dy = pointY - centerY;
            // dx 와 dy의 유클리드 거리를 계산
            // 원의 중심과 주어진점 사이의 거리
            double dr = Math.Sqrt(dx * dx + dy * dy);
            // 주어진 점과 원의 중심을 지나는 직선 방정식을 찾기위해 사용한다 
            // D는 직선의 방정식에서의 상수 항을 나타낸다
            double D = centerX * pointY - pointX * centerY;
            // 두직선이 만나는 지점의 존재여부를 판단하기위함 0보다 크면 교차점이 있고, 작으면 교차점이 없다.
            double discriminant = _IVSIGauageSize * _IVSIGauageSize * dr * dr - D * D;
            double x1 = (D * dy + Math.Sign(dy) * dx * Math.Sqrt(discriminant)) / (dr * dr);
            double y1 = (-D * dx + Math.Abs(dy) * Math.Sqrt(discriminant)) / (dr * dr);
            double x2 = (D * dy - Math.Sign(dy) * dx * Math.Sqrt(discriminant)) / (dr * dr);
            double y2 = (-D * dx - Math.Abs(dy) * Math.Sqrt(discriminant)) / (dr * dr);

            var m = pointX / pointY;
            var a1 =Math.Sqrt(Math.Pow(_IVSIGauageSize, 2) - Math.Pow(pointX * m + _centerAdjustmentYAxis, 2));

            point.X = point2.X < 0 ? x1 < 0 ? x1 : x2 : x1 > 0 ? x1 : x2;
            point.Y = point2.Y < 0 ? y1 < 0 ? y1 : y2 : y1 > 0 ? y1 : y2;
            //point.X -= _intruderSymbolCenterAdjustX;
            //point.Y -= _intruderSymbolCenterAdjustY;
            return point;
        }
    }
//    import math

//# 주어진 값
//radius = 100  # 원의 반지름
//center_x = 0  # 원의 중심 x 좌표
//center_y = -35  # 원의 중심 y 좌표
//angle_degrees = 45  # 각도 (도)

//# 각도를 라디안으로 변환
//angle_radians = math.radians(angle_degrees)

//# 주어진 각도와 거리로부터 점의 좌표 계산
//x = center_x + radius * math.cos(angle_radians)
//y = center_y + radius* math.sin(angle_radians)

//print("계산된 점의 좌표:", x, y)

//# 원의 방정식: (x - center_x)^2 + (y - center_y)^2 = radius^2
//# 직선의 방정식: y = mx + c
//# 두 직선을 연립하여 x, y를 구하는 과정이 필요함
//# 우선 직선의 기울기 m을 계산
//m = math.tan(angle_radians)

//# 직선의 y절편 c 계산
//c = y - m * x

//# 연립방정식을 풀어 교차점 계산
//a = 1 + m**2
//b = 2 * (m* c - m* center_y - center_x)
//c = center_x**2 + (c - center_y)**2 - radius**2

//# 교차점의 x 좌표를 구함
//x_intersect1 = (-b + math.sqrt(b**2 - 4 * a * c)) / (2 * a)
//x_intersect2 = (-b - math.sqrt(b**2 - 4 * a * c)) / (2 * a)

//# 교차점의 y 좌표를 구함
//y_intersect1 = m* x_intersect1 + c
//y_intersect2 = m * x_intersect2 + c

//print("원의 가장자리와의 교차점 1:", x_intersect1, y_intersect1)
//print("원의 가장자리와의 교차점 2:", x_intersect2, y_intersect2)

}
