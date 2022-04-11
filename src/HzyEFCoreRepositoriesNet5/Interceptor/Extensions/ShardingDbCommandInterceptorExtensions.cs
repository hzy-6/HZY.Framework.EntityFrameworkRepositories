using HzyEFCoreRepositories.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HzyEFCoreRepositories.Interceptor.Extensions
{
    /// <summary>
    /// DbCommandInterceptor 扩展
    /// </summary>
    public class ShardingDbCommandInterceptorExtensions
    {

        /// <summary>
        /// 替换表名称
        /// </summary>
        /// <param name="command"></param>
        public static void ReplaceTableName(DbCommand command)
        {
            //替换表名
            if (!command.CommandText.StartsWith($"-- {HzyEFCoreRepositoriesConfig.ShardingName}:")) return;

            var fragment = command.CommandText.Split("\r\n");

            var replaceCmd = fragment[0];

            var oldTableName = replaceCmd.Split(":")[1];
            var newTableName = replaceCmd.Split(":")[2];

            var sql = command.CommandText.Replace(replaceCmd, "");

            command.CommandText = replaceCmd + sql.Replace(oldTableName, newTableName);
        }





    }
}
