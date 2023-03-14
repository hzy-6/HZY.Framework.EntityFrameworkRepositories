using HZY.Framework.EntityFrameworkRepositories.Databases;
using HZY.Framework.EntityFrameworkRepositories.Extensions;
using Microsoft.EntityFrameworkCore;

namespace HZY.Framework.EntityFrameworkRepositories.DatabaseSchemas.Impl;

/// <summary>
/// 
/// </summary>
public class MySqlDatabaseSchemaImpl : AbsDatabaseSchemaImpl
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContext"></param>
    public MySqlDatabaseSchemaImpl(DbContext dbContext) : base(dbContext)
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

                            SELECT 

                            table_name TableName,
                            table_comment Description 

                            from information_schema.tables 
                            where table_schema ='{database}'

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
                            IF(is_nullable='YES',TRUE,FALSE) IsNullable,
                            data_type DataType,
                            IF(column_key='PRI',TRUE,FALSE) IsPrimaryKey,
                            column_comment Description

                            from information_schema.columns 
                            where table_schema = '{database}'

                            ";

        return _dbContext.Database.QueryBySql<ColumnModel>(sqlString);
    }


}
