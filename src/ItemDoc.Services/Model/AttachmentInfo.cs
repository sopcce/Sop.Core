using System;
using ItemDoc.Framework.Repositories;

namespace ItemDoc.Services.Model
{
  /// <summary>
  /// 
  /// </summary>

  [SopTableName("Item_Attachment")]
  [SopTablePrimaryKey("Id", AutoIncrement = true)]
  public class AttachmentInfo
  {

    /// <summary>
    /// 附件ID
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// UE内容ID
    /// </summary>
    public string AttachmentId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string OwnerId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string ServerId { get; set; }

    public AttachmentType FileType { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Extension { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int Size { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Filenames { get; set; }
    public string FriendlyFilenames { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string UploadFileName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string MimeType { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string MediaType { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public AttachmentStatus Status { get; set; }
    public int DisplayOrder { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string IP { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime DateCreated { get; set; }


  }

  public enum AttachmentType
  {
    /// <summary>
    /// 未使用（不及格数据）
    /// </summary>
    Fail = 1,

    /// <summary>
    /// 提交
    /// </summary>
    Submit = 2,

    /// <summary>
    /// 删除
    /// </summary>
    Delete = 3,
  }

  /// <summary>
  /// 附加状态
  /// </summary>
  public enum AttachmentStatus
  {
    /// <summary>
    /// 未使用（不及格数据）
    /// </summary>
    Fail = 1,

    /// <summary>
    /// 提交
    /// </summary>
    Submit = 2,

    /// <summary>
    /// 删除
    /// </summary>
    Delete = 3,
  }
}