using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    public interface IProgressHandler
    {
        /// <summary>
        /// 进度总数
        /// </summary>
        double Total { get; }
        /// <summary>
        /// 进度当前值
        /// </summary>
        double Value { get; }
        /// <summary>
        /// 进度百分比
        /// </summary>
        double Percentage { get; }
        /// <summary>
        /// 进度更新事件
        /// </summary>
        event EventHandler<ProgressValueChangedEventArgs> ProgressValueChanged;
        /// <summary>
        /// 进度完成事件
        /// </summary>
        event EventHandler<ProgressCompletedEventArgs> ProgressCompleted;
    }
}
