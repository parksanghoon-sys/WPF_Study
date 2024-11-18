using Indigator.Lib;
using Indigator.Lib.DigitalFonts;
using Indigator.Lib.GeometryUtil;
using Microsoft.AspNetCore.Components;

namespace indigator.Razor
{
    /// <summary>
    /// 세그먼트 상태에 따라 수치형 값을 디지털 문자들을 표시합니다.
    /// </summary>
    public class DigitalNumberPresenter : DigitalIndicatorPresenter, IDigitalNumberPresenter
    {
        /// <inheritdoc/>
        [Parameter]
        public int IntegerDigits { get; set; }
        /// <inheritdoc/>
        [Parameter]
        public int DecimalPlaces { get; set; }
        /// <inheritdoc/>
        [Parameter]
        public double DecimalSeparatorSize { get; set; }
        /// <inheritdoc/>
        [Parameter]
        public double DecimalPlaceScale { get; set; }
        /// <inheritdoc/>
        [Parameter]
        public bool PadZeroLeft { get; set; }
        /// <inheritdoc/>
        [Parameter]
        public bool PadZeroRight { get; set; }
        /// <inheritdoc/>
        [Parameter]
        public bool MinusAlignLeft { get; set; }

        /// <inheritdoc/>
        protected override Size Measure() => this.MeasureIndicator();
        /// <inheritdoc/>
        protected override IEnumerable<Part> OnCreateParts() => this.CreateParts();
    }
}
