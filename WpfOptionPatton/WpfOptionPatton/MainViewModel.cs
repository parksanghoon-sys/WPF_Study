using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfOptionPatton.Configurations.Mailing;
using WpfOptionPatton.Services;

namespace WpfOptionPatton
{
    internal partial class MainViewModel : ObservableObject
    {
        private readonly IOptions<MailingOptions> _options;
        private readonly IIntService _intService;
        [ObservableProperty]
        private int _data;
        public MainViewModel(IOptions<MailingOptions> options, IIntService intService)
        {            
            _options = options;
            _intService = intService;

            int value;
            while (!TryGetValue(out value))
            {
                TrySetValue();
            }

            Data = value;
        }

        private void TrySetValue()
        {
            int defaultValue = _options.Value.BatchSize;
            _intService.SetValue(defaultValue);

            int value = _intService.GetValue();
            if (value != 0)
            {
                //logger.LogDebug("Successfully set value to {Value}.", value);
            }
        }

        private bool TryGetValue(out int value)
        {
            try
            {
                value = _intService.GetValue();

                return true;
            }
            catch (Exception e)
            {
                //logger.LogError(e, "Error occurred while getting default value from service.");
                value = 0;

                return false;
            }
        }
    }
}
