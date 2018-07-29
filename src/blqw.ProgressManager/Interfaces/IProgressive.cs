using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace blqw.Progress
{
    /// <summary>
    /// 表示一个会自动变化进度的对象
    /// </summary>
    public interface IProgressive : IProgressHandler
    {
        /// <summary>
        /// 当前对象状态
        /// </summary>
        ProgressState State { get; }
        /// <summary>
        /// 开始
        /// </summary>
        /// <returns></returns>
        Task Start(CancellationToken cancellation);
        /// <summary>
        /// 挂起
        /// </summary>
        void Suspend();

        event EventHandler<ProgressStateChangedEventArgs> ProgressStateChanged;
    }
}
