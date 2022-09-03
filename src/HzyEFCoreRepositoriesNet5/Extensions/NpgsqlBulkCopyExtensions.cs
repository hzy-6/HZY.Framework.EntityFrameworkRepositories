using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HzyEFCoreRepositories.Extensions
{
    /// <summary>
    /// Npgsql
    /// </summary>
    public static class NpgsqlBulkCopyExtensions
    {

        /// <summary>
        /// Npgsql 批量拷贝数据
        /// </summary>
        /// <param name="database"></param>
        /// <param name="dataTable"></param>
        /// <param name="tableName"></param>
        /// <param name="dbTransaction"></param>
        public static void NpgsqlBulkCopy(this DatabaseFacade database, DataTable dataTable, string tableName, IDbTransaction dbTransaction)
        {
            if (!database.IsNpgsql())
            {
                throw new Exception("当前不是 Npgsql 数据库无法调用此函数!");
            }
            var dbConnection = (NpgsqlConnection)database.GetDbConnection();

            int curIndex = 0;
            int batchSize = 10000;
            int totalCount = dataTable.Rows.Count;
            var fields = new List<string>();

            foreach (DataColumn item in dataTable.Columns)
            {
                fields.Add(item.ColumnName);
            }

            try
            {
                // 构建导入SQL
                var commandFormat = string.Format("COPY \"{0}\"({1}) FROM STDIN BINARY", tableName, string.Join(",", fields));
                while (curIndex < totalCount)
                {
                    var batchEntities = dataTable.AsEnumerable().Skip(curIndex).Take(batchSize);
                    var copyTable = new DataTable();
                    copyTable.LoadDataRow(batchEntities.ToArray(), true);

                    if (dbConnection.State != ConnectionState.Open)
                    {
                        dbConnection.Open();
                    }

                    using (var writer = dbConnection.BeginBinaryImport(commandFormat))
                    {
                        foreach (DataRow item in copyTable.Rows)
                        {
                            writer.WriteRow(item.ItemArray);
                        }

                        writer.Complete();
                    }

                    curIndex += batchSize;
                }
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
        /// Npgsql 批量拷贝数据
        /// </summary>
        /// <param name="database"></param>
        /// <param name="dataTable"></param>
        /// <param name="tableName"></param>
        /// <param name="dbTransaction"></param>
        /// <param name="cancellationToken"></param>
        public static async Task NpgsqlBulkCopyAsync(this DatabaseFacade database, DataTable dataTable, string tableName, IDbTransaction dbTransaction, CancellationToken cancellationToken = default)
        {
            if (!database.IsNpgsql())
            {
                throw new Exception("当前不是 Npgsql 数据库无法调用此函数!");
            }
            var dbConnection = (NpgsqlConnection)database.GetDbConnection();

            int curIndex = 0;
            int batchSize = 10000;
            int totalCount = dataTable.Rows.Count;
            var fields = new List<string>();

            foreach (DataColumn item in dataTable.Columns)
            {
                fields.Add(item.ColumnName);
            }

            try
            {
                // 构建导入SQL
                var commandFormat = string.Format("COPY \"{0}\"({1}) FROM STDIN BINARY", tableName, string.Join(",", fields));
                while (curIndex < totalCount)
                {
                    var batchEntities = dataTable.AsEnumerable().Skip(curIndex).Take(batchSize);
                    var copyTable = new DataTable();
                    copyTable.LoadDataRow(batchEntities.ToArray(), true);

                    if (dbConnection.State != ConnectionState.Open)
                    {
                        await dbConnection.OpenAsync();
                    }

                    using (var writer = dbConnection.BeginBinaryImport(commandFormat))
                    {
                        foreach (DataRow item in copyTable.Rows)
                        {
                            await writer.WriteRowAsync(cancellationToken, item.ItemArray);
                        }

                        await writer.CompleteAsync(cancellationToken);
                    }

                    curIndex += batchSize;
                }
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
        /// Npgsql 批量拷贝数据
        /// </summary>
        /// <param name="database"></param>
        /// <param name="items"></param>
        /// <param name="dbTransaction"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static void NpgsqlBulkCopy<T>(this DatabaseFacade database, List<T> items, IDbTransaction dbTransaction)
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
            database.NpgsqlBulkCopy(dataTable, tableName, dbTransaction);
        }

        /// <summary>
        /// Npgsql 批量拷贝数据
        /// </summary>
        /// <param name="database"></param>
        /// <param name="items"></param>
        /// <param name="dbTransaction"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Task NpgsqlBulkCopyAsync<T>(this DatabaseFacade database, List<T> items, IDbTransaction dbTransaction)
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

            return database.NpgsqlBulkCopyAsync(dataTable, tableName, dbTransaction);
        }








    }
}
