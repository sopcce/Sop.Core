using System;
using ItemDoc.Framework.Repositories;

namespace ItemDoc.Services.Model
{
  /// <summary>
  /// 用户登录实体
  /// </summary>
  [SopTableName("Item_Article")]
  [SopTablePrimaryKey("Id", AutoIncrement = true)]
  public class ArticleInfo
  {

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public ArticleInfo()
    {
      DateCreated = DateTime.Now;
      // LastUpdateTime = DateTime.Now;

    }
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
    public string UserId { get; set; }

    public string PassWord { get; set; }
    /// <summary>
    /// 密码
    /// </summary>
    public DateTime LastUpdateTime { get; set; } = DateTime.Now;



    public int DisplayOrder { get; set; }
    /// <summary>
    /// Gets or sets the date created.
    /// </summary>
    public DateTime DateCreated { get; set; } = DateTime.Now;
  }





}
