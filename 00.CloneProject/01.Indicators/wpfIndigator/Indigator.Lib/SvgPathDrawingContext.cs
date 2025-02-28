﻿using Indigator.Lib.GeometryUtil;
using System.Text;

namespace Indigator.Lib
{
    /// <summary>
    /// 파트 드로잉 컨텍스를 통해 SVG 패스 태그 구문을 생성합니다.
    /// </summary>
    public class SvgPathDrawingContext : PartDrawingContext<StringBuilder>
    {
        private const char space = ' ';

        /// <summary>
        /// 시작 포인트를 지정하여 렌더러로 패스를 그리기를 시작합니다.
        /// </summary>
        /// <param name="startPoint">시작 포인트</param>
        protected override void OnBeginPath(in Point startPoint)
            => Renderer.Append('M')
            .Append(space).Append(startPoint.X)
            .Append(space).Append(startPoint.Y);

        /// <summary>
        /// 렌더러를 이용하여 특정 포인트를 향해 선분을 그립니다.
        /// </summary>
        /// <param name="endPoint">선분의 끝 포인트</param>
        protected override void OnDrawLine(in Point endPoint)
            => Renderer.Append('L')
            .Append(space).Append(endPoint.X)
            .Append(space).Append(endPoint.Y);

        /// <summary>
        /// 렌더러를 이용하여 3차 베지어 곡선을 그립니다.
        /// </summary>
        /// <param name="controlPoint1">첫 번째 컨트롤 포인트</param>
        /// <param name="controlPoint2">두 번째 컨트롤 포인트</param>
        /// <param name="endPoint">곡선의 끝 포인트</param>
        protected override void OnDrawCubicBezier(in Point controlPoint1, in Point controlPoint2, in Point endPoint)
            => Renderer.Append('C')
            .Append(space).Append(controlPoint1.X)
            .Append(space).Append(controlPoint1.Y)
            .Append(space).Append(controlPoint2.X)
            .Append(space).Append(controlPoint2.Y)
            .Append(space).Append(endPoint.X)
            .Append(space).Append(endPoint.Y);

        /// <summary>
        /// 렌더러를 이용하여 2차 베지어 곡선을 그립니다.
        /// </summary>
        /// <param name="controlPoint">컨트롤 포인트</param>
        /// <param name="endPoint">곡선의 끝 포인트</param>
        protected override void OnDrawQuadraticBezier(in Point controlPoint, in Point endPoint)
            => Renderer.Append('Q')
            .Append(space).Append(controlPoint.X)
            .Append(space).Append(controlPoint.Y)
            .Append(space).Append(endPoint.X)
            .Append(space).Append(endPoint.Y);

        /// <summary>
        /// 렌더러에서 패스 닫기 작업을 수행합니다.
        /// </summary>
        protected override void OnClosePath() => Renderer.Append('Z');
    }
}