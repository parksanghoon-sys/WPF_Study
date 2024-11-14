using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigator.Lib.PartBuilders
{
    /// <summary>
    /// 파라미터 기반 파트 드로잉 빌더이다, 파라미터에 따라 파트 드로잉이 캐시에 보관된다.
    /// </summary>
    public abstract class ParametricPartBuilder : ParametricCache<PartDrawing>
    {
        /// <summary>
        /// 파드 드로잉을 생성하거나 캐시에서 가져온다
        /// </summary>
        /// <returns>파트 드로잉</returns>
        public PartDrawing BuildPartDrawing() => GetObject();
    }
}
