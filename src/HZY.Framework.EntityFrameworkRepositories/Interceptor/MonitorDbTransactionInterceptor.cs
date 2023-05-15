namespace HZY.Framework.EntityFrameworkRepositories.Interceptor;

/// <summary>
/// 监控数据库事务信息
/// Efcore 拦截监控文档: https://docs.microsoft.com/en-us/ef/core/logging-events-diagnostics/interceptors
/// </summary>
public class MonitorDbTransactionInterceptor : DbTransactionInterceptor
{
    /// <summary>
    /// RolledBackToSavepoint
    /// </summary>
    /// <param name="transaction"></param>
    /// <param name="eventData"></param>
    public override void RolledBackToSavepoint(DbTransaction transaction, TransactionEventData eventData)
    {
        EntityFrameworkRepositoriesMonitorCache.RollBackCount();
        base.RolledBackToSavepoint(transaction, eventData);
    }

    /// <summary>
    /// RolledBackToSavepointAsync
    /// </summary>
    /// <param name="transaction"></param>
    /// <param name="eventData"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override Task RolledBackToSavepointAsync(DbTransaction transaction, TransactionEventData eventData, CancellationToken cancellationToken = default)
    {
        EntityFrameworkRepositoriesMonitorCache.RollBackCount();
        return base.RolledBackToSavepointAsync(transaction, eventData, cancellationToken);
    }

    /// <summary>
    /// TransactionCommitted
    /// </summary>
    /// <param name="transaction"></param>
    /// <param name="eventData"></param>
    public override void TransactionCommitted(DbTransaction transaction, TransactionEndEventData eventData)
    {
        EntityFrameworkRepositoriesMonitorCache.SubmitTransactionCount();
        base.TransactionCommitted(transaction, eventData);
    }

    /// <summary>
    /// TransactionCommittedAsync
    /// </summary>
    /// <param name="transaction"></param>
    /// <param name="eventData"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override Task TransactionCommittedAsync(DbTransaction transaction, TransactionEndEventData eventData, CancellationToken cancellationToken = default)
    {
        EntityFrameworkRepositoriesMonitorCache.SubmitTransactionCount();
        return base.TransactionCommittedAsync(transaction, eventData, cancellationToken);
    }

    /// <summary>
    /// TransactionFailed
    /// </summary>
    /// <param name="transaction"></param>
    /// <param name="eventData"></param>
    public override void TransactionFailed(DbTransaction transaction, TransactionErrorEventData eventData)
    {
        EntityFrameworkRepositoriesMonitorCache.TransactionFailedCount();
        base.TransactionFailed(transaction, eventData);
    }

    /// <summary>
    /// TransactionFailedAsync
    /// </summary>
    /// <param name="transaction"></param>
    /// <param name="eventData"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override Task TransactionFailedAsync(DbTransaction transaction, TransactionErrorEventData eventData, CancellationToken cancellationToken = default)
    {
        EntityFrameworkRepositoriesMonitorCache.TransactionFailedCount();
        return base.TransactionFailedAsync(transaction, eventData, cancellationToken);
    }

    /// <summary>
    /// TransactionRolledBack
    /// </summary>
    /// <param name="transaction"></param>
    /// <param name="eventData"></param>
    public override void TransactionRolledBack(DbTransaction transaction, TransactionEndEventData eventData)
    {
        EntityFrameworkRepositoriesMonitorCache.RollBackCount();
        base.TransactionRolledBack(transaction, eventData);
    }

    /// <summary>
    /// TransactionRolledBackAsync
    /// </summary>
    /// <param name="transaction"></param>
    /// <param name="eventData"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override Task TransactionRolledBackAsync(DbTransaction transaction, TransactionEndEventData eventData, CancellationToken cancellationToken = default)
    {
        EntityFrameworkRepositoriesMonitorCache.RollBackCount();
        return base.TransactionRolledBackAsync(transaction, eventData, cancellationToken);
    }

    /// <summary>
    /// TransactionStarted
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="eventData"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public override DbTransaction TransactionStarted(DbConnection connection, TransactionEndEventData eventData, DbTransaction result)
    {
        EntityFrameworkRepositoriesMonitorCache.CreateTransactionCount();
        return base.TransactionStarted(connection, eventData, result);
    }

    /// <summary>
    /// TransactionStartedAsync
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="eventData"></param>
    /// <param name="result"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override ValueTask<DbTransaction> TransactionStartedAsync(DbConnection connection, TransactionEndEventData eventData, DbTransaction result, CancellationToken cancellationToken = default)
    {
        EntityFrameworkRepositoriesMonitorCache.CreateTransactionCount();
        return base.TransactionStartedAsync(connection, eventData, result, cancellationToken);
    }


}
