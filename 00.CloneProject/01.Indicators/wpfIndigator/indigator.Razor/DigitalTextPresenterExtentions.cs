using Indigator.Lib;

namespace indigator.Razor
{
    /// <summary>
    /// IDigitalTextPresenter 인터페이스에 대한 확장 메서드 모음입니다.
    /// </summary>
    public static class DigitalTextPresenterExtentions
    {
        /// <summary>
        /// IDigitalTextPresenter 객체를 그리기 위한 파트 목록을 가져옵니다.
        /// </summary>
        /// <param name="presenter">IDigitalTextPresenter 객체</param>
        /// <returns>인디케이터 파트 목록</returns>
        public static IEnumerable<Part> CreateParts(this IDigitalTextPresenter presenter) => presenter?.CreateParts(presenter.SegmentFilter);
    }
}
