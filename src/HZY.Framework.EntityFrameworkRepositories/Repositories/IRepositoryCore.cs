using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HZY.Framework.EntityFrameworkRepositories.Repositories;

/// <summary>
/// 仓储基础
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TDbContext"></typeparam>
public interface IRepositoryCore<T, TDbContext> : IDisposable, IAsyncDisposable
    where T : class, new()
    where TDbContext : class
{

    /// <summary>
    /// 设置 Attach
    /// </summary>
    /// <param name="model"></param>
    /// <param name="entityState"></param>
    void SetEntityState(T model, EntityState entityState);

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
    void DettachWhenExist(Func<T, bool> detachedWhere);

    /// <summary>
    /// 传入一个 值 获取实体 key 的 表达式
    /// </summary>
    /// <param name="value"></param>
    /// <returns> w => w.Id = 1 </returns>
    Expression<Func<T, bool>> GetKeyExpression(object value);

    /// <summary>
    /// 获取数据上下文
    /// </summary>
    /// <returns></returns>
    TDbContext Orm => default;

    /// <summary>
    /// 获取数据上下文
    /// </summary>
    /// <typeparam name="TDbContextResult"></typeparam>
    /// <returns></returns>
    TDbContextResult GetContext<TDbContextResult>() where TDbContextResult : DbContext;

    /// <summary>
    /// 获取数据上下文 基础对象 DbContext
    /// </summary>
    /// <returns></returns>
    DbContext Context => default;

    /// <summary>
    /// 工作单元
    /// </summary>
    IUnitOfWork UnitOfWork => default;


}
