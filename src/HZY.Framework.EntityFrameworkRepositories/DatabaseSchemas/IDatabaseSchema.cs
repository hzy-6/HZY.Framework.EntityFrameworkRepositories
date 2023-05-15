namespace HZY.Framework.EntityFrameworkRepositories.Databases;

/// <summary>
/// 数据库结构
/// </summary>
public interface IDatabaseSchema : IDisposable, IAsyncDisposable
{

    /// <summary>
    /// 获取所有的表
    /// </summary>
    /// <returns></returns>
    public List<TableModel> GetTables();

    /// <summary>
    /// 获取所有的列
    /// </summary>
    /// <returns></returns>
    public List<ColumnModel> GetColumns();

    /// <summary>
    /// 获取所有的数据类型
    /// </summary>
    /// <returns></returns>
    public List<DataTypeModel> GetDataTypes();


}
