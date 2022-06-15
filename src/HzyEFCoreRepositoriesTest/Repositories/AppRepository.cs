using HzyEFCoreRepositories.Repositories;
using HzyEFCoreRepositories.Repositories.Impl;
using HzyEFCoreRepositoriesTest.DbContexts;
using System.Linq.Expressions;

namespace HzyEFCoreRepositoriesTest.Repositories
{
    public class AppRepository1<T> : AppRepositoryImpl<T> where T : class, new()
    {
        public AppRepository1(Expression<Func<T, bool>> filter = null) : base(filter)
        {

        }
    }

    public class AppRepository<T> : AppRepositoryImpl<T, AppDbContext> where T : class, new()
    {
        public AppRepository(AppDbContext appDbContext, Expression<Func<T, bool>> filter = null) : base(appDbContext, filter)
        {

        }
    }
}
