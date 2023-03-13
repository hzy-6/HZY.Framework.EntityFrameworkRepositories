using HZY.Framework.EntityFrameworkRepositories.Databases;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HZY.Framework.EntityFrameworkRepositories.DatabaseSchemas.Impl;

/// <summary>
/// 数据库结构实现类
/// </summary>
public abstract class AbsDatabaseSchemaImpl : IDatabaseSchema
{
    /// <summary>
    /// 数据上下文
    /// </summary>
    protected readonly DbContext _dbContext;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContext"></param>
    public AbsDatabaseSchemaImpl(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// 获取所有的表
    /// </summary>
    /// <returns></returns>
    public abstract List<TableModel> GetTables();

    /// <summary>
    /// 获取所有的列
    /// </summary>
    /// <returns></returns>
    public abstract List<ColumnModel> GetColumns();

    /// <summary>
    /// 获取所有的数据类型
    /// </summary>
    /// <returns></returns>
    public virtual List<DataTypeModel> GetDataTypes()
    {
        _dbContext.Database.OpenConnection();
        var conn = _dbContext.Database.GetDbConnection();

        var dataTypes = conn.GetSchema("DataTypes");

        var result = new List<DataTypeModel>();

        foreach (DataRow item in dataTypes.Rows)
        {
            var dataType = new DataTypeModel();
            dataType.TypeName = item[nameof(DataTypeModel.TypeName)] == DBNull.Value ? default : item[nameof(DataTypeModel.TypeName)]?.ToString();
            dataType.CreateFormat = item[nameof(DataTypeModel.CreateFormat)] == DBNull.Value ? default : item[nameof(DataTypeModel.CreateFormat)]?.ToString();
            dataType.CSharpDataType = item["DataType"] == DBNull.Value ? default : item["DataType"]?.ToString();

            if (string.IsNullOrWhiteSpace(dataType.CSharpDataType))
            {
                dataType.CSharpDataType = "System.String";
            }

            dataType.CSharpDataTypeAbbreviation = dataType.CSharpDataType.Replace("System.", "");

            result.Add(dataType);
        }

        return result;
    }


    /// <summary>
    /// 供程序员显式调用的Dispose方法
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
        //手动调用了Dispose释放资源，那么析构函数就是不必要的了，这里阻止GC调用析构函数
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 供程序员显式调用的Dispose方法
    /// </summary>
    public virtual void Dispose()
    {
        //调用带参数的Dispose方法，释放托管和非托管资源
        Dispose(true);
        //手动调用了Dispose释放资源，那么析构函数就是不必要的了，这里阻止GC调用析构函数
        GC.SuppressFinalize(this);
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
            _dbContext.Dispose();
        }
        // TODO:在这里加入清理"非托管资源"的代码
    }

    /// <summary>
    /// 供GC调用的析构函数
    /// </summary>
    ~AbsDatabaseSchemaImpl()
    {
        Dispose(false);//释放非托管资源
    }


}
