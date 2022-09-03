using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace HzyEFCoreRepositories.Extensions
{
    /// <summary>
    /// sqlserver 批量拷贝封装
    /// </summary>
    public static class SqlServerBulkCopyExtensions
    {
        /// <summary>
        /// sqlserver 批量拷贝数据
        /// </summary>
        /// <param name="database"></param>
        /// <param name="dataTable"></param>
        /// <param name="tableName"></param>
        /// <param name="dbTransaction"></param>
        public static void SqlServerBulkCopy(this DatabaseFacade database, DataTable dataTable, string tableName, IDbTransaction dbTransaction)
        {
            if (!database.IsSqlServer())
            {
                throw new Exception("当前不是 SqlServer 数据库无法调用此函数!");
            }
            var dbConnection = (SqlConnection)database.GetDbConnection();

            SqlBulkCopy sqlBulkCopy;
            if (dbTransaction == null)
            {
                sqlBulkCopy = new SqlBulkCopy(dbConnection);
            }
            else
            {
                sqlBulkCopy = new SqlBulkCopy(dbConnection, SqlBulkCopyOptions.Default, (SqlTransaction)dbTransaction);
            }

            sqlBulkCopy.DestinationTableName = tableName;
            sqlBulkCopy.BatchSize = dataTable.Rows.Count;
            foreach (DataColumn item in dataTable.Columns)
            {
                sqlBulkCopy.ColumnMappings.Add(item.ColumnName, item.ColumnName);
            }

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

                if (sqlBulkCopy != null)
                {
                    sqlBulkCopy.Close();
                }
            }
        }

        /// <summary>
        /// sqlserver 批量拷贝数据
        /// </summary>
        /// <param name="database"></param>
        /// <param name="dataTable"></param>
        /// <param name="tableName"></param>
        /// <param name="dbTransaction"></param>
        public static async Task SqlServerBulkCopyAsync(this DatabaseFacade database, DataTable dataTable, string tableName, IDbTransaction dbTransaction)
        {
            if (!database.IsSqlServer())
            {
                throw new Exception("当前不是 SqlServer 数据库无法调用此函数!");
            }
            var dbConnection = (SqlConnection)database.GetDbConnection();

            SqlBulkCopy sqlBulkCopy;
            if (dbTransaction == null)
            {
                sqlBulkCopy = new SqlBulkCopy(dbConnection);
            }
            else
            {
                sqlBulkCopy = new SqlBulkCopy(dbConnection, SqlBulkCopyOptions.Default, (SqlTransaction)dbTransaction);
            }
            sqlBulkCopy.DestinationTableName = tableName;
            sqlBulkCopy.BatchSize = dataTable.Rows.Count;
            foreach (DataColumn item in dataTable.Columns)
            {
                sqlBulkCopy.ColumnMappings.Add(item.ColumnName, item.ColumnName);
            }

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

                if (sqlBulkCopy != null)
                {
                    sqlBulkCopy.Close();
                }
            }
        }

        /// <summary>
        /// sqlserver 批量拷贝数据
        /// </summary>
        /// <param name="database"></param>
        /// <param name="items"></param>
        /// <param name="dbTransaction"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static void SqlServerBulkCopy<T>(this DatabaseFacade database, List<T> items, IDbTransaction dbTransaction)
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
            database.SqlServerBulkCopy(dataTable, tableName, dbTransaction);
        }

        /// <summary>
        /// sqlserver 批量拷贝数据
        /// </summary>
        /// <param name="database"></param>
        /// <param name="items"></param>
        /// <param name="dbTransaction"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Task SqlServerBulkCopyAsync<T>(this DatabaseFacade database, List<T> items, IDbTransaction dbTransaction)
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

            return database.SqlServerBulkCopyAsync(dataTable, tableName, dbTransaction);
        }









    }
}