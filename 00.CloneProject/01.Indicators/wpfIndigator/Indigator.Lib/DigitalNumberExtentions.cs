using Indigator.Lib.DigitalFonts;
using Indigator.Lib.GeometryUtil;

namespace Indigator.Lib
{
    /// <summary>
    /// IDigitalNumber 인터페이스에 대한 확장 메서드 모음입니다.
    /// </summary>
    public static class DigitalNumberExtentions
    {

        /// <summary>
        /// IDigitalNumber의 크기를 측정합니다.
        /// </summary>
        /// <param name="indicator">IDigitalNumber 객체</param>
        /// <returns>크기</returns>
        public static Size MeasureIndicator(this IDigitalNumber indicator)
        {
            if (indicator == null) return default;

            var characterStyle = indicator.DigitalFont;

            var charHeight = (characterStyle?.ActualSize ?? default).Height;
            var charWidth = characterStyle?.Width ?? 0d;
            var slantAngle = Math.Max(Math.Min((characterStyle?.SlantAngle ?? 0d).ToValidValue(), 45), 0);
            var integerDigitCount = Math.Min(Math.Max(indicator.IntegerDigits, 0), 29);
            var decimalPlaceCount = Math.Min(Math.Max(indicator.DecimalPlaces, 0), 28);
            var decimalPlaceScale = Math.Min(indicator.DecimalPlaceScale.ToValidValue(1d), 1d);
            var separatorSize = charHeight * Math.Min(indicator.DecimalSeparatorSize.ToValidValue(), 1);
            var spacing = indicator.Spacing.ToValidValue() * charWidth;
            var strokeThickness = 0d;
            var slantWidth = Math.Tan(slantAngle / 180 * Math.PI) * (charHeight + strokeThickness);

            var integerDigitsSizeWidth = (charWidth + strokeThickness) * integerDigitCount + spacing * (integerDigitCount + 1);
            var integerDigitsSizeHeight = charHeight + spacing * 2 + strokeThickness;

            var withoutDecimalPlaceWidth = integerDigitsSizeWidth + slantWidth;
            var decimalPlaceWidth = Math.Max(spacing + separatorSize + strokeThickness
                + (charWidth * decimalPlaceCount + (spacing + strokeThickness) * (decimalPlaceCount - 1)) * decimalPlaceScale + strokeThickness
                - slantWidth * (1 - decimalPlaceScale), 0);

            return new Size(
                Math.Max(0, withoutDecimalPlaceWidth + (decimalPlaceCount > 0 && decimalPlaceScale > 0 ? decimalPlaceWidth : 0d)),
                Math.Max(0, integerDigitsSizeHeight));
        }
        /// <summary>
        /// IDigitalNumber 객체를 그리기 위한 파트 목록을 가져옵니다.
        /// </summary>
        /// <param name="indicator">IDigitalNumber 객체</param>
        /// <param name="segmentFilter">표시할 세그먼트 상태 필터</param>
        /// <returns>인디케이터 파트 목록</returns>
        public static IEnumerable<Part> CreateParts(this IDigitalNumber indicator, DigitalSegmentFilter segmentFilter)
        {
            var characterStyle = indicator?.DigitalFont;
            var value = indicator.Value;

            if (characterStyle == null || value == null) yield break;

            var charHeight = (characterStyle?.ActualSize ?? default).Height;
            var charWidth = characterStyle?.Width ?? 0d;
            var integerDigitCount = Math.Min(Math.Max(indicator.IntegerDigits, 0), 29);
            var decimalPlaceCount = Math.Min(Math.Max(indicator.DecimalPlaces, 0), 28);
            var decimalPlaceScale = Math.Min(indicator.DecimalPlaceScale.ToValidValue(1d), 1d);
            var separatorSize = charHeight * Math.Min(indicator.DecimalSeparatorSize.ToValidValue(), 1);
            var spacing = indicator.Spacing.ToValidValue() * charWidth;
            var strokeThickness = 0d;
            var halfStrokeThickness = strokeThickness / 2;
            var pitch = charWidth + strokeThickness + spacing;

            var integerDigitsSizeWidth = (charWidth + strokeThickness) * integerDigitCount + spacing * (integerDigitCount + 1);
            var integerDigitsSizeHeight = charHeight + spacing * 2 + strokeThickness;

            var decimalPlacesTransform = Transform.CreateScaling(decimalPlaceScale)
                .Translate(integerDigitsSizeWidth - spacing + separatorSize + strokeThickness + (spacing + halfStrokeThickness) * (1 - decimalPlaceScale),
                (charHeight + spacing + halfStrokeThickness) * (1 - decimalPlaceScale));

            var padLeftChar = indicator.PadZeroLeft ? '0' : ' ';
            var padRightChar = indicator.PadZeroRight ? '0' : ' ';
            var minusAlignLeft = indicator.MinusAlignLeft;

            string integerDigits = new string(' ', integerDigitCount);
            string decimalPlaces = new string(' ', decimalPlaceCount);
            bool valueIsValid = false;

            if (value != null)
            {
                var stringValue = value.ToString();

                var parsed = decimal.TryParse(stringValue, out var decimalValue);
                if (!parsed && (parsed = double.TryParse(stringValue, out var doubleValue)))
                    decimalValue = (decimal)doubleValue;
                if (parsed)
                {
                    decimalValue = Math.Round(decimalValue, decimalPlaceCount);

                    var max = integerDigitCount > 28 ? decimal.MaxValue : decimal.Parse("1" + new string('0', integerDigitCount));
                    var min = integerDigitCount > 0 ? decimal.Parse("-1" + new string('0', integerDigitCount - 1)) : 0;
                    if (decimalValue < max && decimalValue > min)
                    {
                        stringValue = null;
                        valueIsValid = true;
                        var parts = decimalValue.ToString().Split('.');
                        if (parts.Length > 0)
                        {
                            integerDigits = integerDigitCount <= 0 ? ""
                                : !minusAlignLeft && padLeftChar == ' '
                                ? parts[0].PadLeft(integerDigitCount, padLeftChar)
                                : parts[0].Remove(0, decimalValue < 0 ? 1 : 0).PadLeft(integerDigitCount - 1, padLeftChar).PadLeft(integerDigitCount, decimalValue < 0 ? '-' : padLeftChar);
                        }
                        decimalPlaces = (parts.Length > 1 ? parts[1] : "0").PadRight(decimalPlaceCount, padRightChar).Substring(0, decimalPlaceCount);
                    }
                    else
                    {
                        stringValue = decimalValue >= max ? "H" : "L";
                    }
                }

                if (stringValue != null)
                {
                    stringValue = stringValue.PadRight(integerDigitCount + decimalPlaceCount, ' ').Substring(0, integerDigitCount + decimalPlaceCount);
                    integerDigits = stringValue.Substring(0, integerDigitCount);
                    decimalPlaces = stringValue.Substring(integerDigitCount, decimalPlaceCount);
                }
            }


            if (characterStyle.GetCharacterSegments(' ', DigitalSegmentFilter.InactiveOnly).Any(d => !d.Drawing.IsEmpty)
                && decimalPlaceCount > 0
                && decimalPlaceScale > 0
                && separatorSize > 0
                && (valueIsValid || !(segmentFilter != DigitalSegmentFilter.InactiveOnly)))
            {
                var x = integerDigitsSizeWidth + (strokeThickness - spacing) / 2;
                var y = integerDigitsSizeHeight - spacing - halfStrokeThickness - separatorSize;
                var skew = Transform.CreateSkew(-Math.Max(Math.Min(characterStyle.SlantAngle.ToValidValue(), 45), 0) / 180d * Math.PI, 0, 0, y + separatorSize);
                yield return Part.CreateEllipse(x, y, separatorSize, separatorSize, skew);
            }

            for (int i = 0; i < integerDigits.Length; i++)
                foreach (var part in characterStyle.GetCharacterSegments(integerDigits[i], segmentFilter))
                    yield return part * Transform.CreateTranslation(halfStrokeThickness + pitch * i + spacing, spacing + halfStrokeThickness);
            if (decimalPlaceScale > 0)
                for (int i = 0; i < decimalPlaces.Length; i++)
                    foreach (var part in characterStyle.GetCharacterSegments(decimalPlaces[i], segmentFilter))
                        yield return part * Transform.CreateTranslation(halfStrokeThickness + pitch * i + spacing, spacing + halfStrokeThickness) * decimalPlacesTransform;
        }
    }
}
