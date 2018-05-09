using System;
using ItemDoc.Framework.Repositories;

namespace ItemDoc.Services.Model
{
  /// <summary>
  /// 用户登录实体
  /// </summary>
  [SopTableName("Item_Items")]
  [SopTablePrimaryKey("Id", AutoIncrement = true)]
  public class ItemsInfo
  {

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public ItemsInfo()
    {
      DateCreated = DateTime.Now;
      LastUpdateTime = DateTime.Now;
      Enabled = true;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
    public string UserId { get; set; }

    public string PassWord { get; set; }
    /// <summary>
    /// 父id
    /// </summary>
    public int ParentId { get; set; }

    /// <summary>
    /// 所有父级CategoriyId
    /// </summary>
    public string ParentIdList { get; set; }

    /// <summary>
    /// 子栏目数目
    /// </summary>
    public int ChildCount { get; set; }

    /// <summary>
    /// 深度(从0开始)
    /// </summary>
    public int Depth { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }

    public DateTime LastUpdateTime { get; set; } = DateTime.Now;


    public int DisplayOrder { get; set; }
    /// <summary>
    /// Gets or sets the date created.
    /// </summary>
    public DateTime DateCreated { get; set; } = DateTime.Now;
  }





}
