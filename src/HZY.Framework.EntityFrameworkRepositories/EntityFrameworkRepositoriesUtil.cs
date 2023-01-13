using HZY.Framework.EntityFrameworkRepositories.Interceptor;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HZY.Framework.EntityFrameworkRepositories
{
    /// <summary>
    /// EntityFrameworkRepositories 工具类
    /// </summary>
    public static class EntityFrameworkRepositoriesUtil
    {
        private static Dictionary<string, Type> _dbContextTypes = null;
        private static List<Type> _allDbContextTypes = null;

        static EntityFrameworkRepositoriesUtil()
        {
            if (_dbContextTypes == null)
            {
                _dbContextTypes = new Dictionary<string, Type>();
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
        public static void AddDbContextTypeByKey(string key, Type dbContextType)
        {
            _dbContextTypes.Add(key, dbContextType);
        }

        /// <summary>
        /// 获取缓存的 dbcontext 类型
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Type GetDbContextTypeByKey(string key)
        {
            return _dbContextTypes[key];
        }

        /// <summary>
        /// 获取所有的 dbcontext
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetAllDbContextType()
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
        /// <param name="app"></param>
        /// <param name="dbContextTypes">数据上下文类型</param>
        /// <returns></returns>
        public static void UseEntityFrameworkRepositories(this IApplicationBuilder app, params Type[] dbContextTypes)
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
                    AddDbContextTypeByKey(model.FullName, item);
                }
            }

        }

        #endregion




    }
}
