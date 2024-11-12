using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigator.Lib
{
    /// <summary>
    /// 대리자를 이용해 파트 드로잉을 정의
    /// </summary>
    public class DelegateDrawing : PartDrawing
    {
        private readonly Action<IPartDrawingContext> _drawingAction;
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="drawingAction">드로잉이 정의된 Action</param>
        public DelegateDrawing(Action<IPartDrawingContext> drawingAction)
        {
            _drawingAction = drawingAction;
        }
        /// <summary>
        /// Drawing 행위 체크
        /// </summary>
        public override bool IsEmpty => _drawingAction == null;
        /// <summary>
        /// Part Drawing Context에 그립니다.
        /// </summary>
        /// <param name="context">파트 드로잉 컨텍스트</param>
        protected override void OnDraw(IPartDrawingContext context) => _drawingAction?.Invoke(context);
    }
}
