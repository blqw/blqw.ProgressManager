using System;

namespace blqw
{
    /// <summary>
    /// 进度状态变更事件参数
    /// </summary>
    public class ProgressStateChangedEventArgs: EventArgs
    {
        /// <summary>
        /// 初始化事件参数
        /// </summary>
        /// <param name="oldState">旧状态</param>
        /// <param name="newState">新状态</param>
        public ProgressStateChangedEventArgs(ProgressState oldState, ProgressState newState)
        {
            OldState = oldState;
            NewState = newState;
        }
        /// <summary>
        /// 旧状态
        /// </summary>
        public ProgressState OldState { get; }
        /// <summary>
        /// 新状态
        /// </summary>
        public ProgressState NewState { get; }
    }
}
