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
using HzyEFCoreRepositories.Extensions.Parser;

namespace HzyEFCoreRepositories.Repositories
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    public interface IRepository<T, TDbContext> : IQueryRepository<T, TDbContext>, IDisposable, IAsyncDisposable
        where T : class, new()
        where TDbContext : class
    {

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
        /// 更新部分字段 根据where 条件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="where"></param>
        /// <param name="ignoreCols"></param>
        /// <returns></returns>
        int UpdateBulk(Expression<Func<T, T>> model, Expression<Func<T, bool>> where, Action<UpdateIgnoreParser<T>> ignoreCols = null);
        /// <summary>
        /// 更新部分字段 根据where 条件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="where"></param>
        /// <param name="ignoreCols"></param>
        /// <returns></returns>
        int UpdateBulk(T model, Expression<Func<T, bool>> where, Action<UpdateIgnoreParser<T>> ignoreCols = null);

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
        /// <summary>
        /// 更新部分字段 根据where 条件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="where"></param>
        /// <param name="ignoreCols"></param>
        /// <returns></returns>
        Task<int> UpdateBulkAsync(Expression<Func<T, T>> model, Expression<Func<T, bool>> where, Action<UpdateIgnoreParser<T>> ignoreCols = null);
        /// <summary>
        /// 更新部分字段 根据where 条件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="where"></param>
        /// <param name="ignoreCols"></param>
        /// <returns></returns>
        Task<int> UpdateBulkAsync(T model, Expression<Func<T, bool>> where, Action<UpdateIgnoreParser<T>> ignoreCols = null);

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
        /// 删除 根据表达式 直接 生成 delete from table  where 语句操作
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        int DeleteBulk(Expression<Func<T, bool>> expWhere);
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
        /// 删除 根据表达式 直接 生成 delete from table  where 语句操作
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        Task<int> DeleteBulkAsync(Expression<Func<T, bool>> expWhere);
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

    }

}
