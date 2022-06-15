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
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace HzyEFCoreRepositories.DbContexts
{
    /// <summary>
    /// 基础上下文
    /// </summary>
    public class DbContextBase : DbContext
    {
        private IUnitOfWork _unitOfWork;

        /// <summary>
        /// 基础上下文
        /// </summary>
        protected DbContextBase() : base()
        {
        }

        /// <summary>
        /// 基础上下文
        /// </summary>
        /// <param name="options"></param>
        public DbContextBase(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// 工作单元
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get
            {
                if (_unitOfWork == null)
                {
                    _unitOfWork = new UnitOfWorkImpl<DbContextBase>(this);
                }

                return _unitOfWork;
            }
        }

        #region 重写 保存

        /// <summary>
        /// 保存提交变更
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            return this._unitOfWork.GetDelaySaveState() ? base.SaveChanges() : 1;
        }

        /// <summary>
        /// 保存提交变更
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <returns></returns>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return this._unitOfWork.GetDelaySaveState() ? base.SaveChanges(acceptAllChangesOnSuccess) : 1;
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
            return this._unitOfWork.GetDelaySaveState()
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
            return this._unitOfWork.GetDelaySaveState() ? base.SaveChangesAsync(cancellationToken) : Task.FromResult(1);
        }

        #endregion


    }

}
