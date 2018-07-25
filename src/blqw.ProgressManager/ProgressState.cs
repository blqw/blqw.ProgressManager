namespace blqw
{
    /// <summary>
    /// 进度状态
    /// </summary>
    public enum ProgressState
    {
        /// <summary>
        /// 最初态,未就绪的
        /// </summary>
        Initial = 0,
        /// <summary>
        /// 准备就绪
        /// </summary>
        Ready = 1,
        /// <summary>
        /// 下载中
        /// </summary>
        Progressing = 2,
        /// <summary>
        /// 等待中,暂停中
        /// </summary>
        Waiting = 3,
        /// <summary>
        /// 下载完成
        /// </summary>
        Completed = 4,
        /// <summary>
        /// 出错
        /// </summary>
        Error = 5,
    }
}
