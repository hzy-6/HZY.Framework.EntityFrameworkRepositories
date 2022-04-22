using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HzyEFCoreRepositoriesTest.Models;

/// <summary>
/// 功能
/// </summary>
public class SysFunction
{


    /// <summary>
    /// 编号
    /// </summary>
    public int? Number { get; set; }

    /// <summary>
    /// 功能名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 功能标识
    /// </summary>
    public string ByName { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public virtual DateTime LastModificationTime { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public virtual DateTime CreationTime { get; set; }

    [Key]
    public Guid Id { get; set; }
}
