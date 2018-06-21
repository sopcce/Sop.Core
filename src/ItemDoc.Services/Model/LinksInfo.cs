using System;
using ItemDoc.Framework.Repositories;

namespace ItemDoc.Services.Model
{
  /// <summary>
  /// 链接实体
  /// </summary>

  [SopTableName("Item_Links")]
  [SopTablePrimaryKey("Id", AutoIncrement = true)]
  public class LinksInfo
  {
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public LinksInfo()
    {
      IsEnabled = true;
      DateCreated = DateTime.Now;
      LastModified = DateTime.Now;
    }
    /// <summary>
    /// 拥有者ID 
    /// </summary>
    public string OwnerId { get; set; }
    /// <summary>
    /// 链接名字
    /// </summary>
    public string LinkName { get; set; }
    /// <summary>
    ///链接地址
    /// </summary>
    public string LinkUrl { get; set; }
    /// <summary>
    /// 链接title提示文字为空时取linkName
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 链接属性
    /// </summary>
    public ATarget Target { get; set; }

    /// <summary>
    /// 图片链接地址
    /// </summary>
    public string ImageUrl { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnabled { get; set; }
    /// <summary>
    /// 排序ID，默认与主键相同
    /// </summary>
    public int DisplayOrder { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime DateCreated { get; set; }
    /// <summary>
    /// 最后一次修改时间
    /// </summary>
    public DateTime LastModified { get; set; }


  }

  /// <summary>
  /// 打开方式
  /// </summary>
  public enum ATarget
  {
    /// <summary>
    /// 在新窗口中打开
    /// </summary>
    Blank = 0,
    /// <summary>
    /// 默认、在相同的框架中打开
    /// </summary>
    Self = 1,
    /// <summary>
    /// 在父框架集中打开
    /// </summary>
    Parent = 2,
    /// <summary>
    /// 在整个窗口中打开
    /// </summary>
    Top = 3
  }
}
