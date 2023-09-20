using CirclularGage.Location.Local.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace CirclularGage.Location.UI.Units

{
    public class TcasIntruderItemsControl : ItemsControl
    {
        private double[] _intruderInitialPointRatito;             
        
        public static readonly DependencyProperty TcasDisplayRangeProperty =
            DependencyProperty.Register("TcasDisplayRange", typeof(TcasDisplayRange), typeof(TcasIntruderItemsControl), 
                new PropertyMetadata(TcasDisplayRange.TcasDisplayRangeNone,OnTcasDisplayRangePropertyChanged));

        // Using a DependencyProperty as the backing store for Radios.  This enables animation, styling, binding, etc...

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(TcasIntruderItemsControl), new PropertyMetadata(null));
        
        /// <summary>
        /// Tcas Display 반지름 
        /// </summary>
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }        
        /// <summary>
        /// Tcas Display Range 변화 
        /// </summary>
        public TcasDisplayRange TcasDisplayRange
        {
            get { return (TcasDisplayRange)GetValue(TcasDisplayRangeProperty); }
            set { SetValue(TcasDisplayRangeProperty, value); }
        }
       
        static TcasIntruderItemsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TcasIntruderItemsControl), new FrameworkPropertyMetadata(typeof(TcasIntruderItemsControl)));
        }
        public TcasIntruderItemsControl()
        {
            this.InitialVariables();
        }
        public override void OnApplyTemplate()
        {
            int a = 1;
        }
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TcasIntruderItem();
        }
        private static void OnTcasDisplayRangePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tcasIntruderItems = d as TcasIntruderItemsControl;
            var tcasRadius = tcasIntruderItems.Radius;

            if (e.OldValue != e.NewValue)
            {
                if (tcasIntruderItems.Items != null)
                {
                    double ratito = 1;
                    for (int i = 0; i < tcasIntruderItems.Items.Count; i++)
                    {
                        switch (e.NewValue)
                        {
                            case TcasDisplayRange.TcasDisplayRangeNone:
                                ratito = 1.75;
                                break;
                            case TcasDisplayRange.TcasDisplayRange10nm:
                                ratito = 21;
                                break;
                            case TcasDisplayRange.TcasDisplayRange20nm:
                                ratito = 10.5;
                                break;
                            case TcasDisplayRange.TcasDisplayRange40nm:
                                ratito = 5.25;
                                break;
                            case TcasDisplayRange.TcasDisplayRange80nm:
                                ratito = 2.62;
                                break;
                        }

                        var child = tcasIntruderItems.Items[i];

                        var container = child.GetType();                        
                        var itemsFild = container.GetProperty("DisplayRangeRatio");                        
                        var objjectPointX = (double)itemsFild.GetValue(child, null) / tcasIntruderItems._intruderInitialPointRatito[i];

                        itemsFild.SetValue(child, objjectPointX * ratito, null);                        

                        tcasIntruderItems._intruderInitialPointRatito[i] = ratito;                        
                    }
                }
            }
        }
        private void InitialVariables()
        {
            _intruderInitialPointRatito = new double[30];

            _intruderInitialPointRatito = Enumerable.Repeat<double>(1, 30).ToArray<double>();
        }

       
    }
}
