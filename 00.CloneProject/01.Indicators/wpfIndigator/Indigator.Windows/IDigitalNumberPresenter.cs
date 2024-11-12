using Indigator.Lib;

namespace Indigator.Windows
{
    public interface IDigitalNumberPresenter : IDigitalNumber, IDigitalIndigatorPresenter
    {
    }
    public static class DigitalNumberPresenterExtentions
    {
        /// <summary>
        /// IDigitalNumberPresenter 객체를 기리기 위한 파트 목록을 가져온다
        /// </summary>
        /// <param name="presenter"></param>
        /// <returns></returns>
        public static IEnumerable<Part> CreateParts(this IDigitalNumberPresenter presenter)
            => presenter?.CreateParts(presenter.SegmentFilter);
    }
}