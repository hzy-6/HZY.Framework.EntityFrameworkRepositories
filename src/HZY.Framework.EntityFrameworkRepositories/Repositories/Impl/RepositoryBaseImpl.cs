/*
 * *******************************************************
 *
 * 作者：hzy
 *
 * 开源地址：https://gitee.com/hzy6
 *
 * *******************************************************
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using HZY.Framework.EntityFrameworkRepositories.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace HZY.Framework.EntityFrameworkRepositories.Repositories.Impl
{

    /// <summary>
    /// 基础仓储 Crud 实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    public abstract class RepositoryBaseImpl<T, TDbContext> : QueryRepositoryImpl<T, TDbContext>, IRepositoryBase<T, TDbContext>
        where T : class, new()
        where TDbContext : class
    {

        /// <summary>
        /// 基础仓储 Crud 实现
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="filter"></param>
        public RepositoryBaseImpl(TDbContext dbContext, Expression<Func<T, bool>> filter = null)
            : base(dbContext, filter, true)
        {

        }

        #region 插入

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual T Insert(T model)
        {
            UnitOfWork.DbSet<T>().Add(model);
            UnitOfWork.SaveChanges();
            return model;
        }

        /// <summary>
        /// 插入 批量
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual int InsertRange(IEnumerable<T> model)
        {
            UnitOfWork.DbSet<T>().AddRange(model);
            return UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<T> InsertAsync(T model)
        {
            await UnitOfWork.DbSet<T>().AddAsync(model);
            await UnitOfWork.SaveChangesAsync(); ;
            return model;
        }

        /// <summary>
        /// 插入 批量
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual Task<int> InsertRangeAsync(IEnumerable<T> model)
        {
            UnitOfWork.DbSet<T>().AddRangeAsync(model);
            return UnitOfWork.SaveChangesAsync();
        }

        #endregion

        #region 更新

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual int Update(T model)
        {
            UnitOfWork.DbSet<T>().Update(model);
            return UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// 根据实体id更新数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual int UpdateById(T model)
        {
            if (_keyPropertyInfo == null) throw new Exception("模型未设置主键特性标记!");
            var value = _keyPropertyInfo.GetValue(model);
            var oldModel = FindById(value);
            if (oldModel == null) return -1;
            return Update(oldModel, model);
        }

        /// <summary>
        /// 更新，通过原模型 修改为新模型
        /// </summary>
        /// <param name="oldModel"></param>
        /// <param name="newModel"></param>
        /// <returns></returns>
        public virtual int Update(T oldModel, T newModel)
        {
            Context.Entry(oldModel).CurrentValues.SetValues(newModel);
            return UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// 更新 批量
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public virtual int UpdateRange(IEnumerable<T> models)
        {
            UnitOfWork.DbSet<T>().UpdateRange(models);
            return UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// 更新部分字段 根据where 条件
        /// </summary>
        /// <param name="setPropertyCalls"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual int UpdateBulk(Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls, Expression<Func<T, bool>> where)
        {
            return Select.Where(where).ExecuteUpdate(setPropertyCalls);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual Task<int> UpdateAsync(T model)
        {
            UnitOfWork.DbSet<T>().Update(model);
            return UnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 根据实体id更新数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateByIdAsync(T model)
        {
            if (_keyPropertyInfo == null) throw new Exception("模型未设置主键特性标记!");
            var value = _keyPropertyInfo.GetValue(model);
            var oldModel = await Query().FirstOrDefaultAsync(GetKeyExpression(value));
            if (oldModel == null) return -1;
            return await UpdateAsync(oldModel, model);
        }

        /// <summary>
        /// 更新，通过原模型 修改为新模型
        /// </summary>
        /// <param name="oldModel"></param>
        /// <param name="newModel"></param>
        /// <returns></returns>
        public virtual Task<int> UpdateAsync(T oldModel, T newModel)
        {
            Context.Entry(oldModel).CurrentValues.SetValues(newModel);
            return UnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 更新 批量
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public virtual Task<int> UpdateRangeAsync(IEnumerable<T> models)
        {
            UnitOfWork.DbSet<T>().UpdateRange(models);
            return UnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 更新部分字段 根据where 条件
        /// </summary>
        /// <param name="setPropertyCalls"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual Task<int> UpdateBulkAsync(Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls, Expression<Func<T, bool>> where)
        {
            return Select.Where(where).ExecuteUpdateAsync(setPropertyCalls);
        }


        #endregion

        #region 插入或者更新

        /// <summary>
        /// 插入或者添加 根据实体 主键判断是否添加还是修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual T InsertOrUpdate(T model)
        {
            if (_keyPropertyInfo == null) throw new Exception("模型未设置主键特性标记!");
            var value = _keyPropertyInfo.GetValue(model);
            var oldModel = FindById(value);
            if (oldModel == null)
                Insert(model);
            else
                Update(oldModel, model);
            return model;
        }

        /// <summary>
        /// 插入或者添加 根据实体 主键判断是否添加还是修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<T> InsertOrUpdateAsync(T model)
        {
            if (_keyPropertyInfo == null) throw new Exception("模型未设置主键特性标记!");
            var value = _keyPropertyInfo.GetValue(model);
            var oldModel = await FindByIdAsync(value);
            if (oldModel == null)
                await InsertAsync(model);
            else
                await UpdateAsync(oldModel, model);
            return model;
        }

        #endregion

        #region 删除

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual int Delete(T model)
        {
            UnitOfWork.DbSet<T>().Remove(model);
            return UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// 删除 批量
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public virtual int Delete(IEnumerable<T> models)
        {
            UnitOfWork.DbSet<T>().RemoveRange(models);
            return UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// 删除 根据表达式
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual int Delete(Expression<Func<T, bool>> expWhere)
            => Delete(Query().Where(expWhere));

        /// <summary>
        /// 删除 根据表达式 直接 生成 delete from table  where 语句操作
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual int DeleteBulk(Expression<Func<T, bool>> expWhere)
        {
            return Select.Where(expWhere).ExecuteDelete();
        }

        /// <summary>
        /// 删除 根据id
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public virtual int DeleteById<TKey>(TKey key)
        {
            return Delete(FindById(key));
        }

        /// <summary>
        /// 删除 根据 id集合
        /// </summary>
        /// <param name="keys"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public virtual int DeleteByIds<TKey>(IEnumerable<TKey> keys)
        {
            if (_keyPropertyInfo == null) throw new Exception("模型未设置主键特性标记!");
            var exp = ExpressionTreeExtensions.Contains<T, TKey>(_keyPropertyInfo.Name, keys);
            return Delete(exp);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteAsync(T model)
        {
            UnitOfWork.DbSet<T>().Remove(model);
            return UnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 删除 批量
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteAsync(IEnumerable<T> models)
        {
            UnitOfWork.DbSet<T>().RemoveRange(models);
            return UnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 删除 根据表达式
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteAsync(Expression<Func<T, bool>> expWhere)
            => DeleteAsync(Query().Where(expWhere));

        /// <summary>
        /// 删除 根据表达式 直接 生成 delete from table  where 语句操作
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteBulkAsync(Expression<Func<T, bool>> expWhere)
        {
            return Select.Where(expWhere).ExecuteDeleteAsync();
        }

        /// <summary>
        /// 删除 根据 id
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public virtual async Task<int> DeleteByIdAsync<TKey>(TKey key)
        {
            return await DeleteAsync(await FindByIdAsync(key));
        }

        /// <summary>
        /// 删除 根据id集合
        /// </summary>
        /// <param name="keys"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public virtual async Task<int> DeleteByIdsAsync<TKey>(IEnumerable<TKey> keys)
        {
            if (_keyPropertyInfo == null) throw new Exception("模型未设置主键特性标记!");
            var exp = ExpressionTreeExtensions.Contains<T, TKey>(_keyPropertyInfo.Name, keys);
            return await DeleteAsync(exp);
        }

        #endregion

        #region  原生 sql 操作 添加、修改、删除

        /// <summary>
        /// 执行sql 返回受影响的行数 insert|update|delete
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual int ExecuteSqlRaw(string sql, params object[] parameters)
        {
            return Context.Database.ExecuteSqlRaw(sql, parameters);
        }

        /// <summary>
        /// 执行sql 返回受影响的行数 insert|update|delete
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual int ExecuteSqlRaw(string sql, IEnumerable<object> parameters)
        {
            return Context.Database.ExecuteSqlRaw(sql, parameters);
        }

        /// <summary>
        /// 执行sql 返回受影响的行数 insert|update|delete
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<int> ExecuteSqlRawAsync(string sql, CancellationToken cancellationToken = default)
        {
            return Context.Database.ExecuteSqlRawAsync(sql, cancellationToken);
        }
        /// <summary>
        /// 执行sql 返回受影响的行数 insert|update|delete
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters)
        {
            return Context.Database.ExecuteSqlRawAsync(sql, parameters);
        }
        /// <summary>
        /// 执行sql 返回受影响的行数 insert|update|delete
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<int> ExecuteSqlRawAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default)
        {
            return Context.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
        }


        #endregion

        #region Sqlserver 数据批量拷贝

        /// <summary>
        /// Sqlserver 数据拷贝
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="tableName"></param>
        /// <param name="dbTransaction"></param>
        public virtual void SqlServerBulkCopy(DataTable dataTable, string tableName, IDbTransaction dbTransaction = null)
        {
            Context.Database.SqlServerBulkCopy(dataTable, tableName, dbTransaction);
        }

        /// <summary>
        /// Sqlserver 数据拷贝
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="tableName"></param>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        public virtual Task SqlServerBulkCopyAsync(DataTable dataTable, string tableName, IDbTransaction dbTransaction = null)
        {
            return Context.Database.SqlServerBulkCopyAsync(dataTable, tableName, dbTransaction);
        }

        /// <summary>
        /// Sqlserver 数据拷贝
        /// </summary>
        /// <param name="items"></param>
        /// <param name="tableName"></param>
        /// <param name="dbTransaction"></param>
        public virtual void SqlServerBulkCopy(List<T> items, string tableName = null, IDbTransaction dbTransaction = null)
        {
            Context.Database.SqlServerBulkCopy(items, tableName, dbTransaction);
        }

        /// <summary>
        /// Sqlserver 数据拷贝
        /// </summary>
        /// <param name="items"></param>
        /// <param name="tableName"></param>
        /// <param name="dbTransaction"></param>
        public virtual Task SqlServerBulkCopyAsync(List<T> items, string tableName = null, IDbTransaction dbTransaction = null)
        {
            return Context.Database.SqlServerBulkCopyAsync(items, tableName, dbTransaction);
        }

        #endregion

        #region MySql 数据批量拷贝


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
        /// <para>
        /// 3、连接字符串中添加此属性：AllowLoadLocalInfile=true
        /// </para>
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="tableName"></param>
        /// <param name="dbTransaction"></param>
        public virtual void MySqlBulkCopy(DataTable dataTable, string tableName, IDbTransaction dbTransaction = null)
        {
            Context.Database.MySqlBulkCopy(dataTable, tableName, dbTransaction);
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
        /// <para>
        /// 3、连接字符串中添加此属性：AllowLoadLocalInfile=true
        /// </para>
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="tableName"></param>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        public virtual Task MySqlBulkCopyAsync(DataTable dataTable, string tableName, IDbTransaction dbTransaction = null)
        {
            return Context.Database.MySqlBulkCopyAsync(dataTable, tableName, dbTransaction);
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
        /// <para>
        /// 3、连接字符串中添加此属性：AllowLoadLocalInfile=true
        /// </para>
        /// </summary>
        /// <param name="items"></param>
        /// <param name="tableName"></param>
        /// <param name="dbTransaction"></param>
        public virtual void MySqlBulkCopy(List<T> items, string tableName = null, IDbTransaction dbTransaction = null)
        {
            Context.Database.MySqlBulkCopy(items, tableName, dbTransaction);
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
        /// <para>
        /// 3、连接字符串中添加此属性：AllowLoadLocalInfile=true
        /// </para>
        /// </summary>
        /// <param name="items"></param>
        /// <param name="tableName"></param>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        public virtual Task MySqlBulkCopyAsync(List<T> items, string tableName = null, IDbTransaction dbTransaction = null)
        {
            return Context.Database.MySqlBulkCopyAsync(items, tableName, dbTransaction);
        }

        #endregion

        #region Npgsql 数据批量拷贝

        /// <summary>
        /// Npgsql 数据拷贝
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="tableName"></param>
        /// <param name="ignoreColumns"></param>
        public virtual void NpgsqlBulkCopy(DataTable dataTable, string tableName, params string[] ignoreColumns)
        {
            Context.Database.NpgsqlBulkCopy(dataTable, tableName, ignoreColumns);
        }

        /// <summary>
        /// Npgsql 数据拷贝
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="tableName"></param>
        /// <param name="ignoreColumns"></param>
        /// <returns></returns>
        public virtual Task NpgsqlBulkCopyAsync(DataTable dataTable, string tableName, params string[] ignoreColumns)
        {
            return Context.Database.NpgsqlBulkCopyAsync(dataTable, tableName, default, ignoreColumns);
        }

        /// <summary>
        /// Npgsql 数据拷贝
        /// </summary>
        /// <param name="items"></param>
        /// <param name="tableName"></param>
        /// <param name="ignoreColumns"></param>
        public virtual void NpgsqlBulkCopy(List<T> items, string tableName = null, params string[] ignoreColumns)
        {
            Context.Database.NpgsqlBulkCopy(items, tableName, ignoreColumns);
        }

        /// <summary>
        /// Npgsql 数据拷贝
        /// </summary>
        /// <param name="items"></param>
        /// <param name="tableName"></param>
        /// <param name="ignoreColumns"></param>
        public virtual Task NpgsqlBulkCopyAsync(List<T> items, string tableName = null, params string[] ignoreColumns)
        {
            return Context.Database.NpgsqlBulkCopyAsync(items, tableName, ignoreColumns);
        }

        #endregion

    }

}
