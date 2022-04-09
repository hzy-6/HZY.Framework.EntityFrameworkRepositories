using HzyEFCoreRepositories.Extensions;
using HzyEFCoreRepositories.Repositories;
using HzyEFCoreRepositories.Repositories.Impl;
using HzyEFCoreRepositoriesTest.DbContexts;
using HzyEFCoreRepositoriesTest.Models;
using HzyEFCoreRepositoriesTest.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HzyEFCoreRepositoriesTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<string> Get()
        {
            var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(@"Server=.;Database=HzyAdminSpa20220318;User ID=sa;Password=123456;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True;")
                .Options;

            using var context = new AppDbContext(contextOptions);

            var list = await context.Set<SysFunction>().FromSqlRaw("select * from SysFunction").ToListAsync();

            var repository = new AppRepository<SysFunction>(context);

            //拷贝
            foreach (var item in list)
            {
                item.Id = Guid.NewGuid();
                item.Name += "批量拷贝";
            }
            await repository.Orm.Database.SqlServerBulkCopyAsync(list);

            var dt = repository.QueryDataTableBySql("select * from SysFunction");
            var dt1 = await repository.QueryDataTableBySqlAsync("select * from SysFunction");

            var listDic = repository.QueryDicBySql("select * from SysFunction");
            var listDic1 = await repository.QueryDicBySqlAsync("select * from SysFunction");

            var list1 = repository.QueryBySql("select * from SysFunction");
            var list2 = await repository.QueryBySqlAsync("select * from SysFunction");

            var id = repository.QuerySingleBySql<Guid>("select id from SysFunction");
            var id1 = await repository.QuerySingleBySqlAsync<Guid>("select id from SysFunction");

            //mysql
            var contextOptionsMySql = new DbContextOptionsBuilder<AppDbContext>()
                           .UseMySql(@"Server=localhost; port=3306; Database=HzyAdminSpa; uid=root; pwd=123456; Convert Zero Datetime=False;AllowLoadLocalInfile=true", MySqlServerVersion.LatestSupportedServerVersion, w => w.MinBatchSize(1).MaxBatchSize(1000))
                           .Options;

            using var context1 = new AppDbContext(contextOptionsMySql);

            var repositoryMySql = new AppRepository<SysFunction>(context1);

            repositoryMySql.Orm.Database.MySqlBulkCopy(list);

            return "Ok" + list.Count();

        }
    }
}