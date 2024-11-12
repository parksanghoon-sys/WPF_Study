namespace Indigator.Windows
{
    /// <summary>
    /// 값을 화면에 표시하는 인디케이터를 정의한다.
    /// </summary>
    public interface IIndicator
    {
        /// <summary>
        /// 화면에 표시할 값을 가져오거나 설정
        /// </summary>
        object Value { get; set; }
    }
}