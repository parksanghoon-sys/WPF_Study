using Indigator.Lib;
using Indigator.Lib.DigitalFonts;

namespace Indigator.Lib
{
    /// <summary>
    /// 표시할 세그먼트에 대한 필터에 따라 디저털 문자를 표시한다.
    /// </summary>
    public interface IDigitalIndigatorPresenter : IDigitalIndicator
    {
        DigitalSegmentFilter SegmentFilter { get; set; }

    }
}