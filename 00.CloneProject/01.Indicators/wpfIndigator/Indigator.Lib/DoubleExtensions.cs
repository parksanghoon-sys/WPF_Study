using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigator.Lib
{
    static class DoubleExtensions
    {
        public static double ToValidValue(this double value, double defaultValue = 0d)
        {
            return double.IsNaN(value) || double.IsInfinity(value) ? defaultValue : Math.Max(value, defaultValue);
        }
    }
}
