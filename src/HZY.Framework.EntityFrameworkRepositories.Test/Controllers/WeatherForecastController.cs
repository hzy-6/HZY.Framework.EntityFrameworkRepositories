using HZY.Framework.EntityFrameworkRepositories;
using HZY.Framework.EntityFrameworkRepositories.Interceptor;
using HZY.Framework.EntityFrameworkRepositoriesTest.Repositories;
using HZY.Framework.EntityFrameworkRepositories.Extensions;
using HZY.Framework.EntityFrameworkRepositories.Monitor;
using HZY.Framework.EntityFrameworkRepositories.Monitor.Models;
using HZY.Framework.EntityFrameworkRepositories.Test.DbContexts;
using HZY.Framework.EntityFrameworkRepositories.Test.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HZY.Framework.EntityFrameworkRepositories.Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly AppDbContext _appDbContext;
        private readonly AppDbContext1 _appDbContext1;
        private readonly IServiceProvider _serviceProvider;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, AppDbContext appDbContext, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _appDbContext = appDbContext;
            _serviceProvider = serviceProvider;
            //_appDbContext1 = appDbContext1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("Update")]
        public async Task<string> Update()
        {
            var repository = new AppRepository<SysFunction>(_appDbContext);
            var sysFunction = await repository.Query().FirstOrDefaultAsync();
            sysFunction.CreationTime = DateTime.Now;
            repository.Update(sysFunction);

            if (sysFunction == null) return "OK";

            var resultCount = repository.UpdateBulk(w => new SysFunction
            {
                Name = sysFunction.Name

            },
            // where 条件
            w => w.Id == sysFunction.Id && w.Name == sysFunction.Name,
            option =>
            {
                // 忽略被 set 字段
                option.AddIgnore(w => w.Id);
            });

            return "Ok" + resultCount;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("Delete")]
        public async Task<string> Delete()
        {
            var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                  .UseSqlServer(@"Server=.;Database=hzy_admin_sqlserver_20220730;User ID=sa;Password=123456;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True;");
            contextOptions.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
            contextOptions.AddInterceptors(new ShardingDbCommandInterceptor());
            using var context = new AppDbContext(contextOptions.Options);
            var repository = new AppRepository<SysFunction>(context);
            var sysFunction = await repository.Query().AsTable("sys_function").FirstOrDefaultAsync();

            if (sysFunction == null) return "OK";

            var resultCount = await repository.DeleteBulkAsync(w => w.Id == sysFunction.Id && w.Name == sysFunction.Name);

            sysFunction = repository.Insert(sysFunction);

            return "Ok" + resultCount;
        }

        /// <summary>
        /// sql 查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("QueryBySql")]
        public async Task<string> QueryBySql()
        {
            var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(@"Server=.;Database=hzy_admin_sqlserver_20220920;User ID=sa;Password=123456;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True;");

            contextOptions.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));

            using var context = new AppDbContext(contextOptions.Options);

            var list = await context.Set<SysFunction>().FromSqlRaw("select * from sys_function").ToListAsync();

            var repository = new AppRepository<SysFunction>(context);

            var dt = repository.QueryDataTableBySql("select * from sys_function");
            var dt1 = await repository.QueryDataTableBySqlAsync("select * from sys_function");

            var listDic = repository.QueryDicBySql("select * from sys_function");
            var listDic1 = await repository.QueryDicBySqlAsync("select * from sys_function");

            var list1 = repository.QueryBySql("select * from sys_function");
            var list2 = await repository.QueryBySqlAsync("select * from sys_function");

            var id = repository.QuerySingleBySql<Guid>("select id from sys_function");
            var id1 = await repository.QuerySingleBySqlAsync<Guid>("select id from sys_function");

            var count = await repository.QuerySingleBySqlAsync<int>("select count(1) from sys_function");

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
        public EntityFrameworkRepositoriesMonitorContext EFCoreMonitorContext()
        {
            return EntityFrameworkRepositoriesMonitorCache.Context;
        }

        /// <summary>
        /// SqlMonitorContext sql 监控
        /// </summary>
        /// <returns></returns>
        [HttpGet("SqlMonitorContext")]
        public List<EntityFrameworkRepositoriesMonitorSqlContext> SqlMonitorContext()
        {
            return EntityFrameworkRepositoriesMonitorCache.SqlContext;
        }


    }
}