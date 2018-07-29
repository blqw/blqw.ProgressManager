using blqw.Function;
using System;

namespace blqw.Progress
{
    /// <summary>
    /// 进度状态变更事件参数
    /// </summary>
    public class ProgressStateChangedEventArgs : StateChangedEventArgs<ProgressState>
    {
        /// <summary>
        /// 初始化事件参数
        /// </summary>
        /// <param name="oldState">旧状态</param>
        /// <param name="newState">新状态</param>
        public ProgressStateChangedEventArgs(ProgressState oldState, ProgressState newState)
            : base(oldState, newState)
        {
        }
    }
}
