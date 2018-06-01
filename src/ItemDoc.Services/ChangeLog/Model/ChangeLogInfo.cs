using System;
using ItemDoc.Framework.Repositories;

namespace ItemDoc.Services.ChangeLog.Model
{
  /// <summary>
  /// 更改日志实体
  /// </summary>
  [SopTableName("Item_ChangeLog")]
  [SopTablePrimaryKey("Id", AutoIncrement = true)]

  public class ChangeLogInfo
  {
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public ChangeLogInfo()
    {
      DateCreated = DateTime.Now;
    }
    /// <summary>
    /// 时间事件
    /// </summary>
    public DateTime DataDate { get; set; }
    /// <summary>
    /// 导航标题
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// 摘要
    /// </summary>
    public string Summary { get; set; }
    /// <summary>
    /// 内容
    /// </summary>
    public string Body { get; set; }
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Selected { get; set; }
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime DateCreated { get; set; }


  }

}
