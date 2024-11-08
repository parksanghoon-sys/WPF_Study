using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigator.Lib
{
    /// <summary>
    /// Part 드로잉 정의
    /// </summary>
    public abstract class PartDrawing
    {
        public static PartDrawing Empty { get; }
        static PartDrawing()
        {
            Empty = new PartDrawing();
        }
        class EmptyPartDrawing : PartDrawing
        {

        }
        public abstract bool IsEmpty { get; }
        internal void DrawTo()
    }
    /// <summary>
    /// Part Drawing 인터페이스, 파트의 모양을 패스 기반으로 그리기 위한 메서드
    /// </summary>
    public interface IPartDrawingContext
    {
        void BeginPath(Point)
    }
}
