using Indigator.Lib.DigitalFonts;
using Indigator.Lib;
using Microsoft.AspNetCore.Components;
using Indigator.Lib.GeometryUtil;

namespace indigator.Razor
{
    /// <summary>
    /// 값을 문자열 형식으로 변환하여 디지털 문자들로 표시하는 인디케이터입니다.
    /// </summary>
    public partial class DigitalText : DigitalIndicator, IDigitalText
    {
        /// <inheritdoc/>
        [Parameter]
        public int Length { get; set; } = 10;
        /// <inheritdoc/>
        [Parameter]
        public string Format { get; set; }

        /// <summary>
        /// 생성자
        /// </summary>
        public DigitalText()
        {
            DigitalFont = new RoundedRectCell5x7Font();
        }

        /// <inheritdoc/>
        protected override Size Measure() => this.MeasureIndicator();
    }
}
