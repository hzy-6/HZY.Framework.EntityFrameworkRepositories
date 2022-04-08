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

            var dt = repository.QueryDataTableBySql("select * from SysFunction");
            var dt1 = await repository.QueryDataTableBySqlAsync("select * from SysFunction");

            var listDic = repository.QueryDicBySql("select * from SysFunction");
            var listDic1 = await repository.QueryDicBySqlAsync("select * from SysFunction");

            var list1 = repository.QueryBySql<SysFunction>("select * from SysFunction");
            var list2 = await repository.QueryBySqlAsync<SysFunction>("select * from SysFunction");

            var id = repository.QueryScalarBySql<Guid>("select id from SysFunction");
            var id1 = await repository.QueryScalarBySqlAsync<Guid>("select id from SysFunction");



            return "Ok" + list.Count();

        }
    }
}