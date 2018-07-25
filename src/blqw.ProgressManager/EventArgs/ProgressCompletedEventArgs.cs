using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    /// <summary>
    /// 进度完成事件参数
    /// </summary>
    public class ProgressCompletedEventArgs : System.ComponentModel.RunWorkerCompletedEventArgs
    {
        public ProgressCompletedEventArgs(double total, double value, object result, Exception error, bool cancelled)
            : base(result, error, cancelled)
        {
            Total = total;
            Value = value;
        }

        public double Total { get; }
        public double Value { get; }
        public bool HasError => Error != null;
    }
}
