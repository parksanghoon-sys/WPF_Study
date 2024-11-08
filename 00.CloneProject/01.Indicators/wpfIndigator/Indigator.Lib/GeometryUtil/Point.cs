using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigator.Lib.GeometryUtil
{
    public struct Point
    {
        internal double x;
        internal double y;
        public Point(in double xy)
        {
            x= xy;
            y= xy;
        }
        public Point(in double x, in double y)
        {
            this.x= x;
            this.y= y;
        }
        /// <summary>
        /// X 좌표를 가져오거나 설정
        /// </summary>
        public double X { get => x; set => x = value; }
        /// <summary>
        /// Y 좌표를 가져오거나 설정
        /// </summary>
        public double Y { get => y; set => y = value; }
        /// <summary>
        /// Point를 이동시킨다
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <returns></returns>
        public Point Offset(in double dx, in double dy) => new Point(dx, dy);
        /// <summary>
        /// X 축을 기준으로 미러링 한다
        /// </summary>
        /// <param name="x">미러링 기준 좌표</param>
        /// <returns>결과 포인트</returns>
        public Point MirrorX(in double x) => new Point(x * 2 - this.x, y);
        /// <summary>
        /// Y 축을 기준으로 미러링 한다
        /// </summary>
        /// <param name="y">미러링 기준 좌표</param>
        /// <returns>결과 포인트</returns>
        public Point MirrorY(in double y) => new Point(x, y * 2 - this.y);

        public Point MirrorXY(in double x, in double y) => new Point(x * 2 - this.x, y * 2 - this.y);
        /// <summary>
        /// 지정한 방향으로 지정한 거리만큼 포인트를 이동한다
        /// </summary>
        /// <param name="radians">방향각</param>
        /// <param name="length">이동거리</param>
        /// <returns>이동 결과 포인트</returns>
        public Point MoveByDirection(in double radians, in double length) => new Point(x + Math.Cos(radians) * length, y + Math.Sin(radians) * length);
        /// <summary>
        /// 지정한 방향으로 지정한 거리만큼 선을 생성
        /// </summary>
        /// <param name="radians">방향각</param>
        /// <param name="length">길이</param>
        /// <returns></returns>
        public Line LineByDirection(in double radians, in double length) => new Line(this,MoveByDirection(radians, length));
        /// <summary>
        /// 현재 점에서 지정한 끝점까지 선분을 생성
        /// </summary>
        /// <param name="end">끝 포인트</param>
        /// <returns>선분</returns>
        public Line LineTo(in Point end) => new Line(this, end);
        /// <summary>
        /// 지정한 시작점에서 현재점까지 선분 생성
        /// </summary>
        /// <param name="start">시작점</param>
        /// <returns>선분</returns>
        public Line LineFrom(in Point start) => new Line(start, this);
        /// <summary>
        /// 지저한 변환을 적용
        /// </summary>
        /// <param name="transform">변환 값</param>
        public void Transform(in Transform transform)
        {
            if (transform.IsIdentity)
                return;
            if(transform.translationOnly)
            {
                x += transform.m31;
                y += transform.m32;
            }
            else
            {
                x = x * transform.m11 + y * transform.m21 + transform.m31;
                y = y * transform.m12 + y * transform.m22 + transform.m32;
            }
        }
    }
}
