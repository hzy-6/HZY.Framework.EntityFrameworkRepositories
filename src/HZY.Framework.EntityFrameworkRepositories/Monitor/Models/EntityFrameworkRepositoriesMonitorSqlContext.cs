namespace HZY.Framework.EntityFrameworkRepositories.Monitor.Models;

/// <summary>
/// sql 上下文
/// </summary>
public class EntityFrameworkRepositoriesMonitorSqlContext
{

    /// <summary>
    /// sql 脚本
    /// </summary>
    public string Sql { get; set; }

    /// <summary>
    /// 耗时(单位：毫秒)
    /// </summary>
    public long ElapsedMilliseconds { get; set; } = 0;

    /// <summary>
    /// 记录时间
    /// </summary>
    public DateTime Time { get; set; } = DateTime.Now;

}
