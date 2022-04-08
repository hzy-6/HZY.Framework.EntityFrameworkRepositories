/*
 * *******************************************************
 *
 * 作者：hzy
 *
 * 开源地址：https://gitee.com/hzy6
 *
 * *******************************************************
 */
namespace HzyEFCoreRepositories.Repositories
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// 获取保存状态
        /// </summary>
        /// <returns></returns>
        bool GetSaveState();
        /// <summary>
        /// 设置保存状态
        /// </summary>
        /// <param name="saveSate"></param>
        void SetSaveState(bool saveSate);
        /// <summary>
        /// 打开延迟提交
        /// </summary>
        void CommitOpen();
    }
}

