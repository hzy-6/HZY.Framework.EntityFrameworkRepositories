using HzyEFCoreRepositories.ExpressionTree;
using HzyEFCoreRepositories.Extensions;
using HzyEFCoreRepositories.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HzyEFCoreRepositories.Extensions.Parser
{
    /// <summary>
    /// Update 扩展
    /// </summary>
    public class UpdateParser : BaseParser
    {
        private readonly string _sourceSql;
        private readonly MemberInitExpression _memberInitExpression;
        private readonly List<DataParameter> _dataParameter;
        private readonly List<string> _ignoreColumns;
        private readonly string symbolStart = "";
        private readonly string symbolEnd = "";
        private readonly string parametricSymbols = "";

        /// <summary>
        /// UpdateParser
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sourceSql"></param>
        /// <param name="memberInitExpression"></param>
        /// <param name="ignoreColumns"></param>
        public UpdateParser(DbContext dbContext, string sourceSql, MemberInitExpression memberInitExpression, List<string> ignoreColumns) : base(dbContext)
        {
            _sourceSql = sourceSql;
            _memberInitExpression = memberInitExpression;
            _dataParameter = new List<DataParameter>();
            var item = this.KeywordHandle();
            symbolStart = item.SymbolStart;
            symbolEnd = item.SymbolEnd;
            parametricSymbols = item.ParametricSymbols;
            _ignoreColumns = ignoreColumns;
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

            //解析出 set 部分
            var sets = AnalysisMemberInitExpression();

            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"{declare} \r\n\r\n");
            //语句主体
            stringBuilder.Append($"UPDATE{tableName} \r\n");
            stringBuilder.Append($"SET {string.Join(',', sets)}\r\n");
            stringBuilder.Append($"{whereSqlString} \r\n");

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 数据参数对象
        /// </summary>
        public List<IDataParameter> GetDataParameters()
        {
            if (this._dbContext.Database.IsSqlServer())
            {
                return _dataParameter
                    .Select(w => (IDataParameter)new SqlParameter(w.ParameterName, w.Value))
                    .ToList()
                    ;
            }

            if (this._dbContext.Database.IsMySql())
            {
                return _dataParameter
                    .Select(w => (IDataParameter)new MySqlParameter(w.ParameterName, w.Value))
                    .ToList()
                    ;
            }

            if (this._dbContext.Database.IsNpgsql())
            {
                return _dataParameter
                    .Select(w => (IDataParameter)new NpgsqlParameter(w.ParameterName, w.Value))
                    .ToList()
                    ;
            }

            if (this._dbContext.Database.IsOracle())
            {
                return _dataParameter
                    .Select(w => (IDataParameter)new OracleParameter(w.ParameterName, w.Value))
                    .ToList()
                    ;
            }

            throw new Exception("暂时不支持数据库类型!");

        }

        /// <summary>
        /// 分析 MemberInitExpression 表达式
        /// </summary>
        /// <returns></returns>
        private List<string> AnalysisMemberInitExpression()
        {
            var result = new List<string>();
            foreach (MemberAssignment item in _memberInitExpression.Bindings)
            {
                var memberName = item.Member.Name;
                if (_ignoreColumns.Any(w => w.ToLower() == memberName.ToLower()))
                {
                    continue;
                }

                if (item.Expression is BinaryExpression)
                {
                    var updateParser = new UpdateParserExpressionVisitor();
                    updateParser.Visit(item.Expression);
                    result.Add($"{Symbol(memberName)} = {updateParser.GetStringBuilder()}");
                }
                else
                {
                    var key = $"{parametricSymbols}{memberName}_P{this._dataParameter.Count}";
                    var value = ExpressionTreeExtensions.Eval(item.Expression);
                    result.Add($"{Symbol(memberName)} = {key}");
                    this._dataParameter.Add(new DataParameter(key, value));
                }
            }

            return result;
        }

        private string Symbol(string name)
        {
            return $"{this.symbolStart}{name}{this.symbolEnd}";
        }



    }


    /// <summary>
    /// 更新忽略解析
    /// </summary>
    public class UpdateIgnoreParser<T>
    {
        private readonly List<string> columns;

        /// <summary>
        /// UpdateIgnoreParser
        /// </summary>
        public UpdateIgnoreParser()
        {
            columns = new List<string>();
        }

        /// <summary>
        /// 忽略 列
        /// </summary>
        /// <typeparam name="F"></typeparam>
        /// <param name="if"></param>
        /// <param name="columnsExpression"></param>
        public void AddIgnore<F>(bool @if, Expression<Func<T, F>> columnsExpression)
        {
            if (@if) return;
            columns.Add(NotMappedColAnalysis(columnsExpression));
        }

        /// <summary>
        /// 忽略 列
        /// </summary>
        /// <typeparam name="F"></typeparam>
        /// <param name="columnsExpression"></param>
        public void AddIgnore<F>(Expression<Func<T, F>> columnsExpression)
        {
            columns.Add(NotMappedColAnalysis(columnsExpression));
        }

        /// <summary>
        /// 获取忽略列
        /// </summary>
        /// <returns></returns>
        public List<string> GetIgnoreColumns() => columns;

        /// <summary>
        /// 忽略列解析
        /// </summary>
        /// <param name="columnsExpression"></param>
        /// <returns></returns>
        protected string NotMappedColAnalysis<F>(Expression<Func<T, F>> columnsExpression)
        {
            var body = columnsExpression.Body;

            if (body is UnaryExpression)
            {
                var unaryExpression = body as UnaryExpression;
                var operand = unaryExpression.Operand;
                if (operand is MemberExpression)
                {
                    var memberExpression = operand as MemberExpression;
                    return memberExpression.Member.Name;
                }
            }
            else if (body is MemberExpression)
            {
                var memberExpression = body as MemberExpression;
                return memberExpression.Member.Name;
            }

            throw new Exception("NotMapped 语法无法解析!");
        }
    }
}
