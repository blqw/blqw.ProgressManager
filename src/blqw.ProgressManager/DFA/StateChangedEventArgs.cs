using System;

namespace blqw.Function
{
    /// <summary>
    /// 状态变化事件参数
    /// </summary>
    public class StateChangedEventArgs<T> : EventArgs
    {
        public StateChangedEventArgs(T oldState, T newState)
        {
            OldState = oldState;
            NewState = newState;
        }
        /// <summary>
        /// 旧状态
        /// </summary>
        public T OldState { get; }
        /// <summary>
        /// 新状态
        /// </summary>
        public T NewState { get; }
    }
}
