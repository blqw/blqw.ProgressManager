using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    static class ProgerssHelper
    {
        public static double Percentage(double total, double value) =>
            total < 0 ? 0 : (100d * value) / total;

    }
}
