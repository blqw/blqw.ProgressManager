using System;

namespace blqw
{
    /// <summary>
    /// 进度完成事件参数
    /// </summary>
    public class ProgressCompletedEventArgs : System.ComponentModel.RunWorkerCompletedEventArgs, IProgerssValue
    {
        /// <summary>
        /// 初始化事件参数
        /// </summary>
        /// <param name="total">总进度值</param>
        /// <param name="value">当前进度值</param>
        /// <param name="result">处理结果</param>
        /// <param name="error">异常</param>
        /// <param name="cancelled">进度完成是否因为被取消</param>
        public ProgressCompletedEventArgs(double total, double value, object result, Exception error, bool cancelled)
            : base(result, error, cancelled)
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
        /// <summary>
        /// 是否有错误
        /// </summary>
        public bool HasError => Error != null;
    }
}
