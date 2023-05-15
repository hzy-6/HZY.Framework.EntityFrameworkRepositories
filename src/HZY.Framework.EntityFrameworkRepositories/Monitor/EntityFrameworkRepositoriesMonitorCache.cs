namespace HZY.Framework.EntityFrameworkRepositories.Monitor;

/// <summary>
/// 监控EFCore缓存
/// </summary>
public static class EntityFrameworkRepositoriesMonitorCache
{
    private readonly static IMemoryCache _memoryCache;
    private readonly static EntityFrameworkRepositoriesMonitorContext _eFCoreMonitorContext;
    private readonly static string EFCoreMonitorContextCacheKey = "EFCoreMonitorContext";
    private readonly static List<EntityFrameworkRepositoriesMonitorSqlContext> _eFCoreMonitorSqlContextList;
    private readonly static string EFCoreMonitorSqlContextListCacheKey = "EFCoreMonitorSqlContext";
    private readonly static int time = 1;

    static EntityFrameworkRepositoriesMonitorCache()
    {
        if (_memoryCache == null)
        {
            var memoryCacheOptions = new MemoryCacheOptions();
            _memoryCache = new MemoryCache(memoryCacheOptions);
            _memoryCache.Set(EFCoreMonitorContextCacheKey, new EntityFrameworkRepositoriesMonitorContext(), DateTime.Now.AddDays(time));
            _memoryCache.Set(EFCoreMonitorSqlContextListCacheKey, new List<EntityFrameworkRepositoriesMonitorSqlContext>(), DateTime.Now.AddDays(time));
        }

        _eFCoreMonitorContext = _memoryCache.Get<EntityFrameworkRepositoriesMonitorContext>(EFCoreMonitorContextCacheKey);
        _eFCoreMonitorSqlContextList = _memoryCache.Get<List<EntityFrameworkRepositoriesMonitorSqlContext>>(EFCoreMonitorSqlContextListCacheKey);

    }

    /// <summary>
    /// efcore 监控上下文
    /// </summary>
    public static EntityFrameworkRepositoriesMonitorContext Context => _eFCoreMonitorContext;

    /// <summary>
    /// sql 监控上下文
    /// </summary>
    public static List<EntityFrameworkRepositoriesMonitorSqlContext> SqlContext => _eFCoreMonitorSqlContextList;

    #region IDbConnectionInterceptor

    /// <summary>
    /// 设置打开连接数量
    /// </summary>
    public static void OpenDbConnectionCount()
    {
        _eFCoreMonitorContext.OpenDbConnectionCount++;
        _memoryCache.Set(EFCoreMonitorContextCacheKey, _eFCoreMonitorContext);
    }

    /// <summary>
    /// 设置关闭连接数量
    /// </summary>
    public static void CloseDbConnectionCount()
    {
        _eFCoreMonitorContext.CloseDbConnectionCount++;
        _memoryCache.Set(EFCoreMonitorContextCacheKey, _eFCoreMonitorContext);
    }

    /// <summary>
    /// 设置连接失败数量
    /// </summary>
    public static void ConnectionFailedCount()
    {
        _eFCoreMonitorContext.ConnectionFailedCount++;
        _memoryCache.Set(EFCoreMonitorContextCacheKey, _eFCoreMonitorContext);
    }

    #endregion

    #region IDbCommandInterceptor

    /// <summary>
    /// 创建命令数量
    /// </summary>
    public static void CreateCommandCount()
    {
        _eFCoreMonitorContext.CreateCommandCount++;
        _memoryCache.Set(EFCoreMonitorContextCacheKey, _eFCoreMonitorContext);
    }

    /// <summary>
    /// 执行命令数量
    /// </summary>
    public static void ExecuteCommandCount()
    {
        _eFCoreMonitorContext.ExecuteCommandCount++;
        _memoryCache.Set(EFCoreMonitorContextCacheKey, _eFCoreMonitorContext);
    }

    /// <summary>
    /// 命令执行失败数量
    /// </summary>
    public static void CommandFailedCount()
    {
        _eFCoreMonitorContext.ExecuteCommandCount++;
        _memoryCache.Set(EFCoreMonitorContextCacheKey, _eFCoreMonitorContext);
    }

    #endregion

    #region IDbTransactionInterceptor

    /// <summary>
    /// 创建事务 数量
    /// </summary>
    public static void CreateTransactionCount()
    {
        _eFCoreMonitorContext.CreateTransactionCount++;
        _memoryCache.Set(EFCoreMonitorContextCacheKey, _eFCoreMonitorContext);
    }

    /// <summary>
    /// 提交事务 数量
    /// </summary>
    public static void SubmitTransactionCount()
    {
        _eFCoreMonitorContext.SubmitTransactionCount++;
        _memoryCache.Set(EFCoreMonitorContextCacheKey, _eFCoreMonitorContext);
    }

    /// <summary>
    /// 回滚事务 数量
    /// </summary>
    public static void RollBackCount()
    {
        _eFCoreMonitorContext.RollBackCount++;
        _memoryCache.Set(EFCoreMonitorContextCacheKey, _eFCoreMonitorContext);
    }

    /// <summary>
    /// 事务失败 数量
    /// </summary>
    public static void TransactionFailedCount()
    {
        _eFCoreMonitorContext.TransactionFailedCount++;
        _memoryCache.Set(EFCoreMonitorContextCacheKey, _eFCoreMonitorContext);
    }

    #endregion

    /// <summary>
    /// 设置 sql 信息
    /// </summary>
    /// <param name="dbCommand"></param>
    /// <param name="ElapsedMilliseconds"></param>
    public static void SetSqlInfo(DbCommand dbCommand, long ElapsedMilliseconds)
    {
        _eFCoreMonitorSqlContextList.Add(new EntityFrameworkRepositoriesMonitorSqlContext { Sql = dbCommand.CommandText, ElapsedMilliseconds = ElapsedMilliseconds });
    }




}
