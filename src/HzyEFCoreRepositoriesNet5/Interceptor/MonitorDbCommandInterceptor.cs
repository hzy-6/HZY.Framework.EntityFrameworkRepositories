using HzyEFCoreRepositories.Monitor;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HzyEFCoreRepositories.Interceptor
{
    /// <summary>
    /// 监控 command 
    /// Efcore 拦截监控文档: https://docs.microsoft.com/en-us/ef/core/logging-events-diagnostics/interceptors
    /// </summary>
    public class MonitorDbCommandInterceptor : DbCommandInterceptor
    {
        private readonly Stopwatch _stopwatch;

        /// <summary>
        /// MonitorDbCommandInterceptor
        /// </summary>
        public MonitorDbCommandInterceptor()
        {
            this._stopwatch ??= new Stopwatch();
        }

        /// <summary>
        /// CommandCreated
        /// </summary>
        /// <param name="eventData"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override DbCommand CommandCreated(CommandEndEventData eventData, DbCommand result)
        {
            MonitorEFCoreCache.CreateCommandCount();
            return base.CommandCreated(eventData, result);
        }

        /// <summary>
        /// CommandFailed
        /// </summary>
        /// <param name="command"></param>
        /// <param name="eventData"></param>
        public override void CommandFailed(DbCommand command, CommandErrorEventData eventData)
        {
            MonitorEFCoreCache.CommandFailedCount();
            //记录 api 执行耗时
            _stopwatch.Stop();
            MonitorEFCoreCache.SetSqlInfo(command, _stopwatch.ElapsedMilliseconds);
            base.CommandFailed(command, eventData);
        }

        /// <summary>
        /// CommandFailedAsync
        /// </summary>
        /// <param name="command"></param>
        /// <param name="eventData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task CommandFailedAsync(DbCommand command, CommandErrorEventData eventData, CancellationToken cancellationToken = default)
        {
            MonitorEFCoreCache.CommandFailedCount();
            //记录 api 执行耗时
            _stopwatch.Stop();
            MonitorEFCoreCache.SetSqlInfo(command, _stopwatch.ElapsedMilliseconds);
            return base.CommandFailedAsync(command, eventData, cancellationToken);
        }

        /// <summary>
        /// NonQueryExecuted
        /// </summary>
        /// <param name="command"></param>
        /// <param name="eventData"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override int NonQueryExecuted(DbCommand command, CommandExecutedEventData eventData, int result)
        {
            MonitorEFCoreCache.ExecuteCommandCount();
            //记录 api 执行耗时
            _stopwatch.Stop();
            MonitorEFCoreCache.SetSqlInfo(command, _stopwatch.ElapsedMilliseconds);
            return base.NonQueryExecuted(command, eventData, result);
        }

        /// <summary>
        /// NonQueryExecutedAsync
        /// </summary>
        /// <param name="command"></param>
        /// <param name="eventData"></param>
        /// <param name="result"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override ValueTask<int> NonQueryExecutedAsync(DbCommand command, CommandExecutedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            MonitorEFCoreCache.ExecuteCommandCount();
            //记录 api 执行耗时
            _stopwatch.Stop();
            MonitorEFCoreCache.SetSqlInfo(command, _stopwatch.ElapsedMilliseconds);
            return base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
        }

        public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
        {
            //记录 api 执行耗时
            _stopwatch.Restart();
            return base.NonQueryExecuting(command, eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            //记录 api 执行耗时
            _stopwatch.Restart();
            return base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
        }

        /// <summary>
        /// ReaderExecuted
        /// </summary>
        /// <param name="command"></param>
        /// <param name="eventData"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
        {
            MonitorEFCoreCache.ExecuteCommandCount();
            //记录 api 执行耗时
            _stopwatch.Stop();
            MonitorEFCoreCache.SetSqlInfo(command, _stopwatch.ElapsedMilliseconds);
            return base.ReaderExecuted(command, eventData, result);
        }

        /// <summary>
        /// ReaderExecutedAsync
        /// </summary>
        /// <param name="command"></param>
        /// <param name="eventData"></param>
        /// <param name="result"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
        {
            MonitorEFCoreCache.ExecuteCommandCount();
            //记录 api 执行耗时
            _stopwatch.Stop();
            MonitorEFCoreCache.SetSqlInfo(command, _stopwatch.ElapsedMilliseconds);
            return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
        }

        public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        {
            //记录 api 执行耗时
            _stopwatch.Restart();
            return base.ReaderExecuting(command, eventData, result);
        }

        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        {
            //记录 api 执行耗时
            _stopwatch.Restart();
            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }

        /// <summary>
        /// ScalarExecuted
        /// </summary>
        /// <param name="command"></param>
        /// <param name="eventData"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override object ScalarExecuted(DbCommand command, CommandExecutedEventData eventData, object result)
        {
            MonitorEFCoreCache.ExecuteCommandCount();
            //记录 api 执行耗时
            _stopwatch.Stop();
            MonitorEFCoreCache.SetSqlInfo(command, _stopwatch.ElapsedMilliseconds);
            return base.ScalarExecuted(command, eventData, result);
        }

        /// <summary>
        /// ScalarExecutedAsync
        /// </summary>
        /// <param name="command"></param>
        /// <param name="eventData"></param>
        /// <param name="result"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override ValueTask<object> ScalarExecutedAsync(DbCommand command, CommandExecutedEventData eventData, object result, CancellationToken cancellationToken = default)
        {
            MonitorEFCoreCache.ExecuteCommandCount();
            //记录 api 执行耗时
            _stopwatch.Stop();
            MonitorEFCoreCache.SetSqlInfo(command, _stopwatch.ElapsedMilliseconds);
            return base.ScalarExecutedAsync(command, eventData, result, cancellationToken);
        }

        public override InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<object> result)
        {
            //记录 api 执行耗时
            _stopwatch.Restart();
            return base.ScalarExecuting(command, eventData, result);
        }

        public override ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<object> result, CancellationToken cancellationToken = default)
        {
            //记录 api 执行耗时
            _stopwatch.Restart();
            return base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
        }
    }
}
