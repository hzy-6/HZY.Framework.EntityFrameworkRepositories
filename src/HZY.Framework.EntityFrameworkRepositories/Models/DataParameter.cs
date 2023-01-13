using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZY.Framework.EntityFrameworkRepositories.Models
{

    /// <summary>
    /// 数据参数
    /// </summary>
    public class DataParameter : DbParameter
    {
        public DataParameter()
        {

        }

        public DataParameter(string ParameterName, object Value)
        {
            this.ParameterName = ParameterName;
            this.Value = Value;
        }

        public DataParameter(string ParameterName, object Value, DbType DbType)
        {
            this.ParameterName = ParameterName;
            this.Value = Value;
            this.DbType = DbType;
        }

        public override DbType DbType { get; set; }
        public override ParameterDirection Direction { get; set; }
        public override bool IsNullable { get; set; }
        public override string ParameterName { get; set; }
        public override int Size { get; set; }
        public override string SourceColumn { get; set; }
        public override bool SourceColumnNullMapping { get; set; }
        public override object Value { get; set; }
        public override void ResetDbType() => DbType = DbType.Object;

    }
}
