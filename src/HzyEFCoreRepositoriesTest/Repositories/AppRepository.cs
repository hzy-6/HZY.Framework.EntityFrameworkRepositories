using HzyEFCoreRepositories.Repositories;
using HzyEFCoreRepositories.Repositories.Impl;
using HzyEFCoreRepositoriesTest.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HzyEFCoreRepositoriesTest.Repositories
{
    public class AppRepository1<T> : RepositoryBaseImpl<T, DbContext> where T : class, new()
    {
        public AppRepository1(Expression<Func<T, bool>> filter = null) : base(null, filter)
        {

        }

        public override DbContext Orm => base.Orm;
    }

    public class AppRepository<T> : RepositoryBaseImpl<T, AppDbContext> where T : class, new()
    {
        public AppRepository(AppDbContext appDbContext, Expression<Func<T, bool>> filter = null) : base(appDbContext, filter)
        {

        }
    }
}
