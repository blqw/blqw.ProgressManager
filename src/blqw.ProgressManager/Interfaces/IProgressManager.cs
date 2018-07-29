using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace blqw.Progress
{
    /// <summary>
    /// 进度管理器
    /// </summary>
    public interface IProgressManager : IProgressive, IProgressHandler, IEnumerable<ManageableProgressive>
    {
        /// <summary>
        /// 获取一个不重复的id
        /// </summary>
        /// <returns></returns>
        Guid CreateId();
        /// <summary>
        /// 添加可管理进度对象
        /// </summary>
        /// <param name="progress"></param>
        ManageableProgressive Add(IProgressive progressive);

        IEnumerable<ManageableProgressive> AddRange(IEnumerable<IProgressive> progressives);
        /// <summary>
        /// 移除可管理进度对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ManageableProgressive Remove(Guid id);
        /// <summary>
        /// 根据id获取可管理进度对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ManageableProgressive this[Guid id] { get; }
        /// <summary>
        /// 管理器中的对象个数
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 准备下载
        /// </summary>
        /// <returns></returns>
        void Start(Guid id);

        /// <summary>
        /// 挂起/
        /// </summary>
        void Suspend(Guid id);
    }
}
