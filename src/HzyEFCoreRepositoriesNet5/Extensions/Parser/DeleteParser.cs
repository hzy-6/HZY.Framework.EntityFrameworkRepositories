using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HzyEFCoreRepositories.Extensions.Parser
{
    /// <summary>
    /// 删除 语法解析器
    /// </summary>
    public class DeleteParser : BaseParser
    {
        private readonly string _sourceSql;
        private readonly string symbolStart = "";
        private readonly string symbolEnd = "";

        /// <summary>
        /// UpdateParser
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sourceSql"></param>
        public DeleteParser(DbContext dbContext, string sourceSql) : base(dbContext)
        {
            _sourceSql = sourceSql;
            var item = this.KeywordHandle();
            symbolStart = item.SymbolStart;
            symbolEnd = item.SymbolEnd;
        }

        /// <summary>
        /// 解析 sql 并分析出 Update 语句
        /// </summary>
        /// <returns></returns>
        public string Parser()
        {
            var sqlList = _sourceSql.Split("\r\n\r\n");
            var declare = sqlList[0];
            var selectSqlString = "";
            for (int i = 0; i < sqlList.Length; i++)
            {
                var item = sqlList[i];
                if (string.IsNullOrWhiteSpace(item) || i == 0) continue;
                selectSqlString += item;
            }

            var tableName = selectSqlString
               .Substring(selectSqlString.IndexOf("FROM"), selectSqlString.IndexOf("AS") - selectSqlString.IndexOf("FROM"))
               .Replace("FROM", "")
               .Replace("AS", "")
               ;

            var whereSqlString = selectSqlString.Substring(selectSqlString.IndexOf("WHERE"));

            //去除 where 条件中的 别名 比如 : [s].
            var reg = new Regex($@"\{symbolStart}[a-z]\{symbolEnd}.");
            whereSqlString = reg.Replace(whereSqlString, "");

            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"{declare} \r\n\r\n");
            //语句主体
            stringBuilder.Append($"DELETE{tableName} \r\n");
            stringBuilder.Append($"{whereSqlString} \r\n");

            return stringBuilder.ToString();
        }


    }
}
