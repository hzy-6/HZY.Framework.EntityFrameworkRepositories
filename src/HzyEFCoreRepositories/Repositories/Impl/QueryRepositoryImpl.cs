using HzyEFCoreRepositories.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HzyEFCoreRepositories.Repositories.Impl
{
    /// <summary>
    /// 基础仓储 查询 实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    public abstract class QueryRepositoryImpl<T, TDbContext> : RepositoryCoreImpl<T, TDbContext>, IQueryRepository<T, TDbContext>
        where T : class, new()
        where TDbContext : class
    {
        /// <summary>
        /// 查询是否跟踪
        /// </summary>
        protected readonly bool _isTracking;

        /// <summary>
        /// 基础仓储 查询 实现
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="filter"></param>
        /// <param name="isTracking">是否跟踪</param>
        public QueryRepositoryImpl(TDbContext dbContext, Expression<Func<T, bool>> filter = null, bool isTracking = false)
            : base(dbContext, filter)
        {
            this._isTracking = isTracking;
        }

        #region 过滤

        /// <summary>
        /// 添加检索过滤
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual IQueryRepository<T, TDbContext> AddQueryFilter(Expression<Func<T, bool>> filter = null)
        {
            this._filter = filter;
            return this;
        }

        /// <summary>
        /// 忽略查询过滤条件
        /// </summary>
        /// <returns></returns>
        public virtual IQueryRepository<T, TDbContext> IgnoreQueryFilter()
        {
            isIgnoreQueryFilter = true;
            return this;
        }

        /// <summary>
        /// 恢复忽略查询过滤条件
        /// </summary>
        /// <returns></returns>
        public virtual IQueryRepository<T, TDbContext> RecoveryQueryFilter()
        {
            isIgnoreQueryFilter = false;
            return this;
        }

        #endregion

        #region 查询 复杂型

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="isTracking">是否追踪</param>
        /// <returns></returns>
        public virtual IQueryable<T> Query(bool? isTracking = null)
            => (isTracking == null ? true : isTracking.Value) ?
            this.UnitOfWork.DbSet<T>().WhereIf(!isIgnoreQueryFilter && _filter != null, _filter).AsQueryable() :
            this.UnitOfWork.DbSet<T>().WhereIf(!isIgnoreQueryFilter && _filter != null, _filter).AsNoTracking();

        /// <summary>
        /// 查询 有跟踪
        /// </summary>
        public virtual IQueryable<T> Select => this.Query();

        /// <summary>
        /// 查询 无跟踪
        /// </summary>
        public virtual IQueryable<T> SelectNoTracking => this.Query(false);

        #endregion

        #region 查询 单条

        /// <summary>
        /// 查询 根据条件
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual T Find(Expression<Func<T, bool>> expWhere)
            => this.Query().Where(expWhere).FirstOrDefault();

        /// <summary>
        /// 查询 根据id
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public virtual T FindById<TKey>(TKey key)
            => this.Query().FirstOrDefault(this.GetKeyExpression(key));

        /// <summary>
        /// 查询 根据条件
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual Task<T> FindAsync(Expression<Func<T, bool>> expWhere)
            => this.Query().Where(expWhere).FirstOrDefaultAsync();

        /// <summary>
        /// 查询 根据id
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public virtual Task<T> FindByIdAsync<TKey>(TKey key)
            => this.Query().FirstOrDefaultAsync(this.GetKeyExpression(key));

        #endregion

        #region 查询 多条

        /// <summary>
        /// 获取列表 根据查询条件
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual List<T> ToList(Expression<Func<T, bool>> expWhere)
            => this.Query().Where(expWhere).ToList();

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public virtual List<T> ToListAll()
            => this.Query().ToList();

        /// <summary>
        /// 获取列表 根据查询条件
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual Task<List<T>> ToListAsync(Expression<Func<T, bool>> expWhere)
            => this.Query().Where(expWhere).ToListAsync();

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public virtual Task<List<T>> ToListAllAsync()
            => this.Query().ToListAsync();

        #endregion

        #region 是否存在 、 数量

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <returns></returns>
        public virtual int Count()
            => this.Query().Count();

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <returns></returns>
        public virtual long CountLong()
            => this.Query().LongCount();

        /// <summary>
        /// 获取数量 根据条件
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual int Count(Expression<Func<T, bool>> expWhere)
            => this.Query().Count(expWhere);

        /// <summary>
        /// 获取数量 根据条件
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual long CountLong(Expression<Func<T, bool>> expWhere)
            => this.Query().LongCount(expWhere);

        /// <summary>
        /// 是否存在 根据条件
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual bool Any(Expression<Func<T, bool>> expWhere)
            => this.Query().Any(expWhere);

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <returns></returns>
        public virtual Task<int> CountAsync()
            => this.Query().CountAsync();

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <returns></returns>
        public virtual Task<long> CountLongAsync()
            => this.Query().LongCountAsync();

        /// <summary>
        /// 获取数量 根据条件
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual Task<int> CountAsync(Expression<Func<T, bool>> expWhere)
            => this.Query().CountAsync(expWhere);

        /// <summary>
        /// 获取数量 根据条件
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual Task<long> CountLongAsync(Expression<Func<T, bool>> expWhere)
            => this.Query().LongCountAsync(expWhere);

        /// <summary>
        /// 是否存在 根据条件
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual Task<bool> AnyAsync(Expression<Func<T, bool>> expWhere)
            => this.Query().AnyAsync(expWhere);

        #endregion

        #region 原生 sql 操作

        /// <summary>
        /// 查询根据sql语句
        /// EFCore 原生sql查询
        /// </summary>
        /// <returns> IQueryable </returns>
        public virtual IQueryable<T> QueryableBySql(string sql, params object[] parameters)
        {
            return this.UnitOfWork.DbSet<T>().FromSqlRaw(sql, parameters);
        }

        /// <summary>
        /// 根据 sql 查询表格
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual DataTable QueryDataTableBySql(string sql, params object[] parameters)
        {
            return Context.Database.QueryDataTableBySql(sql, parameters);
        }

        /// <summary>
        /// 根据 sql 查询表格
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual Task<DataTable> QueryDataTableBySqlAsync(string sql, params object[] parameters)
        {
            return Context.Database.QueryDataTableBySqlAsync(sql, parameters);
        }

        /// <summary>
        /// 根据 sql 查询字典集合
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual List<Dictionary<string, object>> QueryDicBySql(string sql, params object[] parameters)
        {
            return Context.Database.QueryDicBySql(sql, parameters);
        }

        /// <summary>
        /// 根据 sql 查询字典集合
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual Task<List<Dictionary<string, object>>> QueryDicBySqlAsync(string sql, params object[] parameters)
        {
            return Context.Database.QueryDicBySqlAsync(sql, parameters);
        }

        /// <summary>
        /// 查询根据sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual List<T> QueryBySql(string sql, params object[] parameters)
        {
            return Context.Database.QueryBySql<T>(sql, parameters);
        }

        /// <summary>
        /// 查询根据sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual Task<List<T>> QueryBySqlAsync(string sql, params object[] parameters)
        {
            return Context.Database.QueryBySqlAsync<T>(sql, parameters);
        }

        /// <summary>
        /// 查询根据sql返回单个值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual object QuerySingleBySql(string sql, params object[] parameters)
        {
            return Context.Database.QuerySingleBySql(sql, parameters);
        }

        /// <summary>
        /// 查询根据sql返回单个值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual Task<object> QuerySingleBySqlAsync(string sql, params object[] parameters)
        {
            return Context.Database.QuerySingleBySqlAsync(sql, parameters);
        }

        /// <summary>
        /// 查询根据sql返回单个值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual TResult QuerySingleBySql<TResult>(string sql, params object[] parameters)
            where TResult : struct
        {
            return Context.Database.QuerySingleBySql<TResult>(sql, parameters);
        }

        /// <summary>
        /// 查询根据sql返回单个值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual Task<TResult> QuerySingleBySqlAsync<TResult>(string sql, params object[] parameters)
          where TResult : struct
        {
            return Context.Database.QuerySingleBySqlAsync<TResult>(sql, parameters);
        }

        #endregion



    }
}
