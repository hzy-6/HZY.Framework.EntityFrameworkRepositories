using HzyEFCoreRepositories.DbContexts;
using HzyEFCoreRepositoriesTest.Models;
using Microsoft.EntityFrameworkCore;

namespace HzyEFCoreRepositoriesTest.DbContexts
{
    public class AppDbContext : BaseDbContext<AppDbContext>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        DbSet<SysFunction> SysFunction { get; set; }

    }
}
