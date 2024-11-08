using Indigator.Lib.GeometryUtil;

namespace Indigator.Lib
{
    /// <summary>
    /// Part Drawing 인터페이스, 파트의 모양을 패스 기반으로 그리기 위한 메서드
    /// </summary>
    public interface IPartDrawingContext
    {
        /// <summary>
        /// 시작점을 지정하여 Path를 그리기 시작한다
        /// </summary>
        /// <param name="point">시작점</param>
        void BeginPath(Point point);
        /// <summary>
        /// 특정 지점을 향해 선분을 그린다
        /// </summary>
        /// <param name="point">선분의 끝점</param>
        void DrawLine(Point point);
        /// <summary>
        /// 3차 베지어 곡선을 그린다
        /// </summary>
        /// <param name="control1">첫 번째 컨트롤 포인트</param>
        /// <param name="control2">두 번째 컨트롤 포인트</param>
        /// <param name="end">곡선의 끝점</param>
        void DrawCubicBezier(Point control1, Point control2, Point end);
        /// <summary>
        /// 2차 베지어 곡선을 그린다
        /// </summary>
        /// <param name="control">컨트롤 포인트</param>
        /// <param name="end">곡선의 끝점</param>
        void DrawQuadraticBezier(Point control, Point end);
        /// <summary>
        /// 파트를 그니다
        /// </summary>
        /// <param name="point">파트</param>
        void DrawPart(in Point point);
        /// <summary>
        /// Path를 닫는다
        /// </summary>
        void Close();
    }
}
