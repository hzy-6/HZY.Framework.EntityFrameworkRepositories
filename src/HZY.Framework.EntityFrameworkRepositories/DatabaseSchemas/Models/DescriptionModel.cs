namespace HZY.Framework.EntityFrameworkRepositories.DatabaseSchemas.Models;

/// <summary>
/// 说明
/// </summary>
public class DescriptionModel
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
    /// 字段或者表说明
    /// </summary>
    public string Description { get; set; }

}
