using Indigator.Lib.DigitalFonts;
using System.Xaml;

namespace Indigator.Windows
{
    /// <summary>
    /// 값을 여러 세그먼트로 구성된 디지털 무자들로 표시하는 인디게이터이다
    /// </summary>
    public interface IDigitalIndicator : IIndicator
    {
        /// <summary>
        /// 디지털 문자 양식을 가져오거나 설정
        /// </summary>
        DigitalFont DigitalFont { get; set; }
        /// <summary>
        /// 디지털 문자의 가로 크기 대비 문자간 거리 비율을 가져오거나 사용
        /// </summary>
        double Spacing { get; set; }
    }
}