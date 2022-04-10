/*
 * *******************************************************
 *
 * 作者：hzy
 *
 * 开源地址：https://gitee.com/hzy6
 *
 * *******************************************************
 */

using HzyEFCoreRepositories.Repositories;
using HzyEFCoreRepositories.Repositories.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Threading.Tasks;

namespace HzyEFCoreRepositories.DbContexts
{
    /// <summary>
    /// 基础上下文
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public class BaseDbContext<TDbContext> : DbContext where TDbContext : DbContext
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// 基础上下文
        /// </summary>
        /// <param name="options"></param>
        public BaseDbContext(DbContextOptions<TDbContext> options) : base(options)
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// 事务
        /// </summary>
        /// <returns></returns>
        public virtual IDbContextTransaction BeginTransaction() => this.Database.BeginTransaction();

        /// <summary>
        /// 事务
        /// </summary>
        /// <returns></returns>
        public virtual Task<IDbContextTransaction> BeginTransactionAsync() => this.Database.BeginTransactionAsync();

        #region IUnitOfWork

        /// <summary>
        /// 工作单元
        /// </summary>
        /// <returns></returns>
        public virtual IUnitOfWork UnitOfWork => this._unitOfWork;

        /// <summary>
        /// 开启 提交
        /// </summary>
        public virtual void CommitOpen() => this._unitOfWork.CommitOpen();

        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        public virtual int Commit()
        {
            this._unitOfWork.SetSaveState(true);
            return this.SaveChanges();
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        public virtual Task<int> CommitAsync()
        {
            this._unitOfWork.SetSaveState(true);
            return this.SaveChangesAsync();
        }

        #endregion

        #region 重写 保存

        /// <summary>
        /// 保存提交变更
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            return this._unitOfWork.GetSaveState() ? base.SaveChanges() : 1;
        }

        /// <summary>
        /// 保存提交变更
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <returns></returns>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return this._unitOfWork.GetSaveState() ? base.SaveChanges(acceptAllChangesOnSuccess) : 1;
        }

        /// <summary>
        /// 保存提交变更
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return this._unitOfWork.GetSaveState()
                ? base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken)
                : Task.FromResult(1);
        }

        /// <summary>
        /// 保存提交变更
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return this._unitOfWork.GetSaveState() ? base.SaveChangesAsync(cancellationToken) : Task.FromResult(1);
        }

        #endregion

    }

}
