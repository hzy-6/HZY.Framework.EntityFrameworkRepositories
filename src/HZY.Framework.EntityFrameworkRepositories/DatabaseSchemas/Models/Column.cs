using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HZY.Framework.EntityFrameworkRepositories.Databases
{
    /// <summary>
    /// 列信息
    /// </summary>
    public class Column
    {

        //TABLE_CATALOG,
        //TABLE_SCHEMA,
        //TABLE_NAME,
        //COLUMN_NAME,
        //ORDINAL_POSITION,
        //COLUMN_DEFAULT,
        //IS_NULLABLE,
        //DATA_TYPE,
        //CHARACTER_MAXIMUM_LENGTH,
        //CHARACTER_OCTET_LENGTH,
        //NUMERIC_PRECISION,
        //NUMERIC_PRECISION_RADIX,
        //NUMERIC_SCALE,
        //DATETIME_PRECISION,
        //CHARACTER_SET_CATALOG,
        //CHARACTER_SET_SCHEMA,
        //CHARACTER_SET_NAME,
        //COLLATION_CATALOG,
        //IS_SPARSE,
        //IS_COLUMN_SET,
        //IS_FILESTREAM

        /// <summary>
        /// 表_目录
        /// </summary>
        public string TABLE_CATALOG { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TABLE_SCHEMA { get; set; }


    }
}
