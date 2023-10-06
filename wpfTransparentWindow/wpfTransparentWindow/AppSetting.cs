using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpfPopup.Global;

namespace wpfTransparentWindow
{
    public class AppSetting : ViewModelBase
    {
        private static readonly Lazy<AppSetting> lazy = new Lazy<AppSetting>(() => new AppSetting());
        private bool _isTopMost = true;
        public static AppSetting Instance { get => lazy.Value; }
        public bool IsTopMost
        {
            get => _isTopMost;
            set => SetProperty(ref _isTopMost, value);
        }
        public bool IsMouseEventMessagePass { get; set; }   
        public bool IsShowDrawWindow { get; set; }
    }
}
