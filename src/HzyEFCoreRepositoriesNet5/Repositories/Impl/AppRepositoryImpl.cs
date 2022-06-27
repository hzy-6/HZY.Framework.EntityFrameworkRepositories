/*
 * *******************************************************
 *
 * 作者：hzy
 *
 * 开源地址：https://gitee.com/hzy6
 *
 * *******************************************************
 */
using System;
using System.Linq.Expressions;
using HzyEFCoreRepositories.DbContexts;

namespace HzyEFCoreRepositories.Repositories.Impl
{
    /// <summary>
    /// 基础仓储 实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    public class AppRepositoryImpl<T, TDbContext> : RepositoryBaseImpl<T, TDbContext>, IAppRepository<T, TDbContext>
        where T : class, new()
        where TDbContext : DbContextBase
    {
        /// <summary>
        /// 基础仓储
        /// </summary>
        /// <param name="context">dbcontext 上下文</param>
        /// <param name="filter">过滤条件</param>
        public AppRepositoryImpl(TDbContext context, Expression<Func<T, bool>> filter = null) : base(context, filter)
        {

        }

        #region 过滤

        /// <summary>
        /// 添加检索过滤
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual IAppRepository<T, TDbContext> AddQueryFilter(Expression<Func<T, bool>> filter = null)
        {
            this._filter = filter;
            return this;
        }

        /// <summary>
        /// 忽略查询过滤条件
        /// </summary>
        /// <returns></returns>
        public virtual IAppRepository<T, TDbContext> IgnoreQueryFilter()
        {
            isIgnoreQueryFilter = true;
            return this;
        }

        /// <summary>
        /// 恢复忽略查询过滤条件
        /// </summary>
        /// <returns></returns>
        public virtual IAppRepository<T, TDbContext> RecoveryQueryFilter()
        {
            isIgnoreQueryFilter = false;
            return this;
        }

        #endregion
    }

    /// <summary>
    /// 基础仓储 实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AppRepositoryImpl<T> : RepositoryBaseImpl<T, object>, IAppRepository<T>
        where T : class, new()
    {
        /// <summary>
        /// 基础仓储
        /// </summary>
        /// <param name="filter">过滤条件</param>
        public AppRepositoryImpl(Expression<Func<T, bool>> filter = null)
            : base(HzyEFCoreUtil.GetServiceProvider().GetService(HzyEFCoreUtil.GetDbContextTypeByKey(typeof(T).FullName)), filter)
        {

        }

        #region 过滤

        /// <summary>
        /// 添加检索过滤
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual IAppRepository<T> AddQueryFilter(Expression<Func<T, bool>> filter = null)
        {
            this._filter = filter;
            return this;
        }

        /// <summary>
        /// 忽略查询过滤条件
        /// </summary>
        /// <returns></returns>
        public virtual IAppRepository<T> IgnoreQueryFilter()
        {
            isIgnoreQueryFilter = true;
            return this;
        }

        /// <summary>
        /// 恢复忽略查询过滤条件
        /// </summary>
        /// <returns></returns>
        public virtual IAppRepository<T> RecoveryQueryFilter()
        {
            isIgnoreQueryFilter = false;
            return this;
        }

        #endregion


    }


}
