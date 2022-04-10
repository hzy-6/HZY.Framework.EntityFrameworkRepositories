using HzyEFCoreRepositories.DbContexts;
using HzyEFCoreRepositories.Extensions;
using HzyEFCoreRepositories.Interceptor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HzyEFCoreRepositories
{
    /// <summary>
    /// 自带拦截的 BaseDbContext
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public class InterceptorBaseDbContext<TDbContext> : BaseDbContext<TDbContext> where TDbContext : DbContext
    {
        private readonly bool _isMonitor;

        /// <summary>
        /// 自带拦截的 BaseDbContext
        /// </summary>
        /// <param name="options"></param>
        /// <param name="isMonitor">是否注册监控程序</param>
        public InterceptorBaseDbContext(DbContextOptions<TDbContext> options, bool isMonitor = true) : base(options)
        {
            _isMonitor = isMonitor;
        }

        /// <summary>
        /// OnConfiguring
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddEFCoreInterceptor(_isMonitor);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
