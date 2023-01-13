using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZY.Framework.EntityFrameworkRepositories.Monitor.Models
{
    /// <summary>
    /// efcore 监控上下文
    /// </summary>
    public class EntityFrameworkRepositoriesMonitorContext
    {
        #region IDbConnectionInterceptor

        /// <summary>
        /// 打开数据连接数
        /// </summary>
        public long OpenDbConnectionCount { get; set; } = 0;

        /// <summary>
        /// 关闭数据连接数
        /// </summary>
        public long CloseDbConnectionCount { get; set; } = 0;

        /// <summary>
        /// 连接失败数量
        /// </summary>
        public long ConnectionFailedCount { get; set; } = 0;

        #endregion

        #region IDbCommandInterceptor

        /// <summary>
        /// 创建命令数
        /// </summary>
        public long CreateCommandCount { get; set; } = 0;

        /// <summary>
        /// 执行命令数
        /// </summary>
        public long ExecuteCommandCount { get; set; } = 0;

        /// <summary>
        /// 命令执行失败
        /// </summary>
        public long CommandFailedCount { get; set; } = 0;

        #endregion

        #region IDbTransactionInterceptor

        /// <summary>
        /// 创建事务
        /// </summary>
        public long CreateTransactionCount { get; set; } = 0;

        /// <summary>
        /// 提交事务
        /// </summary>
        public long SubmitTransactionCount { get; set; } = 0;

        /// <summary>
        /// 回滚事务
        /// </summary>
        public long RollBackCount { get; set; } = 0;

        /// <summary>
        /// 事务失败
        /// </summary>
        public long TransactionFailedCount { get; set; } = 0;


        #endregion



    }
}
