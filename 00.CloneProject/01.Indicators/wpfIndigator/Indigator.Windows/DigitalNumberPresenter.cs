using Indigator.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Indigator.Windows
{
    public class DigitalNumberPresenter : DigitalIndicatorPresenter, IDigitalNumberPresenter
    {
        /// <summary>
        /// IntegerDigits 종속성 속성의 식별자입니다.
        /// </summary>
        public static readonly DependencyProperty IntegerDigitsProperty;
        /// <summary>
        /// DecimalPlaces 종속성 속성의 식별자입니다.
        /// </summary>
        public static readonly DependencyProperty DecimalPlacesProperty;
        /// <summary>
        /// DecimalSeparatorSize 종속성 속성의 식별자입니다.
        /// </summary>
        public static readonly DependencyProperty DecimalSeparatorSizeProperty;
        /// <summary>
        /// DecimalPlaceScale 종속성 속성의 식별자입니다.
        /// </summary>
        public static readonly DependencyProperty DecimalPlaceScaleProperty;
        /// <summary>
        /// PadZeroLeft 종속성 속성의 식별자입니다.
        /// </summary>
        public static readonly DependencyProperty PadZeroLeftProperty;
        /// <summary>
        /// PadZeroRight 종속성 속성의 식별자입니다.
        /// </summary>
        public static readonly DependencyProperty PadZeroRightProperty;
        /// <summary>
        /// MinusAlignLeft 종속성 속성의 식별자입니다.
        /// </summary>
        public static readonly DependencyProperty MinusAlignLeftProperty;

        /// <inheritdoc/>
        public int IntegerDigits { get => (int)GetValue(IntegerDigitsProperty); set => SetValue(IntegerDigitsProperty, value); }
        /// <inheritdoc/>
        public int DecimalPlaces { get => (int)GetValue(DecimalPlacesProperty); set => SetValue(DecimalPlacesProperty, value); }
        /// <inheritdoc/>
        public double DecimalSeparatorSize { get => (double)GetValue(DecimalSeparatorSizeProperty); set => SetValue(DecimalSeparatorSizeProperty, value); }
        /// <inheritdoc/>
        public double DecimalPlaceScale { get => (double)GetValue(DecimalPlaceScaleProperty); set => SetValue(DecimalPlaceScaleProperty, value); }
        /// <inheritdoc/>
        public bool PadZeroLeft { get => (bool)GetValue(PadZeroLeftProperty); set => SetValue(PadZeroLeftProperty, value); }
        /// <inheritdoc/>
        public bool PadZeroRight { get => (bool)GetValue(PadZeroRightProperty); set => SetValue(PadZeroRightProperty, value); }
        /// <inheritdoc/>
        public bool MinusAlignLeft { get => (bool)GetValue(MinusAlignLeftProperty); set => SetValue(MinusAlignLeftProperty, value); }

        static DigitalNumberPresenter()
        {
            IntegerDigitsProperty = RegisterProperty(nameof(IntegerDigits), typeof(int), 5, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender);
            DecimalPlacesProperty = RegisterProperty(nameof(DecimalPlaces), typeof(int), 0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender);
            DecimalSeparatorSizeProperty = RegisterProperty(nameof(DecimalSeparatorSize), typeof(double), 0.1, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender);
            DecimalPlaceScaleProperty = RegisterProperty(nameof(DecimalPlaceScale), typeof(double), 0.8, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender);
            PadZeroLeftProperty = RegisterProperty(nameof(PadZeroLeft), typeof(bool), false, FrameworkPropertyMetadataOptions.AffectsRender);
            PadZeroRightProperty = RegisterProperty(nameof(PadZeroRight), typeof(bool), false, FrameworkPropertyMetadataOptions.AffectsRender);
            MinusAlignLeftProperty = RegisterProperty(nameof(MinusAlignLeft), typeof(bool), true, FrameworkPropertyMetadataOptions.AffectsRender);

            DefaultStyleKeyProperty.OverrideMetadata(typeof(DigitalNumberPresenter), new FrameworkPropertyMetadata(typeof(DigitalNumberPresenter)));
        }
        private static DependencyProperty RegisterProperty(string name, Type type, object defaultValue, FrameworkPropertyMetadataOptions flags = FrameworkPropertyMetadataOptions.None)
         => DependencyProperty.Register(name, type, typeof(DigitalNumberPresenter), new FrameworkPropertyMetadata(defaultValue, flags));
        protected override Size MeasureOverride(Size availableSize)
        {
            var size = this.MeasureIndicator();
            return new Size(size.Width, size.Height);
        }
        protected override IEnumerable<Part> OnCreateParts()
                => this.CreateParts();
    }
}
