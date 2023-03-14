using HZY.Framework.EntityFrameworkRepositories.Databases;
using HZY.Framework.EntityFrameworkRepositories.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HZY.Framework.EntityFrameworkRepositories.DatabaseSchemas.Impl;

/// <summary>
/// 
/// </summary>
public class NPgsqlDatabaseSchemaImpl : AbsDatabaseSchemaImpl
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContext"></param>
    public NPgsqlDatabaseSchemaImpl(DbContext dbContext) : base(dbContext)
    {
    }

    /// <summary>
    /// 获取所有的表信息
    /// </summary>
    /// <returns></returns>
    public override List<TableModel> GetTables()
    {
        //var dbConnection = _dbContext.Database.GetDbConnection();
        //var database = dbConnection.Database;

        var sqlString = $@"

                            select 
                                a.tablename TableName,
                                d.description Description 
                            from pg_tables a
                            inner join pg_namespace b on b.nspname = a.schemaname
                            inner join pg_class c on c.relnamespace = b.oid and c.relname = a.tablename
                            left join pg_description d on d.objoid = c.oid and objsubid = 0

                            where a.schemaname not in ('pg_catalog', 'information_schema', 'topology')
                            and b.nspname || '.' || a.tablename not in ('public.spatial_ref_sys')

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

        // 查询列数据
        var sqlString = $@"
                            SELECT table_catalog TableCatalog,
                            table_schema TableSchema,
                            table_name TableName,
                            column_name ColumnName,
                            ordinal_position OrdinalPosition,
                            column_default ColumnDefault,
                            is_nullable IsNullable,
                            udt_name DataType,
                            is_identity IsPrimaryKey
                            from information_schema.columns
                            where table_schema = 'public'
                            ";

        var columns = _dbContext.Database.QueryDicBySql(sqlString);

        // 查询表名及关联oid
        var tableSql = $@"
                                    SELECT oid,relname FROM pg_catalog.pg_class
                                    where relkind = 'r' and relname not like 'pg_%' and relname not like 'sql_%' order by relname;
                                    ";
        var tables = _dbContext.Database.QueryDicBySql(tableSql);

        // 查询oid及描述
        var desSql = $@"
                                select objoid,objsubid,description from pg_description as ""b""
                                where ""b"".""objoid"" in (SELECT ""c"".""oid"" FROM pg_catalog.pg_class as ""c"")
                                and ""b"".""objsubid"" > 0
                                order by ""b"".""objsubid""
                                ";
        var descriptions = _dbContext.Database.QueryDicBySql(desSql);

        foreach (var column in columns)
        {
            // 查询列对应的表
            var table = tables.Where(t => t["relname"].ToString() == column["tablename"].ToString()).FirstOrDefault();
            if (table != null)
            {
                // 根据表的id和列的数据点位置，找对应的描述
                var oid = table["oid"].ToString();
                var description = descriptions.Where(d => d["objoid"].ToString() == oid && d["objsubid"].ToString() == column["ordinalposition"].ToString()).FirstOrDefault();
                if (description != null)
                {
                    column["Description"] = description["description"];
                }
                else
                {
                    column["Description"] = "";
                }
            }

            // 字符串转 bool
            if (column["isnullable"].ToString().ToUpper() == "YES")
            {
                column["isnullable"] = true;
            }
            else
            {
                column["isnullable"] = false;
            }

            if (column["isprimarykey"].ToString().ToUpper() == "YES")
            {
                column["isprimarykey"] = true;
            }
            else
            {
                column["isprimarykey"] = false;
            }
        }

        return JsonConvert.DeserializeObject<List<ColumnModel>>(JsonConvert.SerializeObject(columns));
    }



}
