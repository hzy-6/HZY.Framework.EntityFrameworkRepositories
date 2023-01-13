using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HZY.Framework.EntityFrameworkRepositories.ExpressionTree
{
    /// <summary>
    /// 将拉姆达参数 w=> 部分修改替换
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReplateParameterExpressionVisitor<T> : ExpressionVisitor
    {
        /// <summary>
        /// 被替换后新的参数名称
        /// </summary>
        private readonly string _parameterName;

        /// <summary>
        /// ReplateParameterExpressionVisitor
        /// </summary>
        /// <param name="parameterName"></param>
        public ReplateParameterExpressionVisitor(string parameterName)
        {
            _parameterName = parameterName;
        }

        /// <summary>
        /// Visit
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull("node")]
        public override Expression Visit(Expression node)
        {
            return base.Visit(node);
        }

        /// <summary>
        /// VisitParameter
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return Expression.Parameter(typeof(T), _parameterName);
        }
    }
}