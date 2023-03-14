using HZY.Framework.EntityFrameworkRepositories.Databases;
using HZY.Framework.EntityFrameworkRepositories.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HZY.Framework.EntityFrameworkRepositories.DatabaseSchemas.Impl;

/// <summary>
/// 
/// </summary>
public class SqlServerDatabaseSchemaImpl : AbsDatabaseSchemaImpl
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContext"></param>
    public SqlServerDatabaseSchemaImpl(DbContext dbContext) : base(dbContext)
    {

    }

    /// <summary>
    /// 获取所有的表信息
    /// </summary>
    /// <returns></returns>
    public override List<TableModel> GetTables()
    {
        var dbConnection = _dbContext.Database.GetDbConnection();
        var database = dbConnection.Database;

        var sqlString = $@"

                select 

                table_name TableName,
                b.value Description 

                from information_schema.tables a
                left join sys.extended_properties b on object_id(a.table_name) = b.major_id and b.minor_id=0
                where table_catalog ='{database}'
                            ";

        return _dbContext.Database.QueryBySql<TableModel>(sqlString);
    }

    /// <summary>
    /// 获取所有的 列信息
    /// </summary>
    /// <returns></returns>
    public override List<ColumnModel> GetColumns()
    {
        var dbConnection = _dbContext.Database.GetDbConnection();
        var database = dbConnection.Database;

        var sqlString = $@"

                            SELECT

                                table_catalog TableCatalog,
                                table_schema TableSchema,
                                table_name TableName,
                                column_name ColumnName,
                                ordinal_position OrdinalPosition,
                                column_default ColumnDefault,
                                case when is_nullable='YES' then 1 else 0 end IsNullable,
                                data_type DataType,
                                0 IsPrimaryKey,
                                NULL Description,
                                OBJECT_ID(table_name) major_id

                                from information_schema.columns 
                                where table_catalog = '{database}'

                            ";

        var columns = _dbContext.Database.QueryDicBySql(sqlString);

        var propertes = _dbContext.Database.QueryDicBySql(@"select 
major_id,minor_id,value,a.name
from sys.columns a    
left join sys.extended_properties b on a.object_id=b.major_id AND a.column_id = b.minor_id and b.minor_id>0
where value is not null");

        var keys = _dbContext.Database.QueryDicBySql("select CONSTRAINT_NAME,TABLE_NAME TableName,COLUMN_NAME ColumnName,ORDINAL_POSITION OrdinalPosition from INFORMATION_SCHEMA.key_column_usage");

        foreach (var item in columns)
        {
            var major_id = item["major_id"].ToString();
            var tableName = item["TableName"].ToString();
            var columnName = item["ColumnName"].ToString();

            var description = propertes
                .Where(w => w["major_id"].ToString() == major_id && w["name"].ToString() == columnName)
                .Select(w => w["value"]?.ToString())?
                .FirstOrDefault()
                ;

            var primaryKey = keys
                .Where(w => w["TableName"].ToString() == tableName && w["ColumnName"].ToString() == columnName)
                .Where(w => w["CONSTRAINT_NAME"].ToString().StartsWith("PK_"))
                .Any()
                ;

            item["IsPrimaryKey"] = primaryKey;
            item["Description"] = description;
        }

        return JsonConvert.DeserializeObject<List<ColumnModel>>(JsonConvert.SerializeObject(columns));
    }

}
