using System;

namespace blqw.Function
{
    /// <summary>
    /// 状态修改失败事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StateChangeFailedEventArgs<T> : EventArgs
    {
        public StateChangeFailedEventArgs(T currentState, T expectState)
        {
            CurrentState = currentState;
            ExpectState = expectState;
        }

        /// <summary>
        /// 当前状态
        /// </summary>
        public T CurrentState { get; }

        /// <summary>
        /// 期望状态
        /// </summary>
        public T ExpectState { get; }
    }
}
