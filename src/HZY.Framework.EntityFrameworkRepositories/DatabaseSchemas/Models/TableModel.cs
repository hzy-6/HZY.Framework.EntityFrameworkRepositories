using HZY.Framework.EntityFrameworkRepositories.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZY.Framework.EntityFrameworkRepositories.DatabaseSchemas
{
    /// <summary>
    /// 表信息
    /// </summary>
    public class TableModel
    {
        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 表说明
        /// </summary>
        public string Description { get; set; }


    }
}
