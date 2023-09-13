using System;
using System.Collections.Generic;
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
        //protected override void OnCurrentValueChanged(DependencyPropertyChangedEventArgs e)
        //{
            
        //    double newValue = (double)e.NewValue;
        //    double oldValue = (double)e.OldValue;

        //    if (newValue > this.MaxValue)
        //    {
        //        newValue = this.MaxValue;
        //    }
        //    else if (newValue < this.MinValue)
        //    {
        //        newValue = this.MinValue;
        //    }

        //    if (oldValue > this.MaxValue)
        //    {
        //        oldValue = this.MaxValue;
        //    }
        //    else if (oldValue < this.MinValue)
        //    {
        //        oldValue = this.MinValue;
        //    }

        //    if (pointer != null)
        //    {
        //        double db1 = 0;
        //        double oldcurr_realworldunit = 0;
        //        double newcurr_realworldunit = 0;

        //        double realworldunit = (ScaleSweepAngle / (MaxValue - MinValue));
        //        // 1도에 따른 unit

        //        if (oldValue == MinValue && !isInitialValueSet)
        //        {
        //            oldValue = MinValue;
        //            isInitialValueSet = true;
        //        }
        //        if (oldValue < 0)
        //        {
        //            db1 = Math.Abs(MinValue);
        //            oldcurr_realworldunit = ((double)((db1) * realworldunit) - (Test(oldValue)));
        //        }
        //        else
        //        {
        //            db1 = Math.Abs(MinValue);
        //            oldcurr_realworldunit = ((double)((db1) * realworldunit) + (Test(oldValue)));
        //        }
        //        if (newValue < 0)
        //        {
        //            db1 = Math.Abs(MinValue);
        //            newcurr_realworldunit = ((double)((db1) * realworldunit) - (Test(newValue)));
        //        }
        //        else
        //        {
        //            db1 = Math.Abs(MinValue);
        //            newcurr_realworldunit = ((double)((db1) * realworldunit) + (Test(newValue)));
        //        }
        //        double oldCurrentValueAngle = ScaleStartAngle + (oldcurr_realworldunit);
        //        double newCurrentValueAngle = ScaleStartAngle + (newcurr_realworldunit);

        //        AnimaterPointer(oldCurrentValueAngle, newCurrentValueAngle);
        //    }

        //}

        //protected override void DrawRangeIndicator(double startRange, double endRange, IndicatorType indicatorType)
        //{
        //    if (startRange == 0 && endRange == 0) return;
        //    double realworldunit = (ScaleSweepAngle / (MaxValue - MinValue));
        //    double startAngle;
        //    double endAngle;
        //    double db;

        //    if (startRange < 0)
        //    {
        //        db = Math.Abs(MinValue);
        //        startAngle = ((double)(db * realworldunit) - Test(startRange));
        //    }
        //    else
        //    {
        //        db = Math.Abs(MinValue);
        //        startAngle = ((double)(db * realworldunit) + Test(startRange));
        //    }

        //    if (endRange < 0)
        //    {
        //        db = Math.Abs(MinValue);
        //        endAngle = ((double)(db * realworldunit) - Test(endRange));
        //    }
        //    else
        //    {
        //        db = Math.Abs(MinValue);
        //        endAngle = ((double)(db * realworldunit) + Test(endRange));
        //    }
        //    double startAngleFromStart = (ScaleStartAngle + startAngle);

        //    double endAngleFromStart = (ScaleStartAngle + endAngle);

        //    arcradius1 = (RangeIndicatorRadius + RangeIndicatorThickness);
        //    arcradius2 = RangeIndicatorRadius;

        //    double rangeEndAngle = ScaleStartAngle + ScaleSweepAngle;

        //    Color indigatorColor;

        //    if (indicatorType == IndicatorType.OptimalIndicator)
        //    {
        //        indigatorColor = OptimalRangeColor;
        //    }
        //    else
        //    {
        //        indigatorColor = AboveOptimalRangeColor;
        //    }
        //    Point A1 = GetCircumferencePoint(startAngleFromStart, arcradius1);
        //    Point B1 = GetCircumferencePoint(startAngleFromStart, arcradius2);
        //    Point C1 = GetCircumferencePoint(endAngleFromStart, arcradius2);
        //    Point D1 = GetCircumferencePoint(endAngleFromStart, arcradius1);
        //    bool isReflexAngle1 = Math.Abs(endAngleFromStart - startAngleFromStart) > 180.0;
        //    DrawSegment(A1, B1, C1, D1, isReflexAngle1, indigatorColor, indicatorType);
        //}
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
            // 0, 0.4, 0.8. 1.5, 4
            for(int index = 0; index < drawMajorStep.Count; index++)
            {
                if (Math.Abs(drawMajorStep[index].TcasLabelText) >= value)
                {
                    return drawMajorStep[index].TcasTickRatio * value;
                }
            }
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
                TcasTickRatio = 17
            });
            drawMajorStep.Add(1, new TcasGageDrawModel
            {
                TcasLabelText = -4,
                TcasMinorTicksCount = 4,
                TcasTickRatio = 17
            });
            drawMajorStep.Add(2, new TcasGageDrawModel
            {
                TcasLabelText = -2,
                TcasMinorTicksCount = 2,
                TcasTickRatio = 35
            });
            drawMajorStep.Add(3, new TcasGageDrawModel
            {
                TcasLabelText = -1,
                TcasMinorTicksCount = 5,
                TcasTickRatio = 68
            });
            drawMajorStep.Add(4, new TcasGageDrawModel
            {
                TcasLabelText = -0.5,
                TcasMinorTicksCount = 5,
                TcasTickRatio = 68
            });
            drawMajorStep.Add(5, new TcasGageDrawModel
            {
                TcasLabelText = 0,
                TcasMinorTicksCount = 5,
                TcasTickRatio = 1
            });
            drawMajorStep.Add(6, new TcasGageDrawModel
            {
                TcasLabelText = 0.5,
                TcasMinorTicksCount = 5,
                TcasTickRatio = 68
            });
            drawMajorStep.Add(7, new TcasGageDrawModel
            {
                TcasLabelText = 1,
                TcasMinorTicksCount = 2,
                TcasTickRatio = 35
            });
            drawMajorStep.Add(8, new TcasGageDrawModel
            {
                TcasLabelText = 2,
                TcasMinorTicksCount = 4,
                TcasTickRatio = 17
            });
            drawMajorStep.Add(9, new TcasGageDrawModel
            {
                TcasLabelText = 4,
                TcasMinorTicksCount = 4,
                TcasTickRatio = 17
            });
            drawMajorStep.Add(10, new TcasGageDrawModel
            {
                TcasLabelText = 6,
                TcasMinorTicksCount = 4,
                TcasTickRatio = 17
            });
        }
    }
}
