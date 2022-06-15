using HzyEFCoreRepositories.DbContexts;
using HzyEFCoreRepositories.Extensions;
using Microsoft.EntityFrameworkCore;

namespace HzyEFCoreRepositories
{
    /// <summary>
    /// 自带拦截的 BaseDbContext
    /// </summary>
    public class InterceptorDbContextBase : DbContextBase
    {
        private readonly bool _isMonitor;

        /// <summary>
        /// 自带拦截的 BaseDbContext
        /// </summary>
        protected InterceptorDbContextBase(bool isMonitor = true) : base()
        {
            _isMonitor = isMonitor;
        }

        /// <summary>
        /// 自带拦截的 BaseDbContext
        /// </summary>
        /// <param name="options"></param>
        /// <param name="isMonitor"></param>
        public InterceptorDbContextBase(DbContextOptions options, bool isMonitor = true) : base(options)
        {
            _isMonitor = isMonitor;
        }

        /// <summary>
        /// OnConfiguring
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddHzyEFCore(_isMonitor);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
