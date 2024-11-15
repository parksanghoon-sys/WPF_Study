using Indigator.Lib;

namespace Indigator.Windows
{
    public static class DigitalTextPresenterExtentions
    {
        /// <summary>
        /// IDigitalTextPresenter 객체를 그리기 위한 파트의 목록을 가져옴
        /// </summary>
        /// <param name="presenter"></param>
        /// <returns></returns>
        public static IEnumerable<Part> CreateParts(this IDigitalTextPresenter presenter) => presenter?.CreateParts(presenter.SegmentFilter);
    }
}
