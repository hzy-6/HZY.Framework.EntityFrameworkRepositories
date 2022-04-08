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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HzyEFCoreRepositories.Repositories
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="T">实体</typeparam>
    /// <typeparam name="TDbContext">dbcontext 数据上下文</typeparam>
    public interface IRepository<T, out TDbContext> where T : class where TDbContext : DbContext
    {
        /// <summary>
        /// 获取 dbcontext 对象
        /// </summary>
        TDbContext Orm => default;

        /// <summary>
        /// 设置 Attach
        /// </summary>
        /// <param name="model"></param>
        /// <param name="entityState"></param>
        void SetAttach(T model, EntityState entityState);

        /// <summary>
        /// 传入一个 值 获取实体 key 的 表达式
        /// </summary>
        /// <param name="value"></param>
        /// <returns> w => w.Id = 1 </returns>
        Expression<Func<T, bool>> GetKeyExpression(object value);

        #region 插入

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        T Insert(T model);
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int InsertRange(IEnumerable<T> model);

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<T> InsertAsync(T model);
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<T> model);

        #endregion

        #region 更新

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Update(T model);
        /// <summary>
        /// 根据实体id更新数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int UpdateById(T model);
        /// <summary>
        /// 更新，通过原模型 修改为新模型
        /// </summary>
        /// <param name="oldModel"></param>
        /// <param name="newModel"></param>
        /// <returns></returns>
        int Update(T oldModel, T newModel);
        /// <summary>
        /// 更新 批量
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        int UpdateRange(IEnumerable<T> models);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(T model);
        /// <summary>
        /// 根据实体id更新数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<int> UpdateByIdAsync(T model);
        /// <summary>
        /// 更新，通过原模型 修改为新模型
        /// </summary>
        /// <param name="oldModel"></param>
        /// <param name="newModel"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(T oldModel, T newModel);
        /// <summary>
        /// 更新 批量
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<T> models);

        #endregion

        #region 插入或者更新

        /// <summary>
        /// 插入或者添加 根据实体 主键判断是否添加还是修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        T InsertOrUpdate(T model);

        /// <summary>
        /// 插入或者添加 根据实体 主键判断是否添加还是修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<T> InsertOrUpdateAsync(T model);

        #endregion

        #region 删除

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Delete(T model);
        /// <summary>
        /// 删除 批量
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        int Delete(IEnumerable<T> models);
        /// <summary>
        /// 删除 根据表达式
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        int Delete(Expression<Func<T, bool>> expWhere);
        /// <summary>
        /// 删除 根据id
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        int DeleteById<TKey>(TKey key);
        /// <summary>
        /// 删除 根据 id集合
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        int DeleteByIds<TKey>(IEnumerable<TKey> keys);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(T model);
        /// <summary>
        /// 删除 批量
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(IEnumerable<T> models);
        /// <summary>
        /// 删除 根据表达式
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(Expression<Func<T, bool>> expWhere);
        /// <summary>
        /// 删除 根据 id
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<int> DeleteByIdAsync<TKey>(TKey key);
        /// <summary>
        /// 删除 根据id集合
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<int> DeleteByIdsAsync<TKey>(IEnumerable<TKey> keys);

        #endregion

        #region 查询 复杂型

        /// <summary>
        /// 加入查询过滤条件
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IRepository<T, TDbContext> AddQueryFilter(Expression<Func<T, bool>> filter = null);
        /// <summary>
        /// 忽略查询过滤条件
        /// </summary>
        /// <returns></returns>
        IRepository<T, TDbContext> IgnoreQueryFilter();
        /// <summary>
        /// 恢复忽略查询过滤条件
        /// </summary>
        /// <returns></returns>
        IRepository<T, TDbContext> RecoveryQueryFilter();
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="isTracking"></param>
        /// <returns></returns>
        IQueryable<T> Query(bool isTracking);
        /// <summary>
        /// 查询 带有实体追踪
        /// </summary>
        IQueryable<T> Select => this.Query(true);
        /// <summary>
        /// 查询 取消实体追踪
        /// </summary>
        IQueryable<T> SelectNoTracking => this.Query(false);

        #endregion

        #region 查询 单条

        /// <summary>
        /// 查询 根据条件
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        T Find(Expression<Func<T, bool>> expWhere);
        /// <summary>
        /// 查询 根据id
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T FindById<TKey>(TKey key);
        /// <summary>
        /// 查询 根据id集合
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        T FindByIds<TKey>(IEnumerable<TKey> keys);

        /// <summary>
        /// 查询 根据条件
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        Task<T> FindAsync(Expression<Func<T, bool>> expWhere);
        /// <summary>
        /// 查询 根据id
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> FindByIdAsync<TKey>(TKey key);
        /// <summary>
        /// 查询 根据id集合
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<T> FindByIdsAsync<TKey>(IEnumerable<TKey> keys);

        #endregion

        #region 查询 多条

        /// <summary>
        /// 获取列表 根据查询条件
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        List<T> ToList(Expression<Func<T, bool>> expWhere);
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        List<T> ToListAll();

        /// <summary>
        /// 获取列表 根据查询条件
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        Task<List<T>> ToListAsync(Expression<Func<T, bool>> expWhere);
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        Task<List<T>> ToListAllAsync();

        #endregion

        #region 是否存在 、 数量

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <returns></returns>
        int Count();
        /// <summary>
        /// 获取数量
        /// </summary>
        /// <returns></returns>
        long CountLong();
        /// <summary>
        /// 获取数量 根据条件
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> where);
        /// <summary>
        /// 获取数量 根据条件
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        long CountLong(Expression<Func<T, bool>> where);
        /// <summary>
        /// 是否存在 根据条件
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        bool Any(Expression<Func<T, bool>> where);

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <returns></returns>
        Task<int> CountAsync();
        /// <summary>
        /// 获取数量
        /// </summary>
        /// <returns></returns>
        Task<long> CountLongAsync();
        /// <summary>
        /// 获取数量 根据条件
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<int> CountAsync(Expression<Func<T, bool>> where);
        /// <summary>
        /// 获取数量 根据条件
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<long> CountLongAsync(Expression<Func<T, bool>> where);
        /// <summary>
        /// 是否存在 根据条件
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> where);

        #endregion

        #region 原生 sql 操作

        /// <summary>
        /// 执行sql 返回受影响的行数 insert|update|delete
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteSqlRaw(string sql, params object[] parameters);
        /// <summary>
        /// 执行sql 返回受影响的行数 insert|update|delete
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteSqlRaw(string sql, IEnumerable<object> parameters);

        /// <summary>
        /// 执行sql 返回受影响的行数 insert|update|delete
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> ExecuteSqlRawAsync(string sql, CancellationToken cancellationToken = default);
        /// <summary>
        /// 执行sql 返回受影响的行数 insert|update|delete
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters);
        /// <summary>
        /// 执行sql 返回受影响的行数 insert|update|delete
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> ExecuteSqlRawAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// 查询根据sql语句
        /// </summary>
        /// <returns> IQueryable </returns>
        IQueryable<T> QueryBySql(string sql, params object[] parameters);
        /// <summary>
        /// 根据 sql 查询表格
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DataTable QueryDataTableBySql(string sql, params object[] parameters);
        /// <summary>
        /// 根据 sql 查询表格
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<DataTable> QueryDataTableBySqlAsync(string sql, params object[] parameters);

        /// <summary>
        /// 根据 sql 查询字典集合
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        List<Dictionary<string, object>> QueryDicBySql(string sql, params object[] parameters);

        /// <summary>
        /// 根据 sql 查询字典集合
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<List<Dictionary<string, object>>> QueryDicBySqlAsync(string sql, params object[] parameters);

        /// <summary>
        /// 查询根据sql语句
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        List<TEntity> QueryBySql<TEntity>(string sql, params object[] parameters) where TEntity : class, new();

        /// <summary>
        /// 查询根据sql语句
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<List<TEntity>> QueryBySqlAsync<TEntity>(string sql, params object[] parameters) where TEntity : class, new();

        /// <summary>
        /// 查询根据sql返回单个值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        object? QueryScalarBySql(string sql, params object[] parameters);

        /// <summary>
        /// 查询根据sql返回单个值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        TResult QueryScalarBySql<TResult>(string sql, params object[] parameters)
            where TResult : struct;

        /// <summary>
        /// 查询根据sql返回单个值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<TResult> QueryScalarBySqlAsync<TResult>(string sql, params object[] parameters)
             where TResult : struct;

        #endregion

    }

}
