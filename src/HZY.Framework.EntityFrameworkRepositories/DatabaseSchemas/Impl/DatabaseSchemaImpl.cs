using HZY.Framework.EntityFrameworkRepositories.Databases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZY.Framework.EntityFrameworkRepositories.DatabaseSchemas.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class DatabaseSchemaImpl : IDatabaseSchema
    {
        private readonly DbContext _dbContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public DatabaseSchemaImpl(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual List<Table> GetTables()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual List<Column> GetColumns()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual List<DataType> GetDataTypes()
        {
            _dbContext.Database.OpenConnection();
            var conn = _dbContext.Database.GetDbConnection();

            var dataTypes = conn.GetSchema("DataTypes");

            var result = new List<DataType>();

            foreach (DataRow item in dataTypes.Rows)
            {
                var dataType = new DataType();
                dataType.TypeName = item[nameof(DataType.TypeName)]?.ToString() ?? default;
                dataType.CreateFormat = item[nameof(DataType.CreateFormat)]?.ToString() ?? default;
                dataType.CSharpDataType = item[nameof(DataType)]?.ToString() ?? default;

                if (string.IsNullOrWhiteSpace(dataType.CSharpDataType))
                {
                    dataType.CSharpDataType = "System.String";
                }

                dataType.CSharpDataTypeAbbreviation = dataType.CSharpDataType.Replace("System.", "");

                result.Add(dataType);
            }

            return result;
        }

    }
}
