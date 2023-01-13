using HZY.Framework.EntityFrameworkRepositories;
using HZY.Framework.EntityFrameworkRepositories.Test.Models;
using Microsoft.EntityFrameworkCore;

namespace HZY.Framework.EntityFrameworkRepositories.Test.DbContexts
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
