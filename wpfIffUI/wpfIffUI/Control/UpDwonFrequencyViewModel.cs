namespace wpfIffUI.Control
{
    public class UpDwonFrequencyViewModel : PropertyModel
    {
        public double SpinIncrement { get { return (double)this["SpinIncrement"]; } set { this["SpinIncrement"] = value; } }
        public double MinValue { get { return (double)this["MinValue"]; } set { this["MinValue"] = value; } }
        public double MaxValue { get { return (double)this["MaxValue"]; } set { this["MaxValue"] = value; } }
        public FrequencyModel FrequencyValue { get { return (FrequencyModel)this["FrequencyValue"]; } set { this["FrequencyValue"] = value; } }
        public bool IsEnabled { get { return (bool)this["IsEnabled"]; } set { this["IsEnabled"] = value; } }
        public double SecondMinValue { get { return (double)this["SecondMinValue"]; } set { this["SecondMinValue"] = value; } }
        public double SecondMaxValue { get { return (double)this["SecondMaxValue"]; } set { this["SecondMaxValue"] = value; } }
        public bool SecondIsEnabled { get { return (bool)this["SecondIsEnabled"]; } set { this["SecondIsEnabled"] = value; } }
        public UpDwonFrequencyViewModel()
        {
            SpinIncrement = 0;
            MinValue = double.MinValue;
            MaxValue = double.MaxValue;
            FrequencyValue = new FrequencyModel();
            IsEnabled = true;
            SecondMinValue = 0;
            SecondMaxValue = 0;
            SecondIsEnabled = false;
        }
    }
}
