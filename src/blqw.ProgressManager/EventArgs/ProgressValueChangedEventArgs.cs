using System.ComponentModel;

namespace blqw
{
    /// <summary>
    /// 进度值变更事件参数
    /// </summary>
    public class ProgressValueChangedEventArgs : ProgressChangedEventArgs, IProgerssValue
    {
        /// <summary>
        /// 初始化事件参数
        /// </summary>
        /// <param name="total">总进度值</param>
        /// <param name="value">当前进度值</param>
        /// <param name="userState">用户数据</param>
        public ProgressValueChangedEventArgs(double total, double value, object userState)
            : base((int)ProgerssHelper.Percentage(total, value), userState)
        {
            Total = total;
            Value = value;
            Percentage = ProgerssHelper.Percentage(total, value);
        }

        /// <summary>
        /// 总进度值
        /// </summary>
        public double Total { get; }
        /// <summary>
        /// 当前进度值
        /// </summary>
        public double Value { get; }
        /// <summary>
        /// 进度百分比
        /// </summary>
        public double Percentage { get; }
    }
}
