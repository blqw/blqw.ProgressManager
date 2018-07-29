namespace blqw.Progress
{
    /// <summary>
    /// 表示一个拥有进度值和进度百分比的对象
    /// </summary>
    public interface IProgerssValue
    {
        /// <summary>
        /// 进度总数
        /// </summary>
        double Total { get; }
        /// <summary>
        /// 进度当前值
        /// </summary>
        double Value { get; }
        /// <summary>
        /// 进度百分比
        /// </summary>
        double Percentage { get; }
    }
}
