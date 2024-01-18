using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Custom.Wpf.Global.Controls.NumericControl
{
    public class NumericUpDownControl : AbstractBaseUpDown<double>
    {
        protected static readonly DependencyProperty StepSizeProperty = 
            DependencyProperty.Register("StepSize", typeof(double), typeof(NumericUpDownControl), new FrameworkPropertyMetadata(1d), new ValidateValueCallback(IsValidStepSizeReading));      

        public override double StepSize
        {
            get { return (double)GetValue(StepSizeProperty); }
            set { SetValue(StepSizeProperty, value); }
        }
        static NumericUpDownControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDownControl),
                       new FrameworkPropertyMetadata(typeof(NumericUpDownControl)));

            // overide default values inherited dependency properties
            FormatStringProperty.OverrideMetadata(typeof(NumericUpDownControl),
                                                  new PropertyMetadata("F2"));

            MaxValueProperty.OverrideMetadata(typeof(NumericUpDownControl),
                                                  new PropertyMetadata(double.MaxValue));

            MinValueProperty.OverrideMetadata(typeof(NumericUpDownControl),
                                                  new PropertyMetadata(double.MinValue));         
        }
        public NumericUpDownControl()
         : base()
        {
        }
        protected override bool CanDecreaseCommand()
        {
            return (Value > MinValue);
        }

        protected override bool CanIncreaseCommand()
        {
            return (Value < MaxValue);
        }

      

        protected override void OnDecrease()
        {
            // Decrement if possible
            if (this.Value - this.StepSize > this.MinValue)
            {
                this.Value = this.Value - this.StepSize;
            }
            else
            {
                // Reset to min to ensure that value = min at this point
                if (this.Value != this.MinValue)
                    this.Value = this.MinValue;
            }

            // Just to be sure
            // Value was decremented beyond bound so we reset it to min
            if (this.Value < this.MinValue)
                this.Value = this.MinValue;
        }

        protected override bool OnDecrement(double stepValue)
        {
            try
            {
                checked
                {
                    if (Value == MinValue)
                        return false;

                    var result = (double)(Value - stepValue);

                    if (result <= MinValue)
                    {
                        Value = MinValue;
                        return true;
                    }

                    if (result <= MaxValue)
                        Value = result;

                    return true;
                }
            }
            catch (OverflowException)
            {
                Value = MinValue;
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected override void OnIncrease()
        {
            // Increment if possible
            if (this.Value + this.StepSize <= this.MaxValue)
            {
                this.Value = this.Value + this.StepSize;
            }
            else
            {
                // Reset to max to ensure that value = max at this point
                if (this.Value != this.MaxValue)
                    this.Value = this.MaxValue;
            }

            // Just to be sure
            // Value was incremented beyond bound so we reset it to max
            if (this.Value > this.MaxValue)
                this.Value = this.MaxValue;
        }

        protected override bool OnIncrement(double stepValue)
        {
            try
            {
                checked
                {
                    if (Value == MaxValue)
                        return false;

                    var result = (double)(Value + stepValue);

                    if (result >= MaxValue)
                    {
                        Value = MaxValue;
                        return true;
                    }

                    if (result >= MinValue)
                        Value = result;

                    return true;
                }
            }
            catch (OverflowException)
            {
                Value = MaxValue;
                return true;
            }
            catch
            {
                return false;
            }
        }
        protected override double CoerceMaxValue(double newValue)
        {
            if (MaxValue != newValue)
            {
                if (MinValue > newValue)
                    MinValue = newValue;

                if (Value > newValue)
                    Value = newValue;
            }

            return newValue;
        }

        protected override double CoerceMinValue(double newValue)
        {
            if (MinValue != newValue)
            {
                if (Value < newValue)
                    Value = newValue;

                if (MaxValue < newValue)
                    MaxValue = newValue;
            }

            return newValue;
        }
        protected override double CoerceValue(double newValue)
        {
            if (newValue != Value)
            {
                if (MinValue > newValue)
                    MinValue = newValue;

                if (MaxValue < newValue)
                    MaxValue = newValue;
            }

            return newValue;
        }
        protected override bool ParseText(string text, out double number)
        {
            return double.TryParse(text, base.NumberStyle, CultureInfo.CurrentCulture, out number);
        }

        protected override bool VerifyText(string text, ref double tempValue)
        {
            if (double.TryParse(text, base.NumberStyle, CultureInfo.CurrentCulture, out double number))
            {
                tempValue = number;
                if (number > MaxValue || number < MinValue)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
        private static bool IsValidStepSizeReading(object value)
        {
            double v = (double)value;
            return (v > 0);
        }

    }
}
