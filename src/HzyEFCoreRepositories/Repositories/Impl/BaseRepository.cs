﻿/*
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
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using HzyEFCoreRepositories.ExpressionTree;
using HzyEFCoreRepositories.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Npgsql;

namespace HzyEFCoreRepositories.Repositories.Impl
{
#nullable enable
    /// <summary>
    /// 基础仓储 实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    public abstract class BaseRepository<T, TDbContext> : IRepository<T, TDbContext>
        where T : class, new()
        where TDbContext : DbContext
    {
        private readonly TDbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly PropertyInfo _keyPropertyInfo;
        private Expression<Func<T, bool>> _filter;
        private bool isIgnoreQueryFilter;

        /// <summary>
        /// 基础仓储
        /// </summary>
        /// <param name="context">dbcontext 上下文</param>
        /// <param name="filter">过滤条件</param>
        public BaseRepository(TDbContext context, Expression<Func<T, bool>> filter = null)
        {
            this._context = context;
            _dbSet = context.Set<T>();
            _keyPropertyInfo = typeof(T).GetKeyProperty(false);
            _filter = filter;
            isIgnoreQueryFilter = false;
        }

        /// <summary>
        /// DbContext 对象
        /// </summary>
        public virtual TDbContext Orm => this._context;

        /// <summary>
        /// 设置 跟踪 Attachq
        /// </summary>
        /// <param name="model"></param>
        /// <param name="entityState"></param>
        public virtual void SetAttach(T model, EntityState entityState)
        {
            //Context.Entry(model).State = entityState;
            //如果 newModel 未被跟踪 则手动 Attach
            var attach = _context.Attach(model);
            if (attach != null)
            {
                attach.State = entityState;
            }
        }

        /// <summary>
        /// 生成表达式树 例如：( w=>w.Key==Guid.Empty )
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual Expression<Func<T, bool>> GetKeyExpression(object value)
            => ExpressionTreeExtensions.Equal<T, object>(this._keyPropertyInfo.Name, value);

        #region 插入

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual T Insert(T model)
        {
            this._dbSet.Add(model);
            this._context.SaveChanges();
            return model;
        }

        /// <summary>
        /// 插入 批量
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual int InsertRange(IEnumerable<T> model)
        {
            this._dbSet.AddRange(model);
            return this._context.SaveChanges();
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<T> InsertAsync(T model)
        {
            await this._dbSet.AddAsync(model);
            await this._context.SaveChangesAsync();
            return model;
        }

        /// <summary>
        /// 插入 批量
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual Task<int> InsertRangeAsync(IEnumerable<T> model)
        {
            this._dbSet.AddRangeAsync(model);
            return this._context.SaveChangesAsync();
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
            this._dbSet.Update(model);
            return this._context.SaveChanges();
        }

        /// <summary>
        /// 根据实体id更新数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual int UpdateById(T model)
        {
            if (_keyPropertyInfo == null) throw new Exception("模型未设置主键特性标记!");
            var value = this._keyPropertyInfo.GetValue(model);
            var oldModel = this.FindById(value);
            if (oldModel == null) return -1;
            return this.Update(oldModel, model);
        }

        /// <summary>
        /// 更新，通过原模型 修改为新模型
        /// </summary>
        /// <param name="oldModel"></param>
        /// <param name="newModel"></param>
        /// <returns></returns>
        public virtual int Update(T oldModel, T newModel)
        {
            this._context.Entry(oldModel).CurrentValues.SetValues(newModel);
            return this._context.SaveChanges();
        }

        /// <summary>
        /// 更新 批量
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public virtual int UpdateRange(IEnumerable<T> models)
        {
            this._dbSet.UpdateRange(models);
            return this._context.SaveChanges();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual Task<int> UpdateAsync(T model)
        {
            this._dbSet.Update(model);
            return this._context.SaveChangesAsync();
        }

        /// <summary>
        /// 根据实体id更新数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateByIdAsync(T model)
        {
            if (_keyPropertyInfo == null) throw new Exception("模型未设置主键特性标记!");
            var value = this._keyPropertyInfo.GetValue(model);
            var oldModel = await this.Query().FirstOrDefaultAsync(this.GetKeyExpression(value));
            if (oldModel == null) return -1;
            return await this.UpdateAsync(oldModel, model);
        }

        /// <summary>
        /// 更新，通过原模型 修改为新模型
        /// </summary>
        /// <param name="oldModel"></param>
        /// <param name="newModel"></param>
        /// <returns></returns>
        public virtual Task<int> UpdateAsync(T oldModel, T newModel)
        {
            this._context.Entry(oldModel).CurrentValues.SetValues(newModel);
            return this._context.SaveChangesAsync();
        }

        /// <summary>
        /// 更新 批量
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public virtual Task<int> UpdateRangeAsync(IEnumerable<T> models)
        {
            this._dbSet.UpdateRange(models);
            return this._context.SaveChangesAsync();
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
            var value = this._keyPropertyInfo.GetValue(model);
            var oldModel = this.FindById(value);
            if (oldModel == null)
                this.Insert(model);
            else
                this.Update(oldModel, model);
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
            var value = this._keyPropertyInfo.GetValue(model);
            var oldModel = await this.FindByIdAsync(value);
            if (oldModel == null)
                await this.InsertAsync(model);
            else
                await this.UpdateAsync(oldModel, model);
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
            this._dbSet.Remove(model);
            return this._context.SaveChanges();
        }

        /// <summary>
        /// 删除 批量
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public virtual int Delete(IEnumerable<T> models)
        {
            this._dbSet.RemoveRange(models);
            return this._context.SaveChanges();
        }

        /// <summary>
        /// 删除 根据表达式
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual int Delete(Expression<Func<T, bool>> expWhere)
            => this.Delete(this.Query().Where(expWhere));

        /// <summary>
        /// 删除 根据id
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public virtual int DeleteById<TKey>(TKey key)
        {
            return this.Delete(this.FindById(key));
        }

        /// <summary>
        /// 删除 根据 id集合
        /// </summary>
        /// <param name="keys"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public virtual int DeleteByIds<TKey>(IEnumerable<TKey> keys)
        {
            return this.Delete(this.FindByIds(keys));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteAsync(T model)
        {
            this._dbSet.Remove(model);
            return this._context.SaveChangesAsync();
        }

        /// <summary>
        /// 删除 批量
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteAsync(IEnumerable<T> models)
        {
            this._dbSet.RemoveRange(models);
            return this._context.SaveChangesAsync();
        }

        /// <summary>
        /// 删除 根据表达式
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteAsync(Expression<Func<T, bool>> expWhere)
            => this.DeleteAsync(this.Query().Where(expWhere));

        /// <summary>
        /// 删除 根据 id
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public virtual async Task<int> DeleteByIdAsync<TKey>(TKey key)
        {
            return await this.DeleteAsync(await this.FindByIdAsync(key));
        }

        /// <summary>
        /// 删除 根据id集合
        /// </summary>
        /// <param name="keys"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public async Task<int> DeleteByIdsAsync<TKey>(IEnumerable<TKey> keys)
        {
            if (_keyPropertyInfo == null) throw new Exception("模型未设置主键特性标记!");
            var exp = ExpressionTreeExtensions.Contains<T, TKey>(_keyPropertyInfo.Name, keys);
            return await this.DeleteAsync(await this.Query().Where(exp).ToListAsync());
        }

        #endregion

        #region 查询 复杂型

        /// <summary>
        /// 添加检索过滤
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual IRepository<T, TDbContext> AddQueryFilter(Expression<Func<T, bool>> filter = null)
        {
            this._filter = filter;
            return this;
        }

        /// <summary>
        /// 忽略查询过滤条件
        /// </summary>
        /// <returns></returns>
        public virtual IRepository<T, TDbContext> IgnoreQueryFilter()
        {
            isIgnoreQueryFilter = true;
            return this;
        }

        /// <summary>
        /// 恢复忽略查询过滤条件
        /// </summary>
        /// <returns></returns>
        public virtual IRepository<T, TDbContext> RecoveryQueryFilter()
        {
            isIgnoreQueryFilter = false;
            return this;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="isTracking">是否追踪</param>
        /// <returns></returns>
        public virtual IQueryable<T> Query(bool isTracking = true)
            => isTracking ?
            this._dbSet.WhereIf(!isIgnoreQueryFilter && _filter != null, _filter).AsQueryable() :
            this._dbSet.WhereIf(!isIgnoreQueryFilter && _filter != null, _filter).AsNoTracking();

        /// <summary>
        /// 查询 有跟踪
        /// </summary>
        public virtual IQueryable<T> Select => this.Query();

        /// <summary>
        /// 查询 无跟踪
        /// </summary>
        public virtual IQueryable<T> SelectNoTracking => this.Query(false);

        #endregion

        #region 查询 单条

        /// <summary>
        /// 查询 根据条件
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual T Find(Expression<Func<T, bool>> expWhere)
            => this.Query().Where(expWhere).FirstOrDefault();

        /// <summary>
        /// 查询 根据id
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public virtual T FindById<TKey>(TKey key)
            => this.Query().FirstOrDefault(this.GetKeyExpression(key));

        /// <summary>
        /// 查询 根据id集合
        /// </summary>
        /// <param name="keys"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public virtual T FindByIds<TKey>(IEnumerable<TKey> keys)
        {
            if (_keyPropertyInfo == null) throw new Exception("模型未设置主键特性标记!");
            var exp = ExpressionTreeExtensions.Contains<T, TKey>(_keyPropertyInfo.Name, keys);
            return this.Find(exp);
        }

        /// <summary>
        /// 查询 根据条件
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual Task<T> FindAsync(Expression<Func<T, bool>> expWhere)
            => this.Query().Where(expWhere).FirstOrDefaultAsync();

        /// <summary>
        /// 查询 根据id
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public virtual async Task<T> FindByIdAsync<TKey>(TKey key)
            => await this.Query().FirstOrDefaultAsync(this.GetKeyExpression(key));

        /// <summary>
        /// /查询 根据id集合
        /// </summary>
        /// <param name="keys"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public virtual Task<T> FindByIdsAsync<TKey>(IEnumerable<TKey> keys)
        {
            if (_keyPropertyInfo == null) throw new Exception("模型未设置主键特性标记!");
            var exp = ExpressionTreeExtensions.Contains<T, TKey>(_keyPropertyInfo.Name, keys);
            return this.FindAsync(exp);
        }

        #endregion

        #region 查询 多条

        /// <summary>
        /// 获取列表 根据查询条件
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual List<T> ToList(Expression<Func<T, bool>> expWhere)
            => this.Query().Where(expWhere).ToList();

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public virtual List<T> ToListAll()
            => this.Query().ToList();

        /// <summary>
        /// 获取列表 根据查询条件
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual Task<List<T>> ToListAsync(Expression<Func<T, bool>> expWhere)
            => this.Query().Where(expWhere).ToListAsync();

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public virtual Task<List<T>> ToListAllAsync()
            => this.Query().ToListAsync();

        #endregion

        #region 是否存在 、 数量

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <returns></returns>
        public virtual int Count()
            => this.Query().Count();

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <returns></returns>
        public virtual long CountLong()
            => this.Query().LongCount();

        /// <summary>
        /// 获取数量 根据条件
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual int Count(Expression<Func<T, bool>> expWhere)
            => this.Query().Count(expWhere);

        /// <summary>
        /// 获取数量 根据条件
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual long CountLong(Expression<Func<T, bool>> expWhere)
            => this.Query().LongCount(expWhere);

        /// <summary>
        /// 是否存在 根据条件
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual bool Any(Expression<Func<T, bool>> expWhere)
            => this.Query().Any(expWhere);

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <returns></returns>
        public virtual Task<int> CountAsync()
            => this.Query().CountAsync();

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <returns></returns>
        public virtual Task<long> CountLongAsync()
            => this.Query().LongCountAsync();

        /// <summary>
        /// 获取数量 根据条件
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual Task<int> CountAsync(Expression<Func<T, bool>> expWhere)
            => this.Query().CountAsync(expWhere);

        /// <summary>
        /// 获取数量 根据条件
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual Task<long> CountLongAsync(Expression<Func<T, bool>> expWhere)
            => this.Query().LongCountAsync(expWhere);

        /// <summary>
        /// 是否存在 根据条件
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual Task<bool> AnyAsync(Expression<Func<T, bool>> expWhere)
            => this.Query().AnyAsync(expWhere);

        #endregion

        #region 原生 sql 操作

        /// <summary>
        /// 执行sql 返回受影响的行数 insert|update|delete
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteSqlRaw(string sql, params object[] parameters)
        {
            return Orm.Database.ExecuteSqlRaw(sql, parameters);
        }

        /// <summary>
        /// 执行sql 返回受影响的行数 insert|update|delete
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteSqlRaw(string sql, IEnumerable<object> parameters)
        {
            return Orm.Database.ExecuteSqlRaw(sql, parameters);
        }

        /// <summary>
        /// 执行sql 返回受影响的行数 insert|update|delete
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> ExecuteSqlRawAsync(string sql, CancellationToken cancellationToken = default)
        {
            return Orm.Database.ExecuteSqlRawAsync(sql, cancellationToken);
        }
        /// <summary>
        /// 执行sql 返回受影响的行数 insert|update|delete
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters)
        {
            return Orm.Database.ExecuteSqlRawAsync(sql, parameters);
        }
        /// <summary>
        /// 执行sql 返回受影响的行数 insert|update|delete
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> ExecuteSqlRawAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default)
        {
            return Orm.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
        }

        /// <summary>
        /// 查询根据sql语句
        /// </summary>
        /// <returns> IQueryable </returns>
        public IQueryable<T> QueryBySql(string sql, params object[] parameters)
        {
            return _dbSet.FromSqlRaw(sql, parameters);
        }

        /// <summary>
        /// 根据 sql 查询表格
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable QueryDataTableBySql(string sql, params object[] parameters)
        {
            return Orm.Database.QueryDataTableBySql(sql, parameters);
        }

        /// <summary>
        /// 根据 sql 查询表格
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Task<DataTable> QueryDataTableBySqlAsync(string sql, params object[] parameters)
        {
            return Orm.Database.QueryDataTableBySqlAsync(sql, parameters);
        }

        /// <summary>
        /// 根据 sql 查询字典集合
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> QueryDicBySql(string sql, params object[] parameters)
        {
            return Orm.Database.QueryDicBySql(sql, parameters);
        }

        /// <summary>
        /// 根据 sql 查询字典集合
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Task<List<Dictionary<string, object>>> QueryDicBySqlAsync(string sql, params object[] parameters)
        {
            return Orm.Database.QueryDicBySqlAsync(sql, parameters);
        }

        /// <summary>
        /// 查询根据sql语句
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<TEntity> QueryBySql<TEntity>(string sql, params object[] parameters) where TEntity : class, new()
        {
            return Orm.Database.QueryBySql<TEntity>(sql, parameters);
        }

        /// <summary>
        /// 查询根据sql语句
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Task<List<TEntity>> QueryBySqlAsync<TEntity>(string sql, params object[] parameters) where TEntity : class, new()
        {
            return Orm.Database.QueryBySqlAsync<TEntity>(sql, parameters);
        }

        /// <summary>
        /// 查询根据sql返回单个值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object? QuerySingleBySql(string sql, params object[] parameters)
        {
            return Orm.Database.QuerySingleBySql(sql, parameters);
        }

        /// <summary>
        /// 查询根据sql返回单个值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Task<object> QuerySingleBySqlAsync(string sql, params object[] parameters)
        {
            return Orm.Database.QuerySingleBySqlAsync(sql, parameters);
        }

        /// <summary>
        /// 查询根据sql返回单个值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public TResult QuerySingleBySql<TResult>(string sql, params object[] parameters)
            where TResult : struct
        {
            return Orm.Database.QuerySingleBySql<TResult>(sql, parameters);
        }

        /// <summary>
        /// 查询根据sql返回单个值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Task<TResult> QuerySingleBySqlAsync<TResult>(string sql, params object[] parameters)
          where TResult : struct
        {
            return Orm.Database.QuerySingleBySqlAsync<TResult>(sql, parameters);
        }

        #endregion













    }

}
