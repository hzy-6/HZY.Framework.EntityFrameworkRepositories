using HZY.Framework.EntityFrameworkRepositories.DatabaseSchemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZY.Framework.EntityFrameworkRepositories.Databases
{
    /// <summary>
    /// 数据库结构
    /// </summary>
    public interface IDatabaseSchema
    {

        /// <summary>
        /// 获取所有的表
        /// </summary>
        /// <returns></returns>
        public List<Table> GetTables();

        /// <summary>
        /// 获取所有的列
        /// </summary>
        /// <returns></returns>
        public List<Column> GetColumns();

        /// <summary>
        /// 获取所有的数据类型
        /// </summary>
        /// <returns></returns>
        public List<DataType> GetDataTypes();






    }
}
