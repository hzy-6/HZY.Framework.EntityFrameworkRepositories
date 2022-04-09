/*
 * *******************************************************
 *
 * 作者：hzy
 *
 * 开源地址：https://gitee.com/hzy6
 *
 * *******************************************************
 */

using HzyEFCoreRepositories.Interceptor;
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
        private readonly bool _isMonitor;

        /// <summary>
        /// 基础上下文
        /// </summary>
        /// <param name="options"></param>
        /// <param name="isMonitor">是否监控数据库操作</param>
        public BaseDbContext(DbContextOptions<TDbContext> options, bool isMonitor = true) : base(options)
        {
            _unitOfWork = new UnitOfWork();
            _isMonitor = isMonitor;
        }

        /// <summary>
        /// OnConfiguring 重写后，一定要调用此方法
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new ShardingDbCommandInterceptor());

            //注册监控程序
            if (_isMonitor)
            {
                optionsBuilder.AddInterceptors(new MonitorDbConnectionInterceptor());
                optionsBuilder.AddInterceptors(new MonitorDbCommandInterceptor());
                optionsBuilder.AddInterceptors(new MonitorDbTransactionInterceptor());

            }
        }

        /// <summary>
        /// OnModelCreating
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

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
