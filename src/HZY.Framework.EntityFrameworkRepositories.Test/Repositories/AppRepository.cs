using HZY.Framework.EntityFrameworkRepositories.Test.DbContexts;
using HZY.Framework.EntityFrameworkRepositories.Repositories.Impl;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace HZY.Framework.EntityFrameworkRepositoriesTest.Repositories
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
