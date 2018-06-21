using System;
using ItemDoc.Framework.Repositories;

namespace ItemDoc.Services.Model
{
  /// <summary>
  /// 用户登录实体
  /// </summary>
  [SopTableName("Item_FileServer")]
  [SopTablePrimaryKey("Id", AutoIncrement = true)]
  public class FileServerInfo
  {

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public FileServerInfo()
    {
      DateCreated = DateTime.Now;
      // LastUpdateTime = DateTime.Now;

    }
    public int Id { get; set; }
    
    public string ServerId { get; set; }
    public string ServerName { get; set; }
    public string ServerUrl { get; set; }
    public string PicRootPath { get; set; }
    public string MaxPicAmount { get; set; }
    public string CurPicAmount { get; set; }
    public bool Usable { get; set; }
    public bool Enabled { get; set; }

    public int DisplayOrder { get; set; }

    /// <summary>
    /// Gets or sets the date created.
    /// </summary>
    public DateTime DateCreated { get; set; } = DateTime.Now;
  }





}
