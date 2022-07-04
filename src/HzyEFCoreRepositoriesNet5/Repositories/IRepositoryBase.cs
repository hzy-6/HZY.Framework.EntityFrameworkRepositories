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
using HzyEFCoreRepositories.DbContexts;
using HzyEFCoreRepositories.Extensions.Parser;
using Microsoft.EntityFrameworkCore;

namespace HzyEFCoreRepositories.Repositories
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    public interface IRepositoryBase<T, TDbContext> : IDisposable, IAsyncDisposable
        where T : class, new()
        where TDbContext : class
    {
        /// <summary>
        /// 设置 Attach
        /// </summary>
        /// <param name="model"></param>
        /// <param name="entityState"></param>
        void SetEntityState(T model, EntityState entityState);

        /// <summary>
        /// 取消了实体对象的追踪操作 需要调用此函数 才能进行对实体数据库操作
        /// <para>
        /// 用于取消旧实体追踪缓存 防止出现 id 重复问题
        /// </para>
        ///  
        /// <para>
        /// 此函数解决的问题可以看此案例： https://blog.51cto.com/u_15064638/4401901
        /// </para>
        /// 
        /// </summary>
        /// <param name="detachedWhere"></param>
        void DettachWhenExist(Func<T, bool> detachedWhere);

        /// <summary>
        /// 传入一个 值 获取实体 key 的 表达式
        /// </summary>
        /// <param name="value"></param>
        /// <returns> w => w.Id = 1 </returns>
        Expression<Func<T, bool>> GetKeyExpression(object value);

        /// <summary>
        /// 获取数据上下文
        /// </summary>
        /// <returns></returns>
        TDbContext Orm => default;

        /// <summary>
        /// 获取数据上下文
        /// </summary>
        /// <typeparam name="TDbContextResult"></typeparam>
        /// <returns></returns>
        TDbContextResult GetDbContext<TDbContextResult>() where TDbContextResult : DbContextBase;

        /// <summary>
        /// 获取数据上下文 基础对象 DbContextBase
        /// </summary>
        /// <returns></returns>
        DbContextBase DbContextBase => default;

        /// <summary>
        /// 工作单元
        /// </summary>
        IUnitOfWork UnitOfWork => this.DbContextBase.UnitOfWork;

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

        #region 查询 复杂型

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
        IQueryable<T> QueryableBySql(string sql, params object[] parameters);
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
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        List<T> QueryBySql(string sql, params object[] parameters);

        /// <summary>
        /// 查询根据sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<List<T>> QueryBySqlAsync(string sql, params object[] parameters);

        /// <summary>
        /// 查询根据sql返回单个值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        object QuerySingleBySql(string sql, params object[] parameters);

        /// <summary>
        /// 查询根据sql返回单个值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<object> QuerySingleBySqlAsync(string sql, params object[] parameters);

        /// <summary>
        /// 查询根据sql返回单个值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        TResult QuerySingleBySql<TResult>(string sql, params object[] parameters)
            where TResult : struct;

        /// <summary>
        /// 查询根据sql返回单个值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<TResult> QuerySingleBySqlAsync<TResult>(string sql, params object[] parameters)
             where TResult : struct;

        #endregion

        #region  数据批量拷贝

        /// <summary>
        /// Sqlserver 数据拷贝
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="tableName"></param>
        /// <param name="dbTransaction"></param>
        void SqlServerBulkCopy(DataTable dataTable, string tableName, IDbTransaction dbTransaction = null);

        /// <summary>
        /// Sqlserver 数据拷贝
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="tableName"></param>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        Task SqlServerBulkCopyAsync(DataTable dataTable, string tableName, IDbTransaction dbTransaction = null);

        /// <summary>
        /// Sqlserver 数据拷贝
        /// </summary>
        /// <param name="items"></param>
        /// <param name="dbTransaction"></param>
        void SqlServerBulkCopy(List<T> items, IDbTransaction dbTransaction = null);

        /// <summary>
        /// Sqlserver 数据拷贝
        /// </summary>
        /// <param name="items"></param>
        /// <param name="dbTransaction"></param>
        Task SqlServerBulkCopyAsync(List<T> items, IDbTransaction dbTransaction = null);

        /// <summary>
        /// mysql 批量拷贝数据
        /// <para>
        /// 需要开启服务端 mysql 的本地数据加载功能开关
        /// </para>
        /// <para>
        /// 1、请先查看本地加载数据是否开启使用此命令：SHOW GLOBAL VARIABLES LIKE 'local_infile';
        /// </para>
        /// <para>
        /// 2、使用此命令修改为 true 开启本地数据加载功能：SET GLOBAL local_infile = true;
        /// </para>
        /// <para>
        /// 3、连接字符串中添加此属性：AllowLoadLocalInfile=true
        /// </para>
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="tableName"></param>
        /// <param name="dbTransaction"></param>
        void MySqlBulkCopy(DataTable dataTable, string tableName, IDbTransaction dbTransaction = null);

        /// <summary>
        /// mysql 批量拷贝数据
        /// <para>
        /// 需要开启服务端 mysql 的本地数据加载功能开关
        /// </para>
        /// <para>
        /// 1、请先查看本地加载数据是否开启使用此命令：SHOW GLOBAL VARIABLES LIKE 'local_infile';
        /// </para>
        /// <para>
        /// 2、使用此命令修改为 true 开启本地数据加载功能：SET GLOBAL local_infile = true;
        /// </para>
        /// <para>
        /// 3、连接字符串中添加此属性：AllowLoadLocalInfile=true
        /// </para>
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="tableName"></param>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        Task MySqlBulkCopyAsync(DataTable dataTable, string tableName, IDbTransaction dbTransaction = null);

        /// <summary>
        /// mysql 批量拷贝数据
        /// <para>
        /// 需要开启服务端 mysql 的本地数据加载功能开关
        /// </para>
        /// <para>
        /// 1、请先查看本地加载数据是否开启使用此命令：SHOW GLOBAL VARIABLES LIKE 'local_infile';
        /// </para>
        /// <para>
        /// 2、使用此命令修改为 true 开启本地数据加载功能：SET GLOBAL local_infile = true;
        /// </para>
        /// <para>
        /// 3、连接字符串中添加此属性：AllowLoadLocalInfile=true
        /// </para>
        /// </summary>
        /// <param name="items"></param>
        /// <param name="dbTransaction"></param>
        void MySqlBulkCopy(List<T> items, IDbTransaction dbTransaction = null);

        /// <summary>
        /// mysql 批量拷贝数据
        /// <para>
        /// 需要开启服务端 mysql 的本地数据加载功能开关
        /// </para>
        /// <para>
        /// 1、请先查看本地加载数据是否开启使用此命令：SHOW GLOBAL VARIABLES LIKE 'local_infile';
        /// </para>
        /// <para>
        /// 2、使用此命令修改为 true 开启本地数据加载功能：SET GLOBAL local_infile = true;
        /// </para>
        /// <para>
        /// 3、连接字符串中添加此属性：AllowLoadLocalInfile=true
        /// </para>
        /// </summary>
        /// <param name="items"></param>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        Task MySqlBulkCopyAsync(List<T> items, IDbTransaction dbTransaction = null);
        #endregion


    }

}
