using HzyEFCoreRepositories.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HzyEFCoreRepositories.ExpressionTree
{
    /// <summary>
    /// 更新 语法解析器
    /// </summary>
    public class UpdateParserExpressionVisitor : ExpressionVisitor
    {
        /// <summary>
        /// StringBuilder
        /// </summary>
        public readonly StringBuilder _stringBuilder;

        /// <summary>
        /// UpdateParser
        /// </summary>
        public UpdateParserExpressionVisitor()
        {
            _stringBuilder = new StringBuilder();
        }

        /// <summary>
        /// VisitBinary
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            this.Visit(node.Left);
            this._stringBuilder.Append(this.GetOperatorStr(node.NodeType));
            this.Visit(node.Right);
            return node;
        }

        /// <summary>
        /// VisitMember
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            var value = ExpressionTreeExtensions.Eval(node);

            if (value == null)
            {
                this._stringBuilder.Append(node.Member.Name);
            }
            else
            {
                this._stringBuilder.Append(value);
            }

            return node;
        }

        /// <summary>
        /// VisitConstant
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitConstant(ConstantExpression node)
        {
            this._stringBuilder.Append(node.Value);
            return node;
        }

        /// <summary>
        /// VisitUnary
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitUnary(UnaryExpression node)
        {
            return base.Visit(node.Operand);
        }

        /// <summary>
        /// _stringBuilder
        /// </summary>
        /// <returns></returns>
        public StringBuilder GetStringBuilder()
        {
            return this._stringBuilder;
        }

        /// <summary>
        /// GetOperatorStr
        /// </summary>
        /// <param name="_ExpressionType"></param>
        /// <returns></returns>
        private string GetOperatorStr(ExpressionType _ExpressionType)
        {
            switch (_ExpressionType)
            {
                case ExpressionType.Equal: return " = ";
                case ExpressionType.Add: return " + ";
                case ExpressionType.Subtract: return " - ";
                case ExpressionType.Multiply: return " * ";
                case ExpressionType.Divide: return " / ";
                case ExpressionType.Modulo: return " % ";
                default: throw new Exception("无法解析的表达式！");
            }
        }

    }
}
