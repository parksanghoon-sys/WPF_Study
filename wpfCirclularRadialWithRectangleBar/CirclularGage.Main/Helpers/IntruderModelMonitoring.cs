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
        private readonly double _centerAdjustmentYAxis = 40.02816901;
        //private readonly double _centerAdjustmentYAxis = 0;
        private readonly double _airportSymbolSize = 30;
        private readonly double _bearingNotValidXAdjustmentXAxis = -19;
        private readonly double _IVSIGauageSize = 98;

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

            var xPoint = Math.Cos(radianAngle);
            xPoint *= _intruderModel.Range;
            xPoint *= TcasRangeDisplayRatitoX(_intruderModel.TcasDisplayRange);
            point.Y = yPoint;
            point.X = xPoint;

            if (IsOverCheckIVSI(point) == true && (_intruderModel.IntruderType == TcasIntruderSymbol.ResolutionAdvisorty 
                                                    || _intruderModel.IntruderType == TcasIntruderSymbol.TrafficAdvisory))

                point = OverIntruderIVSIPoint2(point);

            point.Y += _intruderSymbolCenterAdjustY;
            point.X += _intruderSymbolCenterAdjustX;

            return point;
        }
        private double TcasRangeDisplayRatitoY(TcasDisplayRange tcasDisplayRange)
        {
            double ratito;
            switch (tcasDisplayRange)
            {
                case TcasDisplayRange.Rate5:
                    ratito = 27.6056338;
                    break;
                case TcasDisplayRange.Rate10:
                    ratito = 13.8028169;
                    break;
                case TcasDisplayRange.Rate20:
                    ratito = 6.901408451;
                    break;
                case TcasDisplayRange.Rate40:
                    ratito = 3.450704225;
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
                    ratito = 25.55785064;
                    break;
                case TcasDisplayRange.Rate10:
                    ratito = 14;
                    break;
                case TcasDisplayRange.Rate20:
                    ratito = 7;
                    break;
                case TcasDisplayRange.Rate40:
                    ratito = 3.5;
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

            point.X = FindColsesetNumber(pointX, x1, x2);
            point.Y = FindColsesetNumber(pointY, y1, y2);

            return point;
        }
        private Point OverIntruderIVSIPoint(Point point)
        {
            Point resultPoint = new Point();
            var pointX = point.X;
            var pointY = point.Y;
            var centerX = 0;
            var centerY = _centerAdjustmentYAxis;

            var angle = _intruderModel.Bearing - 90;
            var radianAngle = (angle * Math.PI) / 180;
            //double edgeX = centerX + vx * _IVSIGauageSize;
            //double edgeY = centerY + vy * _IVSIGauageSize;
            // 주어진 포인트와 중심 사이의 각도 계산
            double angleRadians = Math.Atan2(pointY - centerY, pointX - centerX);
            // 좌표 계산
            double edgeX = centerX + _IVSIGauageSize * Math.Cos(angleRadians);
            double edgeY = _IVSIGauageSize * Math.Sin(angleRadians);

            resultPoint.X = edgeX ;
            resultPoint.Y = edgeY ;
            return resultPoint;
        }
        private Point OverIntruderIVSIPoint2(Point point)
        {
            Point resultPoint = new Point();
            var pointX = point.X;
            var pointY = point.Y;
            var centerX = 0;
            // 90 도 일때 기울기 0 방지코드
            var centerY = _centerAdjustmentYAxis * 0.99;

            var m = (pointY - centerY) / (pointX - centerX);
            var B = centerY;

            var a = 1 + Math.Pow(m, 2);
            var b = 2 * B * m;
            var c = Math.Pow(B,2) - Math.Pow(_IVSIGauageSize,2);

            var x1 = (-b + Math.Sqrt(Math.Pow(b, 2) - (4 * a * c)))/ (2 * a);
            var x2 = (-b - Math.Sqrt(Math.Pow(b, 2) - (4 * a * c)))/ (2 * a);

            m =  (pointX - centerX) / (pointY - centerY);
            B = -1 * centerY * m;
            a = 1 + Math.Pow(m, 2);
            b = 2 * B * m;
            c = Math.Pow(B, 2) - Math.Pow(_IVSIGauageSize, 2);

            var y1 = (-b + Math.Sqrt(Math.Pow(b, 2) - (4 * a * c)))/(2 * a);
            var y2 = (-b - Math.Sqrt(Math.Pow(b, 2) - (4 * a * c)))/ (2 * a);

            resultPoint.X = FindColsesetNumber(pointX, x1, x2);
            resultPoint.Y = FindColsesetNumber(pointY, y1, y2);

            return resultPoint;
        }
        private double FindColsesetNumber(double baseNumber, double data1, double data2)
        {
            double diff1 = Math.Abs(baseNumber - data1);
            double diff2 = Math.Abs(baseNumber - data2);

            if (diff1 > diff2)
                return data2;
            else if(diff1 < diff2)
                return data1;
            return data1;
        }
    }

}
