using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZY.Framework.EntityFrameworkRepositories.Extensions.Parser
{
    /// <summary>
    /// BaseParser
    /// </summary>
    public class BaseParser
    {
        /// <summary>
        /// _dbContext
        /// </summary>
        public readonly DbContext _dbContext;

        /// <summary>
        /// BaseParser
        /// </summary>
        /// <param name="dbContext"></param>
        public BaseParser(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 关键字处理
        /// </summary>
        /// <returns></returns>
        public (string SymbolStart, string SymbolEnd, string ParametricSymbols) KeywordHandle()
        {
            string symbolStart = string.Empty, symbolEnd = string.Empty, parametricSymbols = string.Empty;

            if (_dbContext.Database.IsSqlServer())
            {
                symbolStart = "[";
                symbolEnd = "]";
                parametricSymbols = "@";
            }

            if (_dbContext.Database.IsMySql())
            {
                symbolStart = "`";
                symbolEnd = "`";
                parametricSymbols = "?";
            }

            if (_dbContext.Database.IsNpgsql())
            {
                symbolStart = "\"";
                symbolEnd = "\"";
                parametricSymbols = "@";
            }

            if (_dbContext.Database.IsOracle())
            {
                symbolStart = "\"";
                symbolEnd = "\"";
                parametricSymbols = ":";
            }

            return (symbolStart, symbolEnd, parametricSymbols);
        }


    }
}
