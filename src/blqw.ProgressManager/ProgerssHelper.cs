using System;
using System.Collections.Generic;
using System.Text;

namespace blqw.Progress
{
    static class ProgerssHelper
    {
        /// <summary>
        /// 计算进度的百分比
        /// </summary>
        /// <param name="total">总进度</param>
        /// <param name="value">当前进度</param>
        /// <returns></returns>
        public static double Percentage(double total, double value) =>
            total <= 0 ? 0 : (100d * value) / total;

        public static bool Exchange<T>(object locker, ref T location1, T value, Func<T, bool> comparand)
        {
            if (!comparand(location1))
            {
                return false;
            }
            lock (locker)
            {
                if(comparand(location1))
                {
                    location1 = value;
                    return true;
                }
            }
            return false;
        }
    }
}
