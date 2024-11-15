
using Indigator.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Indigator.Windows
{
    public class DigitalTextPresenter : DigitalIndicatorPresenter, IDigitalTextPresenter
    {
        static DigitalTextPresenter()
        {
            LengthProperty = RegisterProperty(nameof(Length), typeof(int), 10, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender);
            FormatProperty = RegisterProperty(nameof(Format), typeof(string), null, FrameworkPropertyMetadataOptions.AffectsRender);

            DefaultStyleKeyProperty.OverrideMetadata(typeof(DigitalTextPresenter), new FrameworkPropertyMetadata(typeof(DigitalTextPresenter)));
        }

        private static DependencyProperty RegisterProperty(string name, Type type, object defaultValue, FrameworkPropertyMetadataOptions flags = FrameworkPropertyMetadataOptions.None)
            => DependencyProperty.Register(name, type, typeof(DigitalTextPresenter), new FrameworkPropertyMetadata(defaultValue, flags));
        /// <summary>
        /// Length 종속성 속성의 식별자입니다.
        /// </summary>
        public static readonly DependencyProperty LengthProperty;
        /// <summary>
        /// Format 종속성 속성의 식별자입니다.
        /// </summary>
        public static readonly DependencyProperty FormatProperty;

        /// <inheritdoc/>
        public int Length { get => (int)GetValue(LengthProperty); set => SetValue(LengthProperty, value); }
        /// <inheritdoc/>
        public string Format { get => (string)GetValue(FormatProperty); set => SetValue(FormatProperty, value); }
        protected override Size MeasureOverride(Size availableSize)
        {
            var size = this.MeasureIndicator();
            return new Size(size.Width, size.Height);
        }

        protected override IEnumerable<Part> OnCreateParts() => this.CreateParts();
    }
}
