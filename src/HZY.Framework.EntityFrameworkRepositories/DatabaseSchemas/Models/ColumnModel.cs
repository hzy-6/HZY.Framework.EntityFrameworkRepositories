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
    public class ColumnModel
    {

        /// <summary>
        /// 表_目录
        /// </summary>
        public string TableCatalog { get; set; }

        /// <summary>
        /// Schema
        /// </summary>
        public string TableSchema { get; set; }

        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 列名称
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 字段顺序
        /// </summary>
        public uint OrdinalPosition { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string ColumnDefault { get; set; }

        /// <summary>
        /// 可空
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 是主键
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

    }
}
