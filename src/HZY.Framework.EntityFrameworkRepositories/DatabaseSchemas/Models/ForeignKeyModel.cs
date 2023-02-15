using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZY.Framework.EntityFrameworkRepositories.DatabaseSchemas.Models
{
    /// <summary>
    /// 外键
    /// </summary>
    public class ForeignKeyModel
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
        /// 外键名称
        /// </summary>
        public string ForeignKeyName { get; set; }


    }
}
