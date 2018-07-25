using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace blqw
{
    /// <summary>
    /// 表示一个会自动变化进度的对象
    /// </summary>
    public interface IProgressive : IProgressible
    {
        /// <summary>
        ///
        /// </summary>
        ProgressState State { get; }

        Task Start();

        void Suspend();


    }
}
