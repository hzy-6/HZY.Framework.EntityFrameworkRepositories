using HzyEFCoreRepositories;
using HzyEFCoreRepositoriesTest.Models;
using Microsoft.EntityFrameworkCore;

namespace HzyEFCoreRepositoriesTest.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        DbSet<SysFunction> SysFunction { get; set; }

    }
}
