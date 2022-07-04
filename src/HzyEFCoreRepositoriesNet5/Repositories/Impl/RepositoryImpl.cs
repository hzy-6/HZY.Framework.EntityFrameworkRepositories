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
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using HzyEFCoreRepositories.DbContexts;
using HzyEFCoreRepositories.Extensions;
using HzyEFCoreRepositories.Extensions.Parser;
using Microsoft.EntityFrameworkCore;

namespace HzyEFCoreRepositories.Repositories.Impl
{

    /// <summary>
    /// 基础仓储 Crud 实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    public abstract class RepositoryImpl<T, TDbContext> : QueryRepositoryImpl<T, TDbContext>, IRepository<T, TDbContext>
        where T : class, new()
        where TDbContext : class
    {

        /// <summary>
        /// 基础仓储 Crud 实现
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="filter"></param>
        public RepositoryImpl(TDbContext dbContext, Expression<Func<T, bool>> filter = null)
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
            this.DbSet.Add(model);
            this.DbContextBase.SaveChanges();
            return model;
        }

        /// <summary>
        /// 插入 批量
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual int InsertRange(IEnumerable<T> model)
        {
            this.DbSet.AddRange(model);
            return this.DbContextBase.SaveChanges();
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<T> InsertAsync(T model)
        {
            await this.DbSet.AddAsync(model);
            await this.DbContextBase.SaveChangesAsync();
            return model;
        }

        /// <summary>
        /// 插入 批量
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual Task<int> InsertRangeAsync(IEnumerable<T> model)
        {
            this.DbSet.AddRangeAsync(model);
            return this.DbContextBase.SaveChangesAsync();
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
            //如果未跟踪
            if (DbContextBase.Entry(model).State == EntityState.Detached)
            {
                //变更实体未跟踪修改状态
                this.SetEntityState(model, EntityState.Modified);
            }

            this.DbSet.Update(model);
            return DbContextBase.SaveChanges();
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
            this.DbContextBase.Entry(oldModel).CurrentValues.SetValues(newModel);
            return this.DbContextBase.SaveChanges();
        }

        /// <summary>
        /// 更新 批量
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public virtual int UpdateRange(IEnumerable<T> models)
        {
            this.DbSet.UpdateRange(models);
            return this.DbContextBase.SaveChanges();
        }

        /// <summary>
        /// 更新部分字段 根据where 条件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="where"></param>
        /// <param name="ignoreCols"></param>
        /// <returns></returns>
        public virtual int UpdateBulk(Expression<Func<T, T>> model, Expression<Func<T, bool>> where, Action<UpdateIgnoreParser<T>> ignoreCols = null)
        {
            var updateIgnoreParser = new UpdateIgnoreParser<T>();
            ignoreCols?.Invoke(updateIgnoreParser);

            var updateParser = new UpdateParser(DbContextBase, this.Select.Where(where).ToQueryString(), model.Body as MemberInitExpression, updateIgnoreParser.GetIgnoreColumns());
            return this.ExecuteSqlRaw(updateParser.Parser(), updateParser.GetDataParameters());
        }

        /// <summary>
        /// 更新部分字段 根据where 条件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="where"></param>
        /// <param name="ignoreCols"></param>
        /// <returns></returns>
        public virtual int UpdateBulk(T model, Expression<Func<T, bool>> where, Action<UpdateIgnoreParser<T>> ignoreCols = null)
        {
            var updateIgnoreParser = new UpdateIgnoreParser<T>();
            ignoreCols?.Invoke(updateIgnoreParser);

            var updateParser = new UpdateParser(DbContextBase, this.Select.Where(where).ToQueryString(), ExpressionTreeExtensions.ModelToMemberInitExpression(model), updateIgnoreParser.GetIgnoreColumns());
            return this.ExecuteSqlRaw(updateParser.Parser(), updateParser.GetDataParameters());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual Task<int> UpdateAsync(T model)
        {
            //如果未跟踪
            if (DbContextBase.Entry(model).State == EntityState.Detached)
            {
                //变更实体未跟踪修改状态
                this.SetEntityState(model, EntityState.Modified);
            }

            this.DbSet.Update(model);
            return this.DbContextBase.SaveChangesAsync();
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
            this.DbContextBase.Entry(oldModel).CurrentValues.SetValues(newModel);
            return this.DbContextBase.SaveChangesAsync();
        }

        /// <summary>
        /// 更新 批量
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public virtual Task<int> UpdateRangeAsync(IEnumerable<T> models)
        {
            this.DbSet.UpdateRange(models);
            return this.DbContextBase.SaveChangesAsync();
        }

        /// <summary>
        /// 更新部分字段 根据where 条件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="where"></param>
        /// <param name="ignoreCols"></param>
        /// <returns></returns>
        public virtual Task<int> UpdateBulkAsync(Expression<Func<T, T>> model, Expression<Func<T, bool>> where, Action<UpdateIgnoreParser<T>> ignoreCols = null)
        {
            var updateIgnoreParser = new UpdateIgnoreParser<T>();
            ignoreCols?.Invoke(updateIgnoreParser);

            var updateParser = new UpdateParser(DbContextBase, this.Select.Where(where).ToQueryString(), model.Body as MemberInitExpression, updateIgnoreParser.GetIgnoreColumns());
            return this.ExecuteSqlRawAsync(updateParser.Parser(), updateParser.GetDataParameters());
        }

        /// <summary>
        /// 更新部分字段 根据where 条件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="where"></param>
        /// <param name="ignoreCols"></param>
        /// <returns></returns>
        public virtual Task<int> UpdateBulkAsync(T model, Expression<Func<T, bool>> where, Action<UpdateIgnoreParser<T>> ignoreCols = null)
        {
            var updateIgnoreParser = new UpdateIgnoreParser<T>();
            ignoreCols?.Invoke(updateIgnoreParser);

            var updateParser = new UpdateParser(DbContextBase, this.Select.Where(where).ToQueryString(), ExpressionTreeExtensions.ModelToMemberInitExpression(model), updateIgnoreParser.GetIgnoreColumns());
            return this.ExecuteSqlRawAsync(updateParser.Parser(), updateParser.GetDataParameters());
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
            //如果未跟踪
            if (DbContextBase.Entry(model).State == EntityState.Detached)
            {
                //变更实体未跟踪修改状态
                this.SetEntityState(model, EntityState.Deleted);
            }

            this.DbSet.Remove(model);
            return this.DbContextBase.SaveChanges();
        }

        /// <summary>
        /// 删除 批量
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public virtual int Delete(IEnumerable<T> models)
        {
            this.DbSet.RemoveRange(models);
            return this.DbContextBase.SaveChanges();
        }

        /// <summary>
        /// 删除 根据表达式
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual int Delete(Expression<Func<T, bool>> expWhere)
            => this.Delete(this.Query().Where(expWhere));

        /// <summary>
        /// 删除 根据表达式 直接 生成 delete from table  where 语句操作
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual int DeleteBulk(Expression<Func<T, bool>> expWhere)
        {
            var deleteParser = new DeleteParser(DbContextBase, this.Select.Where(expWhere).ToQueryString());
            return this.ExecuteSqlRaw(deleteParser.Parser());
        }

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
            //如果未跟踪
            if (DbContextBase.Entry(model).State == EntityState.Detached)
            {
                //变更实体未跟踪修改状态
                this.SetEntityState(model, EntityState.Deleted);
            }

            this.DbSet.Remove(model);
            return this.DbContextBase.SaveChangesAsync();
        }

        /// <summary>
        /// 删除 批量
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteAsync(IEnumerable<T> models)
        {
            this.DbSet.RemoveRange(models);
            return this.DbContextBase.SaveChangesAsync();
        }

        /// <summary>
        /// 删除 根据表达式
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteAsync(Expression<Func<T, bool>> expWhere)
            => this.DeleteAsync(this.Query().Where(expWhere));

        /// <summary>
        /// 删除 根据表达式 直接 生成 delete from table  where 语句操作
        /// </summary>
        /// <param name="expWhere"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteBulkAsync(Expression<Func<T, bool>> expWhere)
        {
            var deleteParser = new DeleteParser(DbContextBase, this.Select.Where(expWhere).ToQueryString());
            return this.ExecuteSqlRawAsync(deleteParser.Parser());
        }

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
        public virtual async Task<int> DeleteByIdsAsync<TKey>(IEnumerable<TKey> keys)
        {
            if (_keyPropertyInfo == null) throw new Exception("模型未设置主键特性标记!");
            var exp = ExpressionTreeExtensions.Contains<T, TKey>(_keyPropertyInfo.Name, keys);
            return await this.DeleteAsync(await this.Query().Where(exp).ToListAsync());
        }

        #endregion

    }

}
