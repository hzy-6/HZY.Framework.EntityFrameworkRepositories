/*
 * *******************************************************
 *
 * 作者：hzy
 *
 * 开源地址：https://gitee.com/hzy6
 *
 * *******************************************************
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace HZY.Framework.EntityFrameworkRepositories.Repositories
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 获取保存状态
        /// </summary>
        /// <returns></returns>
        bool GetDelaySaveState();

        /// <summary>
        /// 设置保存状态
        /// </summary>
        /// <param name="saveSate"></param>
        void SetDelaySaveState(bool saveSate);

        /// <summary>
        /// 打开延迟提交
        /// </summary>
        void CommitDelayStart();

        /// <summary>
        /// 延迟提交结束
        /// </summary>
        /// <returns></returns>
        int CommitDelayEnd();

        /// <summary>
        /// 延迟提交结束
        /// </summary>
        /// <returns></returns>
        Task<int> CommitDelayEndAsync();

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <returns></returns>
        IDbContextTransaction BeginTransaction();

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <returns></returns>
        Task<IDbContextTransaction> BeginTransactionAsync();

        /// <summary>
        /// 获取当前上下文事务
        /// </summary>
        IDbContextTransaction CurrentDbContextTransaction => default;

        /// <summary>
        /// 获取当前事务
        /// </summary>
        IDbTransaction CurrentDbTransaction => default;

        /// <summary>
        /// 获取当前 事务 根据 IDbContextTransaction 事务
        /// </summary>
        /// <param name="dbContextTransaction"></param>
        /// <returns></returns>
        IDbTransaction GetDbTransaction(IDbContextTransaction dbContextTransaction);

        /// <summary>
        /// 获取 dbset 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        DbSet<T> DbSet<T>() where T : class, new() => default;

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <returns></returns>
        int SaveChanges(bool acceptAllChangesOnSuccess);

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken());





    }
}

