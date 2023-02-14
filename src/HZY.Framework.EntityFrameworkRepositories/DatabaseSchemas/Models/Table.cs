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
    public class Table
    {

        /// <summary>
        /// 列
        /// </summary>
        public List<Column> Columns { get; set; }


    }
}
