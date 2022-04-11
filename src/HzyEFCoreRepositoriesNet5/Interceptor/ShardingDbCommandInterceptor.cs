using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using HzyEFCoreRepositories.Interceptor.Extensions;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace HzyEFCoreRepositories.Interceptor
{

    /// <summary>
    /// Ìæ»»±íÃûÀ¹½Ø sql
    /// Efcore À¹½Ø¼à¿ØÎÄµµ: https://docs.microsoft.com/en-us/ef/core/logging-events-diagnostics/interceptors
    /// </summary>
    public class ShardingDbCommandInterceptor : DbCommandInterceptor
    {
        /// <summary>
        /// ReaderExecuting
        /// </summary>
        /// <param name="command"></param>
        /// <param name="eventData"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        {
            ShardingDbCommandInterceptorExtensions.ReplaceTableName(command);
            return base.ReaderExecuting(command, eventData, result);
        }

        /// <summary>
        /// ReaderExecutingAsync
        /// </summary>
        /// <param name="command"></param>
        /// <param name="eventData"></param>
        /// <param name="result"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        {
            ShardingDbCommandInterceptorExtensions.ReplaceTableName(command);
            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }

        /// <summary>
        /// ScalarExecuting
        /// </summary>
        /// <param name="command"></param>
        /// <param name="eventData"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<object> result)
        {
            ShardingDbCommandInterceptorExtensions.ReplaceTableName(command);
            return base.ScalarExecuting(command, eventData, result);
        }

        /// <summary>
        /// ScalarExecutingAsync
        /// </summary>
        /// <param name="command"></param>
        /// <param name="eventData"></param>
        /// <param name="result"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<object> result, CancellationToken cancellationToken = default)
        {
            ShardingDbCommandInterceptorExtensions.ReplaceTableName(command);
            return base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
        }

    }
}