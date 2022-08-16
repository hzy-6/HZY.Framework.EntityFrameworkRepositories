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


    public class AppDbContext1 : DbContext
    {
        public AppDbContext1(DbContextOptions<AppDbContext1> options) : base(options)
        {

        }

        DbSet<SysFunction> SysFunction { get; set; }

    }


}
