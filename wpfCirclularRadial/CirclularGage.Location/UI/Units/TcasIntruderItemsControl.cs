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
            DependencyProperty.Register("TcasDisplayRange", typeof(TcasDisplayRange), typeof(TcasIntruderItemsControl), new PropertyMetadata(TcasDisplayRange.TcasDisplayRangeNone, OnTcasDisplayRangePropertyChanged));

        // Using a DependencyProperty as the backing store for Radios.  This enables animation, styling, binding, etc...

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(TcasIntruderItemsControl), new PropertyMetadata(null));

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
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
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TcasIntruderItem();
        }
        private static void OnTcasDisplayRangePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tcasIntruderItems = d as TcasIntruderItemsControl;

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
                                ratito = 1;
                                break;
                            case TcasDisplayRange.TcasDisplayRange10nm:
                                ratito = 0.08;
                                break;
                            case TcasDisplayRange.TcasDisplayRange20nm:
                                ratito = 0.16;
                                break;
                            case TcasDisplayRange.TcasDisplayRange40nm:
                                ratito = 0.31;
                                break;
                            case TcasDisplayRange.TcasDisplayRange80nm:
                                ratito = 0.62;
                                break;
                        }

                        var child = tcasIntruderItems.Items[i];

                        var container = child.GetType();
                        var itemsFild = container.GetProperty("Range");                        
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
