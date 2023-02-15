using HZY.Framework.EntityFrameworkRepositories.Databases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;

namespace HZY.Framework.EntityFrameworkRepositories.DatabaseSchemas.Impl
{
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

    }
}
