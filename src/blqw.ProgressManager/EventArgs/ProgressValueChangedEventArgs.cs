using System.ComponentModel;

namespace blqw
{
    /// <summary>
    /// 进度值变更时间参数
    /// </summary>
    public class ProgressValueChangedEventArgs : ProgressChangedEventArgs
    {

        public ProgressValueChangedEventArgs(double total, double value, object userState)
            : base(total < 0 ? 0 : (int)((100 * value) / total), userState)
        {
            Total = total;
            Value = value;
        }

        public double Total { get; }
        public double Value { get; }

    }
}
