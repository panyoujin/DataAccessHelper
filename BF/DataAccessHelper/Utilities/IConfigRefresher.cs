using System;

namespace DataAccessHelper.Utilities
{
    /// <summary>
    /// 定义配置刷新器接口
    /// </summary>
    public interface IConfigRefresher
    {

        /// <summary>
        /// 指示配置是否最新的。
        /// </summary>
        /// <returns></returns>
        bool IsLatest { get; }

        /// <summary>
        /// 刷新配置。
        /// </summary>
        /// <param name="func">刷新方法。</param>
        void Refresh(Action func);
    }
}
