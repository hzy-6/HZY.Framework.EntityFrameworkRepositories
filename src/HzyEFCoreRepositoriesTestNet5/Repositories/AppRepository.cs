using HzyEFCoreRepositories.Repositories;
using HzyEFCoreRepositories.Repositories.Impl;
using HzyEFCoreRepositoriesTest.DbContexts;
using System;
using System.Linq.Expressions;

namespace HzyEFCoreRepositoriesTest.Repositories
{
    public class AppRepository<T> : AppRepositoryImpl<T, AppDbContext> where T : class, new()
    {
        public AppRepository(AppDbContext context, Expression<Func<T, bool>> filter = null) : base(context, filter)
        {
        }
    }
}
