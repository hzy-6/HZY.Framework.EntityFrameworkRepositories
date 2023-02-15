using HZY.Framework.EntityFrameworkRepositories.Databases;
using HZY.Framework.EntityFrameworkRepositories.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HZY.Framework.EntityFrameworkRepositories.DatabaseSchemas.Impl
{
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
            var sqlString = @"

                            SELECT a.name TableName,
                            b.value Description 

                            FROM dbo.sysobjects a
                            left join sys.extended_properties b on a.Id = b.major_id and b.minor_id=0
                            Where a.XType='U'

                            ";

            return _dbContext.Database.QueryBySql<TableModel>(sqlString);
        }

        /// <summary>
        /// 获取所有的 列信息
        /// 
        /// </summary>
        /// <returns></returns>
        public override List<ColumnModel> GetColumns()
        {
            var sqlString = @"

https://www.cnblogs.com/qy1234/p/9044275.html

select value from sys.extended_properties where major_id = object_id ('Sys_User' );


select * from information_schema.columns


select * from information_schema.tables




select * from sys.extended_properties



select * from dbo.sysobjects


select * from sys.extended_properties




                            SELECT 

                            c.name TableName,
                            a.name ColumnName,
                            b.value Description

                            FROM dbo.syscolumns a
                            left join sys.extended_properties b on a.id=b.major_id and a.colid=b.minor_id
                            left join dbo.sysobjects c on a.id = c.id AND c.xtype = 'U'
                            where c.name is not null

                            ";

            return _dbContext.Database.QueryBySql<ColumnModel>(sqlString);
        }

    }
}
