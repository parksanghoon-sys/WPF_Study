using Indigator.Lib.GeometryUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Indigator.Lib
{
    /// <summary>
    /// 파트 드로잉 컨텍스트, 파트의 모양을 Path 기반으로 그리기 위한 메서드 제공
    /// </summary>
    /// <typeparam name="TRenderer">컨텍스트를 통해 패스 그리기를 실제로 수행할 렌더러의 형식</typeparam>
    /// <typeparam name="TRenderPoint">렌더러에서 사용될 포인트 형식</typeparam>
    public abstract class PartDrawingContext<TRenderer, TRenderPoint> : IPartDrawingContext
    {
        /// <summary>
        /// 컨텍스트를 통해 패스 그리기를 실제로 수행할 렌더러를 가져오거나 설정한다.
        /// </summary>
        public TRenderer Renderer { get; set; }
        private Transform currentTransform = Transform.Identity;
        /// <summary>
        /// 시작점을 지정하여 페스를 그린다
        /// </summary>
        /// <param name="point">시작점</param>
        public void BeginPath(Point point)
        {
            if(currentTransform.IsIdentity == false)
            {
                point.Transform(currentTransform);
            }
            OnBeginPath(OnConvertToRendererPoint(point));
        }
        /// <summary>
        /// 패스를 닫는다
        /// </summary>
        public void Close()
        {
            OnClosePath();
        }
        /// <summary>
        /// 3차 베지어 곡선을 그립니다.
        /// </summary>
        /// <param name="control1">첫 번째 컨트롤 포인트</param>
        /// <param name="control2">두 번째 컨트롤 포인트</param>
        /// <param name="end">곡선의 끝점</param>
        public void DrawCubicBezier(Point control1, Point control2, Point end)
        {
            if(currentTransform.IsIdentity == false)
            {
                control1.Transform(currentTransform);
                control2.Transform(currentTransform);
                end.Transform(currentTransform);
            }
            OnDrawCubicBezier(OnConvertToRendererPoint(control1), OnConvertToRendererPoint(control2), OnConvertToRendererPoint(end));
        }
        /// <summary>
        /// 특정 지점을 향해 선분을 그립니다.
        /// </summary>
        /// <param name="endPoint">선분의 끝점</param>
        public void DrawLine(Point endPoint)
        {
            if (!currentTransform.IsIdentity)
                endPoint.Transform(currentTransform);
            OnDrawLine(OnConvertToRendererPoint(endPoint));
        }

        public virtual void DrawPart(in Part part)
        {
            var temp = currentTransform;
            currentTransform = part.Transform;
            part.Drawing.DrawTo(this);
            currentTransform = temp;
        }
        /// <summary>
        /// 2차 베지어 곡선을 그립니다.
        /// </summary>
        /// <param name="control">컨트롤 포인트</param>
        /// <param name="end">곡선의 끝점</param>
        public void DrawQuadraticBezier(Point control, Point end)
        {
            if (!currentTransform.IsIdentity)
            {
                control.Transform(currentTransform);
                end.Transform(currentTransform);
            }
            OnDrawQuadraticBezier(OnConvertToRendererPoint(control), OnConvertToRendererPoint(end));
        }
        /// <summary>
        /// 시작점을 지정하여 렌더러로 Path를 그리기시작
        /// </summary>
        /// <param name="startPoint">시작점</param>
        protected abstract void OnBeginPath(in TRenderPoint startPoint);
        /// <summary>
        /// 렌더러를 이용, 특정 지점을 향해 선분을 그린다.
        /// </summary>
        /// <param name="endpoint">선부느이 끝점</param>
        protected abstract void OnDrawLine(in TRenderPoint endpoint);
        /// <summary>
        /// 렌더러를 이용해, 3차 베지어 곡선을 그린다
        /// </summary>
        /// <param name="controlPoint1">첫 번째 컨트롤 포인트</param>
        /// <param name="controlPoint2">두 번째 컨트롤 포인트</param>
        /// <param name="endPoint">끝 점</param>
        protected abstract void OnDrawCubicBezier(in TRenderPoint controlPoint1, in TRenderPoint controlPoint2, in TRenderPoint endPoint);
        /// <summary>
        /// 렌더러를 이용 2차 베지어 곡선을 그린다
        /// </summary>
        /// <param name="controlPoint">컨트롤 포인트</param>
        /// <param name="endPoint">곡선의 끝점</param>
        protected abstract void OnDrawQuadraticBezier(in TRenderPoint controlPoint, in TRenderPoint endPoint);
        /// <summary>
        /// 렌더러에서 패스 닫기 작업을 수행
        /// </summary>
        protected abstract void OnClosePath();
        /// <summary>
        /// Point 형식을 렌더러에서 사용하는 포인터의 형식으로 변환한다.
        /// </summary>
        /// <param name="point">변환할 Point 값</param>
        /// <returns>변환된 포인트</returns>
        protected abstract TRenderPoint OnConvertToRendererPoint(in Point point);
    }
    /// <summary>
    /// 파트 드로잉 컨텍스트. 파트의 모양을 패스 기반으로 그리기 위한 메서드를 제공합니다.
    /// </summary>
    /// <typeparam name="TRenderer">컨텍스트를 통한 패스 그리기를 실제로 수행할 렌더러의 형식입니다.</typeparam>
    public abstract class PartDrawingContext<TRenderer> : PartDrawingContext<TRenderer, Point>
    {
        /// <summary>
        /// VagabondK.Indicators.GeometryUtil.Point 형식을 렌더러에서 사용하는 포인트 형식으로 변환합니다.
        /// </summary>
        /// <param name="point">변환할 Point 값</param>
        /// <returns>변환된 포인트</returns>
        protected override Point OnConvertToRendererPoint(in Point point) => point;
    }
}
