using HzyEFCoreRepositories.Monitor;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HzyEFCoreRepositories.Interceptor
{
    /// <summary>
    /// 监控数据库连接
    /// Efcore 拦截监控文档: https://docs.microsoft.com/en-us/ef/core/logging-events-diagnostics/interceptors
    /// </summary>
    public class MonitorDbConnectionInterceptor : DbConnectionInterceptor
    {
        /// <summary>
        /// ConnectionClosed
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="eventData"></param>
        public override void ConnectionClosed(DbConnection connection, ConnectionEndEventData eventData)
        {
            MonitorEFCoreCache.CloseDbConnectionCount();
            base.ConnectionClosed(connection, eventData);
        }

        /// <summary>
        /// ConnectionClosedAsync
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public override Task ConnectionClosedAsync(DbConnection connection, ConnectionEndEventData eventData)
        {
            MonitorEFCoreCache.CloseDbConnectionCount();
            return base.ConnectionClosedAsync(connection, eventData);
        }

        /// <summary>
        /// ConnectionFailed
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="eventData"></param>
        public override void ConnectionFailed(DbConnection connection, ConnectionErrorEventData eventData)
        {
            MonitorEFCoreCache.ConnectionFailedCount();
            base.ConnectionFailed(connection, eventData);
        }

        /// <summary>
        /// ConnectionFailedAsync
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="eventData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task ConnectionFailedAsync(DbConnection connection, ConnectionErrorEventData eventData, CancellationToken cancellationToken = default)
        {
            MonitorEFCoreCache.ConnectionFailedCount();
            return base.ConnectionFailedAsync(connection, eventData, cancellationToken);
        }

        /// <summary>
        /// ConnectionOpened
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="eventData"></param>
        public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
        {
            MonitorEFCoreCache.OpenDbConnectionCount();
            base.ConnectionOpened(connection, eventData);
        }

        /// <summary>
        /// ConnectionOpenedAsync
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="eventData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = default)
        {
            MonitorEFCoreCache.OpenDbConnectionCount();
            return base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
        }

    }
}
