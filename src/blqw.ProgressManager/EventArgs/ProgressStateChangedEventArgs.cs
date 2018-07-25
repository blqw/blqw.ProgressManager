using System;
using System.Collections.Generic;
using System.Text;

namespace blqw
{
    public class ProgressStateChangedEventArgs: EventArgs
    {
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
