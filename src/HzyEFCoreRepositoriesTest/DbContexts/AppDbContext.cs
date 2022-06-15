using HzyEFCoreRepositories;
using HzyEFCoreRepositories.DbContexts;
using HzyEFCoreRepositoriesTest.Models;
using Microsoft.EntityFrameworkCore;

namespace HzyEFCoreRepositoriesTest.DbContexts
{
    public class AppDbContext : InterceptorDbContextBase
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        DbSet<SysFunction> SysFunction { get; set; }

    }


    public class AppDbContext1 : DbContextBase
    {
        public AppDbContext1(DbContextOptions<AppDbContext1> options) : base(options)
        {

        }

        DbSet<SysFunction> SysFunction { get; set; }

    }


}
