using System;

namespace wpfIffUI.Control
{
    /// <summary>
    /// UVHF 주파수 모델
    /// </summary>
    public class FrequencyModel : PropertyModel
    {        
        public EFreqeuncyManagement FreqeuncyManagement { get { return (EFreqeuncyManagement)this["FreqeuncyManagement"]; } set { this["FreqeuncyManagement"] = value; } }
        private double _frequency = 0.0d;

        //public string InputStringRegex
        //{
        //    get
        //    {
        //        switch (FreqeuncyManagement)
        //        {
        //            case EFreqeuncyManagement.Manual:
        //                return "^[0-9]{0,3}*([.][0-9]{0,3})?$";
        //            case EFreqeuncyManagement.HQ_Active:
        //                return "^[AB]?[0-9]*([.][0-9]{0,3})?$";
        //            case EFreqeuncyManagement.SATURN_Active:
        //                return "^[AB]?[0-9]*([.][0-9]{0,3})?$";
        //            case EFreqeuncyManagement.SATURN_Trainning:
        //                return "^[AB]?[0-9]*([.][0-9]{0,3})?$";
        //        }
        //        return "";
        //    }
        //}
        public string InputFrequency 
        { 
            get 
            {
                return (string)FreqencyStringConverter(true); 
            } 
            set 
            {
                SetFreqencyModel(value);
                Notify("InputFrequency");
            } 
        }  
        public void IncreaseFreqeuncy(double incrementValue)
        {
            _frequency += incrementValue;
            _frequency = Math.Round(_frequency, 4);
            InputFrequency = FreqencyStringConverter(true);
        }
        public void ReductionFreqeuncy(double reductionValue)
        {
            _frequency -= reductionValue;
            _frequency = Math.Round(_frequency, 4);
            InputFrequency = FreqencyStringConverter(true);
        }
        public string FreqencyStringConverter(bool isResolution)
        {
            string freqencyString = string.Empty;

            double frequencyValue = _frequency;

            if (isResolution == true)
            {
                frequencyValue = FrequencyResolution(frequencyValue);
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
        private double FrequencyResolution(double frequency)
        {
            int valide = (int)(Math.Round((frequency / 0.025), 2));
            frequency = valide * 0.025;
            return frequency;
        }
        public void SetFreqencyModel(string inputValue)
        {
            inputValue = inputValue.ToUpper();
            if (double.TryParse(inputValue, out _frequency))
            {
                this.FreqeuncyManagement = EFreqeuncyManagement.Manual;                
            }
            else if (inputValue.Contains("A"))
            {
                FreqeuncyManagement = EFreqeuncyManagement.HQ_Active;
                _frequency = InputFreqencyToFrequency(inputValue);
                if (inputValue.EndsWith("75", StringComparison.OrdinalIgnoreCase))
                {
                    this.FreqeuncyManagement = EFreqeuncyManagement.SATURN_Trainning;
                }                
            }
            else if (inputValue.Contains("B"))
            {
                this.FreqeuncyManagement = EFreqeuncyManagement.SATURN_Active;
                _frequency = InputFreqencyToFrequency(inputValue);                
            }

        }
        public FrequencyModel()
        {
            _frequency = 0;
            FreqeuncyManagement = EFreqeuncyManagement.Manual;
        }
        public byte[] GetFreqencyBytes()
        {
            return BitConverter.GetBytes(_frequency);
        }
        private string ConverterInputFrequencyManual(double freqency)
        {
            if (freqency < 118.0)
                this._frequency = 118;
            else if ((155.975 < freqency) && (freqency < 225.000))
                this._frequency = 155.975;
            else if (freqency > 399.975)
                this._frequency = 399.975;
            else
                this._frequency = freqency;

            return _frequency == 0 ? "" : string.Format("{0:F3}", _frequency);

        }
        private string ConverterInputFrequencySaturnTraninng(double freqeuncy)
        {
            if (freqeuncy < 300.0) this._frequency = 300;
            else if (freqeuncy > 399.975) this._frequency = 399.975;
            else this._frequency = freqeuncy;

            return string.Format("A{0:00.000}", _frequency - 300);

        }
        private string ConverterInputFrequencySaturnActive(double freqeuncy)
        {
            if (freqeuncy < 300.0) this._frequency = 300.0;
            else if (freqeuncy > 300.975) this._frequency = 300.975;
            else this._frequency = freqeuncy;

            return string.Format("B{0:00.000}", _frequency - 300);
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
