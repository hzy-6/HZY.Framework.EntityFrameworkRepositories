using HzyEFCoreRepositories.Extensions;
using HzyEFCoreRepositories.Monitor;
using HzyEFCoreRepositories.Monitor.Models;
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

        /// <summary>
        /// sql 查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("QueryBySql")]
        public async Task<string> QueryBySql()
        {
            var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(@"Server=.;Database=HzyAdminSpa20220410;User ID=sa;Password=123456;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True;");

            contextOptions.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));

            using var context = new AppDbContext(contextOptions.Options);

            var list = await context.Set<SysFunction>().FromSqlRaw("select * from SysFunction").ToListAsync();

            var repository = new AppRepository<SysFunction>(context);

            var dt = repository.QueryDataTableBySql("select * from SysFunction");
            var dt1 = await repository.QueryDataTableBySqlAsync("select * from SysFunction");

            var listDic = repository.QueryDicBySql("select * from SysFunction");
            var listDic1 = await repository.QueryDicBySqlAsync("select * from SysFunction");

            var list1 = repository.QueryBySql("select * from SysFunction");
            var list2 = await repository.QueryBySqlAsync("select * from SysFunction");

            var id = repository.QuerySingleBySql<Guid>("select id from SysFunction");
            var id1 = await repository.QuerySingleBySqlAsync<Guid>("select id from SysFunction");

            var model = await repository.SelectNoTracking.FirstOrDefaultAsync();
            model.ByName = model.ByName + "1";
            repository.DettachWhenExist(w => w.Id == model.Id);
            repository.Update(model);


            return "Ok" + list.Count();

        }

        /// <summary>
        /// sqlserver 批量拷贝
        /// </summary>
        /// <returns></returns>
        [HttpGet("SqlServerBulkCopyAsync")]
        public async Task<string> SqlServerBulkCopyAsync()
        {
            var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(@"Server=.;Database=HzyAdminSpa20220410;User ID=sa;Password=123456;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True;");

            contextOptions.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));

            using var context = new AppDbContext(contextOptions.Options);

            var list = await context.Set<SysFunction>().FromSqlRaw("select * from SysFunction").ToListAsync();

            var repository = new AppRepository<SysFunction>(context);

            //拷贝
            foreach (var item in list)
            {
                item.Id = Guid.NewGuid();
                item.Name += "批量拷贝";
            }
            await repository.SqlServerBulkCopyAsync(list);

            return "Ok" + list.Count();

        }

        /// <summary>
        /// mysql 批量拷贝
        /// </summary>
        /// <returns></returns>
        [HttpGet("MySqlBulkCopy")]
        public string MySqlBulkCopy()
        {
            //mysql
            var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                           .UseMySql(@"Server=localhost; port=3306; Database=HzyAdminSpa; uid=root; pwd=123456; Convert Zero Datetime=False;AllowLoadLocalInfile=true", MySqlServerVersion.LatestSupportedServerVersion, w => w.MinBatchSize(1).MaxBatchSize(1000));

            contextOptions.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));

            using var context = new AppDbContext(contextOptions.Options);

            var repository = new AppRepository<SysFunction>(context);

            var list = repository.QueryBySql("select * from SysFunction");

            repository.MySqlBulkCopy(list);

            return "Ok";
        }

        /// <summary>
        /// MyDbCommandInterceptor
        /// dbcommand 拦截
        /// </summary>
        /// <returns></returns>
        [HttpGet("MyDbCommandInterceptor")]
        public async Task<string> MyDbCommandInterceptor()
        {
            //mysql
            var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                           .UseMySql(@"Server=localhost; port=3306; Database=HzyAdminSpa; uid=root; pwd=123456; Convert Zero Datetime=False;AllowLoadLocalInfile=true", MySqlServerVersion.LatestSupportedServerVersion, w => w.MinBatchSize(1).MaxBatchSize(1000))
                           ;

            contextOptions.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));

            using var context = new AppDbContext(contextOptions.Options);

            var repository = new AppRepository<SysFunction>(context);

            var tableName = $"SysFunction_{DateTime.Now.ToString("yyyyMMdd")}";

            var list = await repository.Select.Where(w => w.ByName == "Insert").AsTable(tableName).ToListAsync();
            var list1 = await repository.Select.Where(w => w.ByName == "Update").AsTable(tableName).ToListAsync();

            var Count = repository.Select.AsTable(tableName).Count();

            return "Ok";
        }

        /// <summary>
        /// EFCoreMonitorContext efcore 程序监控
        /// </summary>
        /// <returns></returns>
        [HttpGet("EFCoreMonitorContext")]
        public EFCoreMonitorContext EFCoreMonitorContext()
        {
            return MonitorEFCoreCache.Context;
        }

        /// <summary>
        /// SqlMonitorContext sql 监控
        /// </summary>
        /// <returns></returns>
        [HttpGet("SqlMonitorContext")]
        public List<EFCoreMonitorSqlContext> SqlMonitorContext()
        {
            return MonitorEFCoreCache.SqlContext;
        }


    }
}