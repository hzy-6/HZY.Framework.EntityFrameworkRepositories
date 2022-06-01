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
using System.Threading.Tasks;

namespace HzyEFCoreRepositories.Repositories.Impl
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        private bool _saveState = true;
        private readonly TDbContext _dbContext;

        /// <summary>
        /// 工作单元 构造
        /// </summary>
        /// <param name="dbContext"></param>
        public UnitOfWork(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 获取延迟保存状态
        /// </summary>
        /// <returns></returns>
        public virtual bool GetDelaySaveState() => this._saveState;

        /// <summary>
        /// 设置延迟保存状态
        /// </summary>
        /// <param name="saveSate"></param>
        public virtual void SetDelaySaveState(bool saveSate) => this._saveState = saveSate;

        /// <summary>
        /// 打开延迟提交
        /// </summary>
        public virtual void CommitDelayStart() => this._saveState = false;

        /// <summary>
        /// 延迟提交结束
        /// </summary>
        /// <returns></returns>
        public virtual int CommitDelayEnd()
        {
            this.SetDelaySaveState(true);
            return this._dbContext.SaveChanges();
        }

        /// <summary>
        /// 延迟提交结束
        /// </summary>
        /// <returns></returns>
        public virtual Task<int> CommitDelayEndAsync()
        {
            this.SetDelaySaveState(true);
            return this._dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <returns></returns>
        public virtual IDbContextTransaction BeginTransaction() => this._dbContext.Database.BeginTransaction();

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <returns></returns>
        public virtual Task<IDbContextTransaction> BeginTransactionAsync() => this._dbContext.Database.BeginTransactionAsync();

        /// <summary>
        /// 获取当前 dbcontext 事务
        /// </summary>
        public virtual IDbContextTransaction CurrentDbContextTransaction => this._dbContext.Database.CurrentTransaction;

        /// <summary>
        /// 获取当前 事务
        /// </summary>
        public virtual IDbTransaction CurrentDbTransaction => this.GetDbTransaction(this._dbContext.Database.CurrentTransaction);

        /// <summary>
        /// 获取当前 事务 根据 IDbContextTransaction 事务
        /// </summary>
        /// <param name="dbContextTransaction"></param>
        /// <returns></returns>
        public virtual IDbTransaction GetDbTransaction(IDbContextTransaction dbContextTransaction)
        {
            return dbContextTransaction.GetDbTransaction();
        }

        /// <summary>
        /// 获取 dbset 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public DbSet<T> DbSet<T>() where T : class, new() => _dbContext.Set<T>();


    }


}

