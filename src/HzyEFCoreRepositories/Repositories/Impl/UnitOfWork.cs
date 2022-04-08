/*
 * *******************************************************
 *
 * 作者：hzy
 *
 * 开源地址：https://gitee.com/hzy6
 *
 * *******************************************************
 */


namespace HzyEFCoreRepositories.Repositories.Impl
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public sealed class UnitOfWork : IUnitOfWork
    {
        private bool _saveState = true;
        /// <summary>
        /// 获取保存状态
        /// </summary>
        /// <returns></returns>
        public bool GetSaveState() => this._saveState;
        /// <summary>
        /// 设置保存状态
        /// </summary>
        /// <param name="saveSate"></param>
        public void SetSaveState(bool saveSate) => this._saveState = saveSate;
        /// <summary>
        /// 打开延迟提交
        /// </summary>
        public void CommitOpen() => this._saveState = false;
    }


}

