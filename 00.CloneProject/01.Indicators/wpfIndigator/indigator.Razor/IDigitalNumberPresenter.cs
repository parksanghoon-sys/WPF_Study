using Indigator.Lib;

namespace indigator.Razor
{
    /// <summary>
    /// 세그먼트 상태에 따라 수치형 값을 디지털 문자들을표시
    /// </summary>
    public interface IDigitalNumberPresenter : IDigitalNumber, IDigitalIndigatorPresenter
    {
    }
    public static class DigitalNumerPresenterExtentions
    {
        /// <summary>
        /// IDigitalNumberPresenter 객체를 그리기 위한 파트 목록을 가져옵니다.
        /// </summary>
        /// <param name="presenter">IDigitalNumberPresenter 객체</param>
        /// <returns>인디케이터 파트 목록</returns>
        public static IEnumerable<Part> CreateParts(this IDigitalNumberPresenter presenter) => presenter?.CreateParts(presenter.SegmentFilter);
    }
}