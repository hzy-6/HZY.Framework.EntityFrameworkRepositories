﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HzyEFCoreRepositories.Models
{
    /// <summary>
    /// 分页查询排序
    /// </summary>
    public class PaginationSearchSort
    {
        /// <summary>
        /// 字段
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public string Order { get; set; }

        /// <summary>
        /// 是否倒序排列
        /// </summary>
        /// <returns></returns>
        public bool IsDesc()
        {
            if (string.IsNullOrWhiteSpace(this.Order))
            {
                return false;
            }

            return this.Order.ToLower().Contains("desc");
        }

        /// <summary>
        /// 排序方式是否为空
        /// </summary>
        /// <returns></returns>
        public bool IsOrderEmpty() => string.IsNullOrWhiteSpace(this.Order);
    }
}
