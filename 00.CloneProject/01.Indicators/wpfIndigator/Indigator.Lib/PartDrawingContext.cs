using Indigator.Lib.GeometryUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigator.Lib
{
    /// <summary>
    /// 파트 드로잉 컨텍스트, 파트의 모양을 Path 기반으로 그리기 위한 메서드 제공
    /// </summary>
    /// <typeparam name="TRender">컨텍스트를 통해 패스 그리기를 실제로 수행할 렌더러의 형식</typeparam>
    /// <typeparam name="TRenderPoint">렌더러에서 사용될 포인트 형식</typeparam>
    public abstract class PartDrawingContext<TRender, TRenderPoint> : IPartDrawingContext
    {
        public void BeginPath(Point point)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void DrawCubicBezier(Point control1, Point control2, Point end)
        {
            throw new NotImplementedException();
        }

        public void DrawLine(Point point)
        {
            throw new NotImplementedException();
        }

        public void DrawPart(in Point point)
        {
            throw new NotImplementedException();
        }

        public void DrawQuadraticBezier(Point control, Point end)
        {
            throw new NotImplementedException();
        }
    }
}
