using Indigator.Lib;
using System.Windows;
using System.Windows.Media;

namespace Indigator.Windows
{
    public class PartDrawingContext : PartDrawingContext<StreamGeometryContext, Point>
    {
        protected override void OnBeginPath(in Point startPoint)
        {
            Renderer.BeginFigure(startPoint, true, true);
        }

        protected override void OnClosePath()
        {
            
        }

        protected override Point OnConvertToRendererPoint(in Indigator.Lib.GeometryUtil.Point point)
        {
            return new Point(point.X, point.Y);
        }

        protected override void OnDrawCubicBezier(in Point controlPoint1, in Point controlPoint2, in Point endPoint)
        {
            Renderer.BezierTo(controlPoint1, controlPoint2, endPoint, true, true);
        }

        protected override void OnDrawLine(in Point endpoint)
        {
            Renderer.LineTo(endpoint, true, true);
        }

        protected override void OnDrawQuadraticBezier(in Point controlPoint, in Point endPoint)
        {
            Renderer.QuadraticBezierTo(controlPoint, endPoint, true, true);
        }
    }
}
