using System;
using System.ComponentModel.DataAnnotations;
using ItemDoc.Framework.Repositories;

namespace ItemDoc.Services.ViewModel
{
  /// <summary>
  /// 用户登录实体
  /// </summary>

  public class PostParameter
  {
    /// <summary>
    /// 分类
    /// </summary>
    public int CatalogId { get; set; } = 0;

    public int pageSize { get; set; } = 10;

    public int pageIndex { get; set; } = 0;

    public string keyword { get; set; }

    public SortOrder sortOrder { get; set; }
    public SortName sortName { get; set; }

  }

  /// <summary>
  /// 
  /// </summary>
  public enum SortName
  {
    DateCreated,
    Title,
    ViewCount,
    DisplayOrder

  }

  public enum SortOrder
  {
    [Display(Name = "Desc")]
    Desc = 0,

    [Display(Name = "Asc")]
    Asc = 1

  }




}
