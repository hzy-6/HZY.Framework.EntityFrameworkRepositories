﻿namespace HZY.Framework.EntityFrameworkRepositories.Repositories;

/// <summary>
/// 基础仓储 查询 实现
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TDbContext"></typeparam>
public interface IQueryRepository<T, TDbContext> : IRepositoryCore<T, TDbContext>, IDisposable, IAsyncDisposable
    where T : class, new()
    where TDbContext : class
{
    #region 过滤

    /// <summary>
    /// 加入查询过滤条件
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    IQueryRepository<T, TDbContext> AddQueryFilter(Expression<Func<T, bool>> filter = null);
    /// <summary>
    /// 忽略查询过滤条件
    /// </summary>
    /// <returns></returns>
    IQueryRepository<T, TDbContext> IgnoreQueryFilter();
    /// <summary>
    /// 恢复忽略查询过滤条件
    /// </summary>
    /// <returns></returns>
    IQueryRepository<T, TDbContext> RecoveryQueryFilter();

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
    IQueryable<T> Select => Query(true);
    /// <summary>
    /// 查询 取消实体追踪
    /// </summary>
    IQueryable<T> SelectNoTracking => Query(false);

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="isTracking"></param>
    /// <returns></returns>
    IQueryable<T> GetAll(bool isTracking);

    #endregion

    #region 查询 单条

    /// <summary>
    /// 查询 根据条件
    /// </summary>
    /// <param name="expWhere"></param>
    /// <returns></returns>
    T Find(Expression<Func<T, bool>> expWhere);

    /// <summary>
    /// 查询 根据条件
    /// </summary>
    /// <param name="expWhere"></param>
    /// <returns></returns>
    T Get(Expression<Func<T, bool>> expWhere);

    /// <summary>
    /// 查询 根据id
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    T FindById<TKey>(TKey key);

    /// <summary>
    /// 查询 根据id
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    T GetById<TKey>(TKey key);

    /// <summary>
    /// 查询 根据条件
    /// </summary>
    /// <param name="expWhere"></param>
    /// <returns></returns>
    Task<T> FindAsync(Expression<Func<T, bool>> expWhere);

    /// <summary>
    /// 查询 根据条件
    /// </summary>
    /// <param name="expWhere"></param>
    /// <returns></returns>
    Task<T> GetAsync(Expression<Func<T, bool>> expWhere);

    /// <summary>
    /// 查询 根据id
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<T> FindByIdAsync<TKey>(TKey key);

    /// <summary>
    /// 查询 根据id
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<T> GetByIdAsync<TKey>(TKey key);

    #endregion

    #region 查询 多条

    /// <summary>
    /// 获取列表 根据查询条件
    /// </summary>
    /// <param name="expWhere"></param>
    /// <returns></returns>
    List<T> ToList(Expression<Func<T, bool>> expWhere);

    /// <summary>
    /// 获取列表 根据查询条件
    /// </summary>
    /// <param name="expWhere"></param>
    /// <returns></returns>
    List<T> GetAllList(Expression<Func<T, bool>> expWhere);

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <returns></returns>
    List<T> ToListAll();

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <returns></returns>
    List<T> GetAllList();

    /// <summary>
    /// 获取列表 根据查询条件
    /// </summary>
    /// <param name="expWhere"></param>
    /// <returns></returns>
    Task<List<T>> ToListAsync(Expression<Func<T, bool>> expWhere);

    /// <summary>
    /// 获取列表 根据查询条件
    /// </summary>
    /// <param name="expWhere"></param>
    /// <returns></returns>
    Task<List<T>> GetAllListAsync(Expression<Func<T, bool>> expWhere);

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <returns></returns>
    Task<List<T>> ToListAllAsync();

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <returns></returns>
    Task<List<T>> GetAllListAsync();

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

    #region 查询数据库结构

    /// <summary>
    /// 获取所有的表
    /// </summary>
    /// <returns></returns>
    List<TableModel> GetTables();

    /// <summary>
    /// 获取所有的列
    /// </summary>
    /// <returns></returns>
    List<ColumnModel> GetColumns();

    /// <summary>
    /// 获取所有的数据类型
    /// </summary>
    /// <returns></returns>
    List<DataTypeModel> GetDataTypes();

    #endregion

}
