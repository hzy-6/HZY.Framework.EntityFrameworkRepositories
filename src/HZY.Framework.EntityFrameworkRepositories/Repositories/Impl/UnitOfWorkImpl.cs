/*
 * *******************************************************
 *
 * 作者：hzy
 *
 * 开源地址：https://gitee.com/hzy6
 *
 * *******************************************************
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace HZY.Framework.EntityFrameworkRepositories.Repositories.Impl;

/// <summary>
/// 工作单元
/// </summary>
public class UnitOfWorkImpl<TDbContext> : IUnitOfWork where TDbContext : DbContext
{
    private bool _saveState = true;
    private readonly TDbContext _dbContext;

    /// <summary>
    /// 工作单元 构造
    /// </summary>
    /// <param name="dbContext"></param>
    public UnitOfWorkImpl(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// 获取延迟保存状态
    /// </summary>
    /// <returns></returns>
    public virtual bool GetDelaySaveState() => _saveState;

    /// <summary>
    /// 设置延迟保存状态
    /// </summary>
    /// <param name="saveSate"></param>
    public virtual void SetDelaySaveState(bool saveSate) => _saveState = saveSate;

    /// <summary>
    /// 打开延迟提交
    /// </summary>
    public virtual void CommitDelayStart() => _saveState = false;

    /// <summary>
    /// 延迟提交结束
    /// </summary>
    /// <returns></returns>
    public virtual int CommitDelayEnd()
    {
        SetDelaySaveState(true);
        return _dbContext.SaveChanges();
    }

    /// <summary>
    /// 延迟提交结束
    /// </summary>
    /// <returns></returns>
    public virtual Task<int> CommitDelayEndAsync()
    {
        SetDelaySaveState(true);
        return _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 开始事务
    /// </summary>
    /// <returns></returns>
    public virtual IDbContextTransaction BeginTransaction() => _dbContext.Database.BeginTransaction();

    /// <summary>
    /// 开始事务
    /// </summary>
    /// <returns></returns>
    public virtual Task<IDbContextTransaction> BeginTransactionAsync() => _dbContext.Database.BeginTransactionAsync();

    /// <summary>
    /// 获取当前 dbcontext 事务
    /// </summary>
    public virtual IDbContextTransaction CurrentDbContextTransaction => _dbContext.Database.CurrentTransaction;

    /// <summary>
    /// 获取当前 事务
    /// </summary>
    public virtual IDbTransaction CurrentDbTransaction => GetDbTransaction(_dbContext.Database.CurrentTransaction);

    /// <summary>
    /// 获取当前 事务 根据 IDbContextTransaction 事务
    /// </summary>
    /// <param name="dbContextTransaction"></param>
    /// <returns></returns>
    public virtual IDbTransaction GetDbTransaction(IDbContextTransaction dbContextTransaction)
    {
        return dbContextTransaction.GetDbTransaction();
    }

    /// <summary>
    /// 获取 dbset 对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public virtual DbSet<T> DbSet<T>() where T : class, new() => _dbContext.Set<T>();

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <returns></returns>
    public virtual int SaveChanges()
    {
        return GetDelaySaveState() ? _dbContext.SaveChanges() : 1;
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess"></param>
    /// <returns></returns>
    public virtual int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        return GetDelaySaveState() ? _dbContext.SaveChanges(acceptAllChangesOnSuccess) : 1;
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <returns></returns>
    public virtual Task<int> SaveChangesAsync()
    {
        return GetDelaySaveState() ? _dbContext.SaveChangesAsync() : Task.FromResult(1);
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return GetDelaySaveState() ? _dbContext.SaveChangesAsync(cancellationToken) : Task.FromResult(1);
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        return GetDelaySaveState() ? _dbContext.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken) : Task.FromResult(1);
    }
}

