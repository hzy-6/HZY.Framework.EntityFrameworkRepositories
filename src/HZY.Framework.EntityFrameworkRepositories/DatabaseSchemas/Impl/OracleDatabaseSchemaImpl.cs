using HZY.Framework.EntityFrameworkRepositories.Databases;
using Microsoft.EntityFrameworkCore;

namespace HZY.Framework.EntityFrameworkRepositories.DatabaseSchemas.Impl;

/// <summary>
/// 
/// </summary>
public class OracleDatabaseSchemaImpl : AbsDatabaseSchemaImpl
{
    public OracleDatabaseSchemaImpl(DbContext dbContext) : base(dbContext)
    {
    }

    public override List<ColumnModel> GetColumns()
    {
        throw new NotImplementedException();
    }

    public override List<TableModel> GetTables()
    {
        throw new NotImplementedException();
    }
}
