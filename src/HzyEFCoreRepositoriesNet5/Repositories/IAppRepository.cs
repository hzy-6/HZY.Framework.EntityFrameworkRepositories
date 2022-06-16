using HzyEFCoreRepositories.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HzyEFCoreRepositories.Repositories
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    public interface IAppRepository<T, TDbContext> : IRepositoryBase<T>, IDisposable
        where T : class, new()
        where TDbContext : DbContextBase
    {
        /// <summary>
        /// 获取 dbcontext 对象
        /// </summary>
        TDbContext Orm => default;

        /// <summary>
        /// 工作单元
        /// </summary>
        IUnitOfWork UnitOfWork => default;

        #region 过滤

        /// <summary>
        /// 加入查询过滤条件
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IAppRepository<T, TDbContext> AddQueryFilter(Expression<Func<T, bool>> filter = null);
        /// <summary>
        /// 忽略查询过滤条件
        /// </summary>
        /// <returns></returns>
        IAppRepository<T, TDbContext> IgnoreQueryFilter();
        /// <summary>
        /// 恢复忽略查询过滤条件
        /// </summary>
        /// <returns></returns>
        IAppRepository<T, TDbContext> RecoveryQueryFilter();

        #endregion

    }

    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAppRepository<T> : IRepositoryBase<T>, IDisposable
        where T : class, new()
    {
        /// <summary>
        /// 工作单元
        /// </summary>
        IUnitOfWork UnitOfWork => default;

        #region 过滤

        /// <summary>
        /// 加入查询过滤条件
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IAppRepository<T> AddQueryFilter(Expression<Func<T, bool>> filter = null);
        /// <summary>
        /// 忽略查询过滤条件
        /// </summary>
        /// <returns></returns>
        IAppRepository<T> IgnoreQueryFilter();
        /// <summary>
        /// 恢复忽略查询过滤条件
        /// </summary>
        /// <returns></returns>
        IAppRepository<T> RecoveryQueryFilter();

        #endregion

    }

}
