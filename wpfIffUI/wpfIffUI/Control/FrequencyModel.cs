using System;

namespace wpfIffUI.Control
{
    /// <summary>
    /// UVHF 주파수 모델
    /// </summary>
    public class FrequencyModel : PropertyModel
    {
        private double _oldFrequency = 0.0;
        public EFreqeuncyManagement FreqeuncyManagement { get { return (EFreqeuncyManagement)this["FreqeuncyManagement"]; } set { this["FreqeuncyManagement"] = value; } }
        public double Frequency { get { return (double)this["Frequency"]; } set { this["Frequency"] = value; } }

        public string InputStringRegex
        {
            get
            {
                switch (FreqeuncyManagement)
                {
                    case EFreqeuncyManagement.Manual:
                        return "^[0-9]{0,3}*([.][0-9]{0,3})?$";
                    case EFreqeuncyManagement.HQ_Active:
                        return "^[AB]?[0-9]*([.][0-9]{0,3})?$";
                    case EFreqeuncyManagement.SATURN_Active:
                        return "^[AB]?[0-9]*([.][0-9]{0,3})?$";
                    case EFreqeuncyManagement.SATURN_Trainning:
                        return "^[AB]?[0-9]*([.][0-9]{0,3})?$";
                }
                return "";
            }
        }
        public string InputFrequency
        {
            get { return FreqencyStringConverter(true); }
            set { SetFreqencyModel(value); }
        }
        public void IncreaseFreqeuncy(double incrementValue)
        {
            Frequency += incrementValue;
            Frequency = Math.Round(Frequency, 4);
        }
        public void ReductionFreqeuncy(double reductionValue)
        {
            Frequency -= reductionValue;
            Frequency = Math.Round(Frequency, 4);
        }
        public string FreqencyStringConverter(bool isResolution)
        {
            string freqencyString = string.Empty;

            double frequencyValue = Frequency;

            if (isResolution == true)
            {
            }

            switch (FreqeuncyManagement)
            {
                case EFreqeuncyManagement.Manual:
                    freqencyString = ConverterInputFrequencyManual(frequencyValue);
                    break;
                case EFreqeuncyManagement.HQ_Active:
                    freqencyString = ConverterInputFrequencySaturnTraninng(frequencyValue);
                    break;
                case EFreqeuncyManagement.SATURN_Active:
                    freqencyString = ConverterInputFrequencySaturnActive(frequencyValue);
                    break;
                case EFreqeuncyManagement.SATURN_Trainning:
                    freqencyString = ConverterInputFrequencySaturnTraninng(frequencyValue);
                    break;
                default:
                    freqencyString = string.Format("{0:F3}", frequencyValue); 
                    break;
            }

            return freqencyString;
        }
        public void SetFreqencyModel(string inputValue)
        {

            if (double.TryParse(inputValue, out _oldFrequency))
            {
                this.FreqeuncyManagement = EFreqeuncyManagement.Manual;
                this.Frequency = _oldFrequency;
            }
            else if (inputValue.Contains("A") || inputValue.Contains('a'))
            {
                FreqeuncyManagement = EFreqeuncyManagement.HQ_Active;
                _oldFrequency = InputFreqencyToFrequency(inputValue);
                if (_oldFrequency.ToString().EndsWith("75", StringComparison.OrdinalIgnoreCase))
                {
                    this.FreqeuncyManagement = EFreqeuncyManagement.SATURN_Trainning;
                }
                Frequency = _oldFrequency;
            }
            else if (inputValue.Contains("B") || inputValue.Contains('b'))
            {
                this.FreqeuncyManagement = EFreqeuncyManagement.SATURN_Active;
                _oldFrequency = InputFreqencyToFrequency(inputValue);
                Frequency = _oldFrequency;
            }

        }
        public FrequencyModel()
        {
            Frequency = 0;
            FreqeuncyManagement = EFreqeuncyManagement.Manual;
        }
        public byte[] GetFreqencyBytes()
        {
            return BitConverter.GetBytes(Frequency);
        }
        private string ConverterInputFrequencyManual(double freqency)
        {
            if (freqency < 118.0)
                this._oldFrequency = 118;
            else if ((155.975 < freqency) && (freqency < 225.000))
                this._oldFrequency = 155.975;
            else if (freqency > 399.975)
                this._oldFrequency = 399.975;
            else
                this._oldFrequency = freqency;

            return _oldFrequency == 0 ? "" : string.Format("{0:F3}", _oldFrequency);

        }
        private string ConverterInputFrequencySaturnTraninng(double freqeuncy)
        {
            if (freqeuncy < 300.0) this._oldFrequency = 300;
            else if (freqeuncy > 399.975) this._oldFrequency = 399.975;
            else this._oldFrequency = freqeuncy;

            return string.Format("A{0:00.000}", _oldFrequency - 300);

        }
        private string ConverterInputFrequencySaturnActive(double freqeuncy)
        {
            if (freqeuncy < 300.0) this._oldFrequency = 300.0;
            else if (freqeuncy > 300.975) this._oldFrequency = 300.975;
            else this._oldFrequency = freqeuncy;

            return string.Format("B{0:00.000}", _oldFrequency - 300);
        }
        private double InputFreqencyToFrequency(string inputFrequency)
        {
            double freqeuncy = 0;
            switch (FreqeuncyManagement)
            {
                case EFreqeuncyManagement.HQ_Active:
                    if (Double.TryParse(inputFrequency.Trim().Replace('A', '3'), out freqeuncy) == true)
                        return freqeuncy;
                    break;
                case EFreqeuncyManagement.SATURN_Active:
                    if (Double.TryParse(inputFrequency.Trim().Replace('B', '3'), out freqeuncy) == true)
                        return freqeuncy;
                    break;
                case EFreqeuncyManagement.SATURN_Trainning:
                    if (Double.TryParse(inputFrequency.Trim().Replace('A', '3'), out freqeuncy) == true)
                        return freqeuncy;
                    break;
                default:
                    if (Double.TryParse(inputFrequency.Trim(), out freqeuncy) == true)
                        return freqeuncy;
                    break;
            }
            return freqeuncy;
        }

    }
}
