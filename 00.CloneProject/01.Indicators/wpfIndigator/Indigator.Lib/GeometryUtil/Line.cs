namespace Indigator.Lib.GeometryUtil
{
    public struct Line
    {
        private Point start;
        private Point end;
        public Line(in Point point)
        {
            start = point;
            end = point;
        }
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="start">시작점</param>
        /// <param name="end">끝점</param>
        public Line(in Point start, in Point end)
        {
            this.start = start;
            this.end = end;
        }
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="startX">시작점 X 좌표</param>
        /// <param name="startY">시작점 Y 좌표</param>
        /// <param name="endX">끝점 X 좌표</param>
        /// <param name="endY">끝점 Y 좌표</param>
        public Line(in double startX, in double startY, in double endX, in double endY)
        {
            start = new Point(startX, startY);
            end = new Point(endX, endY);
        }
        /// <summary>
        /// 선분의 시작점을 가져오거나 설정합니다.
        /// </summary>
        public Point Start { get => start; set => start = value; }
        /// <summary>
        /// 선분의 끝점을 가져오거나 설정합니다.
        /// </summary>
        public Point End { get => end; set => end = value; }
        /// <summary>
        /// 시작점과 끝점의 X 좌표 차이를 가져옵니다.
        /// </summary>
        public double DeltaX => end.x - start.x;
        /// <summary>
        /// 시작점과 끝점의 Y 좌표 차이를 가져옵니다.
        /// </summary>
        public double DeltaY => end.y - start.y;
        /// <summary>
        /// 시작점을 기준으로 선분의 방향을 가져옵니다. 단위는 라디안입니다.
        /// </summary>
        public double Direction => Math.Atan2(DeltaY, DeltaX);
        /// <summary>
        /// 선분의 길이를 가져옵니다.
        /// </summary>
        public double Length => Math.Sqrt(Math.Pow(DeltaX, 2) + Math.Pow(DeltaY, 2));
        public void Transform(in Transform transform)
        {
            if (transform.IsIdentity)
                return;
            start.Tr
        }

    }
}
