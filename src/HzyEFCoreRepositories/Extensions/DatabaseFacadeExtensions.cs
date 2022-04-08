﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HzyEFCoreRepositories.Extensions
{
    /// <summary>
    /// DatabaseFacade 扩展
    /// </summary>
    public static class DatabaseFacadeExtensions
    {
        /// <summary>
        /// 根据 sql 查询表格
        /// </summary>
        /// <param name="database"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable QueryDataTableBySql(this DatabaseFacade database, string sql, params object[] parameters)
        {
            var dbConnection = database.GetDbConnection();
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                command.Parameters.AddRange(parameters);

                var reader = command.ExecuteReader();
                var dt = new DataTable();

                int fieldCount = reader.FieldCount;
                for (int i = 0; i < fieldCount; i++)
                {
                    dt.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
                }

                dt.BeginLoadData();

                object[] objValues = new object[fieldCount];
                while (reader.Read())
                {
                    reader.GetValues(objValues);
                    dt.LoadDataRow(objValues, true);
                }

                dbConnection.Close();
                return dt;
            }

        }

        /// <summary>
        /// 根据 sql 查询表格
        /// </summary>
        /// <param name="database"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static async Task<DataTable> QueryDataTableBySqlAsync(this DatabaseFacade database, string sql, params object[] parameters)
        {
            var dbConnection = database.GetDbConnection();
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                if (command.Connection.State != ConnectionState.Open)
                {
                    await command.Connection.OpenAsync();
                }

                command.Parameters.AddRange(parameters);

                var reader = await command.ExecuteReaderAsync();
                var dt = new DataTable();

                int fieldCount = reader.FieldCount;
                for (var i = 0; i < fieldCount; i++)
                {
                    dt.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
                }

                dt.BeginLoadData();

                object[] objValues = new object[fieldCount];
                while (reader.Read())
                {
                    reader.GetValues(objValues);
                    dt.LoadDataRow(objValues, true);
                }

                await dbConnection.CloseAsync();

                return dt;
            }
        }

        /// <summary>
        /// 根据 sql 查询字典集合
        /// </summary>
        /// <param name="database"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> QueryDicBySql(this DatabaseFacade database, string sql, params object[] parameters)
        {
            var dbConnection = database.GetDbConnection();
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                command.Parameters.AddRange(parameters);

                var reader = command.ExecuteReader();

                var fieldCount = reader.FieldCount;
                var columns = new List<string>();
                for (int i = 0; i < fieldCount; i++)
                {
                    columns.Add(reader.GetName(i));
                }

                var result = new List<Dictionary<string, object>>();

                while (reader.Read())
                {
                    var dic = new Dictionary<string, object>();

                    foreach (var item in columns)
                    {
                        dic[item] = reader.GetValue(item);
                    }

                    result.Add(dic);
                }

                dbConnection.Close();

                return result;
            }
        }

        /// <summary>
        /// 根据 sql 查询字典集合
        /// </summary>
        /// <param name="database"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static async Task<List<Dictionary<string, object>>> QueryDicBySqlAsync(this DatabaseFacade database, string sql, params object[] parameters)
        {
            var dbConnection = database.GetDbConnection();
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                if (command.Connection.State != ConnectionState.Open)
                {
                    await command.Connection.OpenAsync();
                }

                command.Parameters.AddRange(parameters);

                var reader = await command.ExecuteReaderAsync();

                var fieldCount = reader.FieldCount;
                var columns = new List<string>();
                for (int i = 0; i < fieldCount; i++)
                {
                    columns.Add(reader.GetName(i));
                }

                var result = new List<Dictionary<string, object>>();

                while (reader.Read())
                {
                    var dic = new Dictionary<string, object>();

                    foreach (var item in columns)
                    {
                        dic[item] = reader.GetValue(item);
                    }

                    result.Add(dic);
                }

                await dbConnection.CloseAsync();

                return result;
            }
        }

        /// <summary>
        /// 查询根据sql语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static List<T> QueryBySql<T>(this DatabaseFacade database, string sql, params object[] parameters)
            where T : class, new()
        {
            var dbConnection = database.GetDbConnection();
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                command.Parameters.AddRange(parameters);

                var reader = command.ExecuteReader();

                var fieldCount = reader.FieldCount;
                var columns = new List<string>();
                for (int i = 0; i < fieldCount; i++)
                {
                    columns.Add(reader.GetName(i));
                }

                var result = new List<T>();

                while (reader.Read())
                {
                    var entity = new T();
                    var type = entity.GetType();

                    foreach (var propertyInfo in type.GetProperties())
                    {
                        if (!columns.Any(w => w == propertyInfo.Name))
                        {
                            continue;
                        }

                        propertyInfo.SetValue(entity, reader.GetValue(propertyInfo.Name));
                    }

                    result.Add(entity);
                }

                dbConnection.Close();

                return result;
            }
        }

        /// <summary>
        /// 查询根据sql语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static async Task<List<T>> QueryBySqlAsync<T>(this DatabaseFacade database, string sql, params object[] parameters)
            where T : class, new()
        {
            var dbConnection = database.GetDbConnection();
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                if (command.Connection.State != ConnectionState.Open)
                {
                    await command.Connection.OpenAsync();
                }

                command.Parameters.AddRange(parameters);

                var reader = await command.ExecuteReaderAsync();

                var fieldCount = reader.FieldCount;
                var columns = new List<string>();
                for (int i = 0; i < fieldCount; i++)
                {
                    columns.Add(reader.GetName(i));
                }

                var result = new List<T>();

                while (reader.Read())
                {
                    var entity = new T();
                    var type = entity.GetType();

                    foreach (var propertyInfo in type.GetProperties())
                    {
                        if (!columns.Any(w => w == propertyInfo.Name))
                        {
                            continue;
                        }

                        propertyInfo.SetValue(entity, reader.GetValue(propertyInfo.Name));
                    }

                    result.Add(entity);
                }

                await dbConnection.CloseAsync();

                return result;
            }
        }

        /// <summary>
        /// 查询根据sql返回单个值
        /// </summary>
        /// <param name="database"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object? QueryScalarBySql(this DatabaseFacade database, string sql, params object[] parameters)
        {
            var dbConnection = database.GetDbConnection();
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                command.Parameters.AddRange(parameters);

                var result = command.ExecuteScalar();

                dbConnection.Close();

                return result;
            }
        }

        /// <summary>
        /// 查询根据sql返回单个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T QueryScalarBySql<T>(this DatabaseFacade database, string sql, params object[] parameters)
            where T : struct
        {
            var dbConnection = database.GetDbConnection();
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                command.Parameters.AddRange(parameters);

                var result = command.ExecuteScalar();

                dbConnection.Close();

                if (result == null)
                {
                    return default;
                }

                if (result is T)
                {
                    return (T)result;
                }

                return default;
            }
        }

        /// <summary>
        /// 查询根据sql返回单个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static async Task<T> QueryScalarBySqlAsync<T>(this DatabaseFacade database, string sql, params object[] parameters)
            where T : struct
        {
            var dbConnection = database.GetDbConnection();
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                if (command.Connection.State != ConnectionState.Open)
                {
                    await command.Connection.OpenAsync();
                }

                command.Parameters.AddRange(parameters);

                var result = await command.ExecuteScalarAsync();

                await dbConnection.CloseAsync();

                if (result == null)
                {
                    return default;
                }

                if (result is T)
                {
                    return (T)result;
                }

                return default;

            }
        }


    }
}
