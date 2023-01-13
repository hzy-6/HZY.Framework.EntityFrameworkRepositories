using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MySqlConnector;

namespace HZY.Framework.EntityFrameworkRepositories.Extensions
{
    /// <summary>
    /// mysql 批量拷贝封装
    /// <para>
    /// 需要开启服务端 mysql 的本地数据加载功能开关
    /// </para>
    /// <para>
    /// 1、请先查看本地加载数据是否开启使用此命令：SHOW GLOBAL VARIABLES LIKE 'local_infile';
    /// </para>
    /// <para>
    /// 2、使用此命令修改为 true 开启本地数据加载功能：SET GLOBAL local_infile = true;
    /// </para>
    /// </summary>
    public static class MySqlBulkCopyExtensions
    {
        /// <summary>
        /// mysql 批量拷贝数据
        /// <para>
        /// 需要开启服务端 mysql 的本地数据加载功能开关
        /// </para>
        /// <para>
        /// 1、请先查看本地加载数据是否开启使用此命令：SHOW GLOBAL VARIABLES LIKE 'local_infile';
        /// </para>
        /// <para>
        /// 2、使用此命令修改为 true 开启本地数据加载功能：SET GLOBAL local_infile = true;
        /// </para>
        /// </summary>
        /// <param name="database"></param>
        /// <param name="dataTable"></param>
        /// <param name="tableName"></param>
        /// <param name="dbTransaction"></param>
        public static void MySqlBulkCopy(this DatabaseFacade database, DataTable dataTable, string tableName, IDbTransaction dbTransaction)
        {
            if (!database.IsMySql())
            {
                throw new Exception("当前不是 MySql 数据库无法调用此函数!");
            }

            var dbConnection = (MySqlConnection)database.GetDbConnection();

            var sqlBulkCopy = new MySqlBulkCopy(dbConnection, (MySqlTransaction)dbTransaction);
            sqlBulkCopy.DestinationTableName = tableName;

            if (dbConnection.State != ConnectionState.Open)
            {
                dbConnection.Open();
            }

            try
            {
                sqlBulkCopy.WriteToServer(dataTable);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dbConnection.State == ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }
        }

        /// <summary>
        /// mysql 批量拷贝数据
        /// <para>
        /// 需要开启服务端 mysql 的本地数据加载功能开关
        /// </para>
        /// <para>
        /// 1、请先查看本地加载数据是否开启使用此命令：SHOW GLOBAL VARIABLES LIKE 'local_infile';
        /// </para>
        /// <para>
        /// 2、使用此命令修改为 true 开启本地数据加载功能：SET GLOBAL local_infile = true;
        /// </para>
        /// </summary>
        /// <param name="database"></param>
        /// <param name="dataTable"></param>
        /// <param name="tableName"></param>
        /// <param name="dbTransaction"></param>
        public static async Task MySqlBulkCopyAsync(this DatabaseFacade database, DataTable dataTable, string tableName, IDbTransaction dbTransaction)
        {
            if (!database.IsMySql())
            {
                throw new Exception("当前不是 MySql 数据库无法调用此函数!");
            }

            var dbConnection = (MySqlConnection)database.GetDbConnection();

            var sqlBulkCopy = new MySqlBulkCopy(dbConnection, (MySqlTransaction)dbTransaction);
            sqlBulkCopy.DestinationTableName = tableName;

            if (dbConnection.State != ConnectionState.Open)
            {
                await dbConnection.OpenAsync();
            }

            try
            {
                await sqlBulkCopy.WriteToServerAsync(dataTable);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dbConnection.State == ConnectionState.Open)
                {
                    await dbConnection.CloseAsync();
                }
            }
        }

        /// <summary>
        /// mysql 批量拷贝数据
        /// <para>
        /// 需要开启服务端 mysql 的本地数据加载功能开关
        /// </para>
        /// <para>
        /// 1、请先查看本地加载数据是否开启使用此命令：SHOW GLOBAL VARIABLES LIKE 'local_infile';
        /// </para>
        /// <para>
        /// 2、使用此命令修改为 true 开启本地数据加载功能：SET GLOBAL local_infile = true;
        /// </para>
        /// </summary>
        /// <param name="database"></param>
        /// <param name="items"></param>
        /// <param name="dbTransaction"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static void MySqlBulkCopy<T>(this DatabaseFacade database, List<T> items, IDbTransaction dbTransaction)
            where T : class, new()
        {
            var dataTable = items.ToDataTable();
            var type = typeof(T);
            var tableName = type.Name;
            var tableAttribute = type.GetTableAttribute();
            if (tableAttribute != null)
            {
                tableName = tableAttribute.Name;
            }
            database.MySqlBulkCopy(dataTable, tableName, dbTransaction);
        }

        /// <summary>
        /// mysql 批量拷贝数据
        /// <para>
        /// 需要开启服务端 mysql 的本地数据加载功能开关
        /// </para>
        /// <para>
        /// 1、请先查看本地加载数据是否开启使用此命令：SHOW GLOBAL VARIABLES LIKE 'local_infile';
        /// </para>
        /// <para>
        /// 2、使用此命令修改为 true 开启本地数据加载功能：SET GLOBAL local_infile = true;
        /// </para>
        /// </summary>
        /// <param name="database"></param>
        /// <param name="items"></param>
        /// <param name="dbTransaction"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Task MySqlBulkCopyAsync<T>(this DatabaseFacade database, List<T> items, IDbTransaction dbTransaction)
            where T : class, new()
        {
            var dataTable = items.ToDataTable();
            var type = typeof(T);
            var tableName = type.Name;
            var tableAttribute = type.GetTableAttribute();
            if (tableAttribute != null)
            {
                tableName = tableAttribute.Name;
            }

            return database.MySqlBulkCopyAsync(dataTable, tableName, dbTransaction);
        }





    }
}