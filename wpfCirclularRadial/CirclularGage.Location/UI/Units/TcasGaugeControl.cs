using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CirclularGage.Location.UI.Units
{
    public class TcasGaugeControl : CircularGaugeBaseControl 
    {
        static TcasGaugeControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TcasGaugeControl), new FrameworkPropertyMetadata(typeof(TcasGaugeControl)));
        }
        public TcasGaugeControl()
        {

        }
        protected override void DrawScale()
        {
            base.DrawScale();
        }
        protected override void OnCurrentValueChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnCurrentValueChanged(e);
        }

    }
}
