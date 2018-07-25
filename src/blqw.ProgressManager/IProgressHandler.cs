using System;

namespace blqw
{
    /// <summary>
    /// 表示一个可绑定进度变更事件的对象
    /// </summary>
    public interface IProgressHandler : IProgerssValue
    {
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
