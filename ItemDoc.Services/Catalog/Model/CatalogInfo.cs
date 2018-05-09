using System;
using System.ComponentModel.DataAnnotations;
using ItemDoc.Framework.Repositories;

namespace ItemDoc.Services.Model
{
  /// <summary>
  /// 用户登录实体
  /// </summary>
  [SopTableName("Item_Catalog")]
  [SopTablePrimaryKey("Id", AutoIncrement = true)]
  public class CatalogInfo
  {
    #region 默认值
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public CatalogInfo()
    {
      Enabled = true;
      ParentIdList = "";
      DateCreated = DateTime.Now;
    }
    #endregion
    public int Id { get; set; }
    public string UserId { get; set; }

    public int ItemId { get; set; }
    /// <summary>
    /// 分类名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 栏目描述
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// 父id
    /// </summary>
    public int ParentId { get; set; }
    /// <summary>
    /// 父id
    /// </summary>
    public string ParentIdList { get; set; }
    /// <summary>
    /// 子栏目数目
    /// </summary>
    public int ChildCount { get; set; }
    /// <summary>
    /// 深度
    /// </summary>
    public int Depth { get; set; }
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }
    /// <summary>
    /// 内容计数
    /// </summary>
    public int ContentCount { get; set; }

    /// <summary>
    /// 访问计数类型
    /// </summary>
    public ViewCountTypeStatus ViewCountType { get; set; }
    /// <summary>
    /// 排序id
    /// </summary>
    public int DisplayOrder { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime DateCreated { get; set; }

    public string Tags { get; set; }

    public string Color { get; set; }
    public string Url { get; set; }

    public string BackColor { get; set; }

  }
  public enum ViewCountTypeStatus
  {
    /// <summary>
    /// 每次访问记录
    /// </summary>
    [Display(Name = "每次访问")]
    EveryView = 0,
    /// <summary>
    /// 每天访问记录1次
    /// </summary>、
    [Display(Name = "每天访问")]
    EveryDayView = 1,

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "访问一次")]
    EveryOneView = 2,
  }




}
