namespace HZY.Framework.EntityFrameworkRepositories;

/// <summary>
/// EntityFrameworkRepositories 工具类
/// </summary>
public static class EntityFrameworkRepositoriesUtil
{
    /// <summary>
    /// key 是 dbset 得type fullname
    /// value 是 dbcontext 的 type
    /// </summary>
    private static readonly Dictionary<string, Type> _dbSetTypeFullNames = null;
    private static readonly List<Type> _allDbContextTypes = null;

    static EntityFrameworkRepositoriesUtil()
    {
        if (_dbSetTypeFullNames == null)
        {
            _dbSetTypeFullNames = new Dictionary<string, Type>();
        }
        if (_allDbContextTypes == null)
        {
            _allDbContextTypes = new List<Type>();
        }
    }

    /// <summary>
    /// 缓存dbcontext 类型
    /// </summary>
    /// <param name="key"></param>
    /// <param name="dbContextType"></param>
    public static void AddDbContextTypeByDbSetTypeFullName(string key, Type dbContextType)
    {
        if (!_dbSetTypeFullNames.ContainsKey(key))
        {
            _dbSetTypeFullNames.Add(key, dbContextType);
        }
    }

    /// <summary>
    /// 获取缓存的 dbcontext 类型
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static Type Get_DbSetTypeFullName_DbContextType(string key)
    {
        if (!_dbSetTypeFullNames.ContainsKey(key))
        {
            return _allDbContextTypes?.FirstOrDefault();
        }

        return _dbSetTypeFullNames[key];
    }

    /// <summary>
    /// 获取缓存的 dbcontext 类型
    /// </summary>
    /// <returns></returns>
    public static Dictionary<string, Type> Get_DbSetTypeFullName_DbContextType_All() => _dbSetTypeFullNames;

    /// <summary>
    /// 获取所有的 dbcontext
    /// </summary>
    /// <returns></returns>
    public static List<Type> GetDbContextTypeAll()
    {
        return _allDbContextTypes;
    }

    #region EntityFrameworkRepositories

    /// <summary>
    /// 注册 EntityFrameworkRepositories
    /// </summary>
    /// <param name="dbContextOptionsBuilder"></param>
    /// <param name="_isMonitor"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder AddEntityFrameworkRepositories(this DbContextOptionsBuilder dbContextOptionsBuilder, bool _isMonitor = true)
    {
        UseEntityFrameworkRepositories(dbContextOptionsBuilder.Options.ContextType);

        dbContextOptionsBuilder.AddInterceptors(new ShardingDbCommandInterceptor());
        //注册监控程序
        if (_isMonitor)
        {
            dbContextOptionsBuilder.AddInterceptors(new MonitorDbConnectionInterceptor());
            dbContextOptionsBuilder.AddInterceptors(new MonitorDbCommandInterceptor());
            dbContextOptionsBuilder.AddInterceptors(new MonitorDbTransactionInterceptor());
        }

        return dbContextOptionsBuilder;
    }

    /// <summary>
    /// 使用 EntityFrameworkRepositories
    /// </summary>
    /// <param name="dbContextTypes">数据上下文类型</param>
    /// <returns></returns>
    public static void UseEntityFrameworkRepositories(params Type[] dbContextTypes)
    {
        foreach (var item in dbContextTypes)
        {
            if (!_allDbContextTypes.Any(w => w.FullName == item.FullName))
            {
                _allDbContextTypes.Add(item);
            }

            //扫描类型下面的 dbset model
            var propertyInfos = item.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            var dbsets = propertyInfos.Where(w => w.PropertyType.Name == "DbSet`1");

            foreach (var dbset in dbsets)
            {
                if (dbset.PropertyType.GenericTypeArguments.Length <= 0) continue;

                var model = dbset.PropertyType.GenericTypeArguments[0];
                AddDbContextTypeByDbSetTypeFullName(model.FullName, item);
            }
        }
    }

    /// <summary>
    /// 使用 EntityFrameworkRepositories
    /// </summary>
    /// <param name="webApplicationBuilder"></param>
    /// <returns></returns>
    public static void UseEntityFrameworkRepositories<T>(this WebApplicationBuilder webApplicationBuilder)
    {
        UseEntityFrameworkRepositories(typeof(T));
    }

    #endregion


}
