using System;
using System.Collections.Generic;
using System.Text;

namespace blqw.Progress
{
    /// <summary>
    /// 表示一个可设置进度的对象
    /// </summary>
    public interface IProgressible : IProgressHandler, IProgress<ProgressValueChangedEventArgs>
    {

        /// <summary>
        /// 通知进度完成
        /// </summary>
        /// <param name="e"></param>
        void OnCompleted(ProgressCompletedEventArgs e);
    }
}
