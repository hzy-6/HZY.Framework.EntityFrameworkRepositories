using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HzyEFCoreRepositories.Repositories
{
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
        IQueryable<T> Query(bool? isTracking);
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
