using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CirclularGage.Location.UI.Units
{
    public class TcasGaugeControl : CircularGaugeBaseControl
    {
        private readonly int tcasGagueTickRatio = 17;
        class TcasGageDrawModel
        {
            public double TcasLabelText;
            public int TcasMinorTicksCount;
            public double TcasTickRatio;
        }
        
        private Dictionary<int, TcasGageDrawModel> drawMajorStep;
        private int drawIndex;
        static TcasGaugeControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TcasGaugeControl), new FrameworkPropertyMetadata(typeof(TcasGaugeControl)));
        }
        public TcasGaugeControl()
        {
            InitialVariables();
        }
        protected override TextBlock DrawMajorLabel(double majorTicksUnitValue)
        {
            TextBlock textBlock = base.DrawMajorLabel(majorTicksUnitValue);
            if (drawMajorStep.ContainsKey(drawIndex))
            {
                textBlock.Text = Math.Abs(drawMajorStep[drawIndex].TcasLabelText).ToString();
            }
            return textBlock;
        }
        protected override void DrawMinorTicksRectangle(double scaleStartAngle, double majorTickUnitAngle)
        {
            if (drawMajorStep.ContainsKey(drawIndex))
            {
                double onedegree = ((scaleStartAngle + majorTickUnitAngle) - scaleStartAngle) / (drawMajorStep[drawIndex].TcasMinorTicksCount);
                if ((scaleStartAngle < (ScaleStartAngle + ScaleSweepAngle)) && (Math.Round(minvalue, ScaleValuePrecision) <= Math.Round(MaxValue, ScaleValuePrecision)))
                {
                    for (double mi = scaleStartAngle + onedegree; mi < (scaleStartAngle + majorTickUnitAngle); mi = mi + onedegree)
                    {
                        Rectangle mr = new Rectangle();
                        mr.Height = MinorTickSize.Height;
                        mr.Width = MinorTickSize.Width;
                        mr.Fill = new SolidColorBrush(MinorTickColor);
                        mr.HorizontalAlignment = HorizontalAlignment.Center;
                        mr.VerticalAlignment = VerticalAlignment.Center;
                        Point p1 = new Point(0.5, 0.5);
                        mr.RenderTransformOrigin = p1;

                        TransformGroup minortickgp = new TransformGroup();
                        RotateTransform minortickrt = new RotateTransform();
                        minortickrt.Angle = mi;
                        minortickgp.Children.Add(minortickrt);
                        TranslateTransform minorticktt = new TranslateTransform();

                        double mi_radian = (mi * Math.PI) / 180;
                        minorticktt.X = (int)((ScaleRadius) * Math.Cos(mi_radian));
                        minorticktt.Y = (int)((ScaleRadius) * Math.Sin(mi_radian));

                        minortickgp.Children.Add(minorticktt);
                        mr.RenderTransform = minortickgp;
                        rootGrid.Children.Add(mr);

                    }
                }
            }
            drawIndex++;
        }
        protected override double CalculationStartAngel(double realworldunit, double startRange)
        {
            double db;
            if (startRange < 0)
            {
                db = Math.Abs(MinValue);
                return ((double)((db) * realworldunit) - (CalculationTcasInstrumentPanelAngle(startRange)));
            }
            else
            {
                db = Math.Abs(MinValue);
                return ((double)((db) * realworldunit) + (CalculationTcasInstrumentPanelAngle(startRange)));
            }
        }       
        protected override double CalculationEndAngel(double realworldunit, double endRange)
        {
            double db;
            if (endRange < 0)
            {
                db = Math.Abs(MinValue);
                return ((double)((db) * realworldunit) - (CalculationTcasInstrumentPanelAngle(endRange)));
            }
            else
            {
                db = Math.Abs(MinValue);
                return ((double)((db) * realworldunit) + (CalculationTcasInstrumentPanelAngle(endRange)));
            }
        }
        private double Test(double value)
        {
            double realworldunit = (ScaleSweepAngle / (MaxValue - MinValue));

            value = Math.Abs(value);
            var fineGaugeRule = new[]{ 
                new { Rule = (Func<double, bool>) (p => p == 0 ), Value = value * 1},
                new { Rule = (Func<double, bool>) (p => p <= 1 ), Value = value * 17 * 4  },
                new { Rule = (Func<double, bool>) (p => p <= 2 ), Value = ((value - 1) * 17 * 2 ) + (17 * 4)},
                new { Rule = (Func<double, bool>) (p => p <= MaxValue ), Value = ((value - 2) * 17 ) + (17 * 6) }
            };

            var fineAngle = fineGaugeRule.First(p => p.Rule(value));
            return fineAngle.Value;
        }
        private double CalculationTcasInstrumentPanelAngle(double value)
        {
            value = Math.Abs(value);
            foreach (var drawmodel in drawMajorStep.Values)
            {
                if ((drawmodel.TcasLabelText) >= value)
                {
                    if (value <= 1)
                        return (drawmodel.TcasTickRatio * tcasGagueTickRatio) * value;
                    if (value <= 2)
                        return (value - 1) * (drawmodel.TcasTickRatio * tcasGagueTickRatio) + (tcasGagueTickRatio * 4);
                    if (value <= MaxValue)
                        return (value - 2) * (drawmodel.TcasTickRatio * tcasGagueTickRatio) + (tcasGagueTickRatio * 6);
                }
            }
            
            //// 0, 0.4, 0.8. 1.5, 4
            //for (int index = 0; index < drawMajorStep.Count; index++)
            //{
            //    if ((drawMajorStep[index].TcasLabelText) >= value)
            //    {                    
            //        if (value <= 1)
            //            return (drawMajorStep[index].TcasTickRatio * tcasGagueTickRatio) * value;
            //        if (value <= 2)                    
            //            return (value - 1) * (drawMajorStep[index].TcasTickRatio * tcasGagueTickRatio) + (tcasGagueTickRatio * 4);
            //        if(value <= MaxValue)
            //            return (value - 2) * (drawMajorStep[index].TcasTickRatio * tcasGagueTickRatio) + (tcasGagueTickRatio * 6);
            //    }
            //}
            return 1;
        }
        
        private void InitialVariables()
        {
            drawIndex = 0;
            drawMajorStep = new Dictionary<int, TcasGageDrawModel>();
            drawMajorStep.Add(0, new TcasGageDrawModel
            {
                TcasLabelText = -6,
                TcasMinorTicksCount = 4,
                TcasTickRatio = 1
            });
            drawMajorStep.Add(1, new TcasGageDrawModel
            {
                TcasLabelText = -4,
                TcasMinorTicksCount = 4,
                TcasTickRatio = 1
            });
            drawMajorStep.Add(2, new TcasGageDrawModel
            {
                TcasLabelText = -2,
                TcasMinorTicksCount = 2,
                TcasTickRatio = 1
            });
            drawMajorStep.Add(3, new TcasGageDrawModel
            {
                TcasLabelText = -1,
                TcasMinorTicksCount = 5,
                TcasTickRatio = 2
            });
            drawMajorStep.Add(4, new TcasGageDrawModel
            {
                TcasLabelText = -0.5,
                TcasMinorTicksCount = 5,
                TcasTickRatio = 4
            });
            drawMajorStep.Add(5, new TcasGageDrawModel
            {
                TcasLabelText = 0,
                TcasMinorTicksCount = 5,
                TcasTickRatio = 4
            });
            drawMajorStep.Add(6, new TcasGageDrawModel
            {
                TcasLabelText = 0.5,
                TcasMinorTicksCount = 5,
                TcasTickRatio = 4
            });
            drawMajorStep.Add(7, new TcasGageDrawModel
            {
                TcasLabelText = 1,
                TcasMinorTicksCount = 2,
                TcasTickRatio = 4
            });
            drawMajorStep.Add(8, new TcasGageDrawModel
            {
                TcasLabelText = 2,
                TcasMinorTicksCount = 4,
                TcasTickRatio = 2
            });
            drawMajorStep.Add(9, new TcasGageDrawModel
            {
                TcasLabelText = 4,
                TcasMinorTicksCount = 4,
                TcasTickRatio = 1
            });
            drawMajorStep.Add(10, new TcasGageDrawModel
            {
                TcasLabelText = 6,
                TcasMinorTicksCount = 4,
                TcasTickRatio = 1
            });
        }
    }
}
