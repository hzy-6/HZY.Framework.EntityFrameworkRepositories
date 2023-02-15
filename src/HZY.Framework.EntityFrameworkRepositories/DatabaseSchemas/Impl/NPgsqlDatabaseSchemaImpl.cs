using HZY.Framework.EntityFrameworkRepositories.Databases;
using HZY.Framework.EntityFrameworkRepositories.DatabaseSchemas.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZY.Framework.EntityFrameworkRepositories.DatabaseSchemas.Impl
{
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

        public override List<ColumnModel> GetColumns()
        {
            throw new NotImplementedException();
        }

        public override List<TableModel> GetTables()
        {
            throw new NotImplementedException();
        }
    }
}
