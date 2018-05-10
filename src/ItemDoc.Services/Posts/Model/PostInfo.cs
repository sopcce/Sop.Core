using System;
using ItemDoc.Framework.Repositories;

namespace ItemDoc.Services.Model
{
  /// <summary>
  /// 用户登录实体
  /// </summary>
  [SopTableName("Item_Posts")]
  [SopTablePrimaryKey("Id", AutoIncrement = true)]
  public class PostInfo
  {

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public PostInfo()
    {
      DateCreated = DateTime.Now;
      // LastUpdateTime = DateTime.Now;
    }
    public int Id { get; set; }

    public int CatalogId { get; set; }

    public string UserId { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }
    public string HtmlContentPath { get; set; }

    public int ViewCount { get; set; }
    
    public int DisplayOrder { get; set; }
    /// <summary>
    /// Gets or sets the date created.
    /// </summary>
    public DateTime DateCreated { get; set; } = DateTime.Now;
  }





}
