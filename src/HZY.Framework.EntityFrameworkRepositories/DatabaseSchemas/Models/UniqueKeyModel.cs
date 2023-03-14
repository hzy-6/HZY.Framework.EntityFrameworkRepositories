namespace HZY.Framework.EntityFrameworkRepositories.DatabaseSchemas.Models;

/// <summary>
/// 唯一key
/// </summary>
public class UniqueKeyModel
{

    /// <summary>
    /// 表名称
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    /// 列名称
    /// </summary>
    public string ColumnName { get; set; }

    /// <summary>
    /// 唯一键名称
    /// </summary>
    public string UniqueKeyName { get; set; }
}
