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
        /// <summary>
        /// 비어 있는 파트 드로잉을 가져옵니다.
        /// </summary>
        public static PartDrawing Empty { get; }
        static PartDrawing()
        {
            Empty = new EmptyPartDrawing();
        }
        class EmptyPartDrawing : PartDrawing
        {
            public override bool IsEmpty => true;

            protected override void OnDraw(IPartDrawingContext context)
            { }
        }
        /// <summary>
        /// Draw 행위가 없는 여부를 가져온다
        /// </summary>
        public abstract bool IsEmpty { get; }
        internal void DrawTo(IPartDrawingContext context) => OnDraw(context);
        /// <summary>
        /// 파트 드로잉 컨텍스트에 그린다.
        /// </summary>
        /// <param name="context"></param>
        protected abstract void OnDraw(IPartDrawingContext context);        
    }
}
