using HzyEFCoreRepositories.DbContexts;
using HzyEFCoreRepositories.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HzyEFCoreRepositories.Repositories.Impl
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    public abstract class RepositoryCoreImpl<T, TDbContext> : IRepositoryCore<T, TDbContext>
        where T : class, new()
        where TDbContext : class
    {

        /// <summary>
        /// 数据上下文
        /// </summary>
        protected readonly TDbContext _context;
        /// <summary>
        /// dbset
        /// </summary>
        protected DbSet<T> DbSet => this.DbContextBase.Set<T>();
        /// <summary>
        /// 主键的 PropertyInfo 对象
        /// </summary>
        protected readonly PropertyInfo _keyPropertyInfo;
        /// <summary>
        /// 过滤条件
        /// </summary>
        protected Expression<Func<T, bool>> _filter;
        /// <summary>
        /// 是否忽略过滤
        /// </summary>
        protected bool isIgnoreQueryFilter;

        /// <summary>
        /// 基础仓储
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="filter"></param>
        public RepositoryCoreImpl(TDbContext dbContext, Expression<Func<T, bool>> filter = null)
        {
            _context = dbContext;
            _keyPropertyInfo = typeof(T).GetKeyProperty(false);
            _filter = filter;
            isIgnoreQueryFilter = false;
        }

        /// <summary>
        /// 设置 跟踪 Attachq
        /// </summary>
        /// <param name="model"></param>
        /// <param name="entityState"></param>
        public virtual void SetEntityState(T model, EntityState entityState)
        {
            DbContextBase.Entry(model).State = entityState;
        }

        /// <summary>
        /// 取消了实体对象的追踪操作 需要调用此函数 才能进行对实体数据库操作
        /// <para>
        /// 用于取消旧实体追踪缓存 防止出现 id 重复问题
        /// </para>
        ///  
        /// <para>
        /// 此函数解决的问题可以看此案例： https://blog.51cto.com/u_15064638/4401901
        /// </para>
        /// 
        /// </summary>
        /// <param name="detachedWhere"></param>
        public virtual void DettachWhenExist(Func<T, bool> detachedWhere)
        {
            var local = DbSet.Local.FirstOrDefault(detachedWhere);
            if (local == null) return;
            this.SetEntityState(local, EntityState.Detached);
        }

        /// <summary>
        /// 生成表达式树 例如：( w=>w.Key==Guid.Empty )
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual Expression<Func<T, bool>> GetKeyExpression(object value)
            => ExpressionTreeExtensions.Equal<T, object>(this._keyPropertyInfo.Name, value);

        /// <summary>
        /// 获取 dbcontext 对象
        /// </summary>
        /// <returns></returns>
        public virtual TDbContext Orm
        {
            get
            {
                return _context;
            }
        }

        /// <summary>
        /// 获取 dbcontext 对象
        /// </summary>
        /// <typeparam name="TDbContextResult"></typeparam>
        /// <returns></returns>
        public virtual TDbContextResult GetDbContext<TDbContextResult>() where TDbContextResult : DbContextBase
            => this.Orm as TDbContextResult;

        /// <summary>
        /// 获取上下文基础对象 DbContextBase
        /// </summary>
        /// <returns></returns>
        public virtual DbContextBase DbContextBase => GetDbContext<DbContextBase>();

        /// <summary>
        /// 工作单元
        /// </summary>
        public virtual IUnitOfWork UnitOfWork => this.DbContextBase.UnitOfWork;

        /// <summary>
        /// 供程序员显式调用的Dispose方法
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async ValueTask DisposeAsync()
        {
            await DbContextBase.DisposeAsync();
            //手动调用了Dispose释放资源，那么析构函数就是不必要的了，这里阻止GC调用析构函数
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 供程序员显式调用的Dispose方法
        /// </summary>
        public virtual void Dispose()
        {
            //调用带参数的Dispose方法，释放托管和非托管资源
            Dispose(true);
            //手动调用了Dispose释放资源，那么析构函数就是不必要的了，这里阻止GC调用析构函数
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// protected的Dispose方法，保证不会被外部调用。
        /// 传入bool值disposing以确定是否释放托管资源
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // TODO:在这里加入清理"托管资源"的代码，应该是xxx.Dispose();
                DbContextBase.Dispose();
            }
            // TODO:在这里加入清理"非托管资源"的代码
        }

        /// <summary>
        /// 供GC调用的析构函数
        /// </summary>
        ~RepositoryCoreImpl()
        {
            Dispose(false);//释放非托管资源
        }



    }
}
