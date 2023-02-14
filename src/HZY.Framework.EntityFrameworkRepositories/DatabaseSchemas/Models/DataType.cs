using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZY.Framework.EntityFrameworkRepositories.DatabaseSchemas
{
    /// <summary>
    /// 数据类型
    /// </summary>
    public class DataType
    {
        /// <summary>
        /// 数据库中 类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 数据库中 类型名称 带有长度符号 可格式化
        /// </summary>
        public string CreateFormat { get; set; }

        /// <summary>
        /// 数据类型 C# 对应类型
        /// </summary>
        public string CSharpDataType { get; set; }

        /// <summary>
        /// 数据类型 C# 对应类型 缩写
        /// </summary>
        public string CSharpDataTypeAbbreviation { get; set; }



    }
}
