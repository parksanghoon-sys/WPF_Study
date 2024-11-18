using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigator.Lib
{
    /// <summary>
    /// 수치형 값을 디지털 문자들로 표시하는 인디케이터입니다.
    /// </summary>
    public interface IDigitalNumber : IDigitalIndicator
    {
        /// <summary>
        /// 표시할 정수 자리 개수을 가져오거나 설정합니다.
        /// </summary>
        int IntegerDigits { get; set; }
        /// <summary>
        /// 표시할 소수 자리 개수를 가져오거나 설정합니다.
        /// </summary>
        int DecimalPlaces { get; set; }
        /// <summary>
        /// 디지털 문자 세로 크기 대비 소수점 크기 비율을 가져오거나 설정합니다.
        /// </summary>
        double DecimalSeparatorSize { get; set; }
        /// <summary>
        /// 디지털 문자 세로 크기 대비 소수 자리 문자의 크기 비율을 가져오거나 설정합니다.
        /// </summary>
        double DecimalPlaceScale { get; set; }
        /// <summary>
        /// 왼쪽(정수 자리) 빈칸을 0으로 채울지 여부를 가져오거나 설정합니다.
        /// </summary>
        bool PadZeroLeft { get; set; }
        /// <summary>
        /// 오른쪽(소수 자리) 빈칸을 0으로 채울지 여부를 가져오거나 설정합니다.
        /// </summary>
        bool PadZeroRight { get; set; }
        /// <summary>
        /// 마이너스 기호를 왼쪽 끝에 정렬할지 여부를 가져오거나 설정합니다.
        /// </summary>
        bool MinusAlignLeft { get; set; }
    }

}
