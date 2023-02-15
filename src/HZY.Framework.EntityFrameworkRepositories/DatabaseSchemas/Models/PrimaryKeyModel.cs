using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZY.Framework.EntityFrameworkRepositories.DatabaseSchemas.Models
{
    /// <summary>
    /// 主键
    /// </summary>
    public class PrimaryKeyModel
    {

        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 列名称
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 主键名称
        /// </summary>
        public string PrimaryKeyName { get; set; }

    }
}
