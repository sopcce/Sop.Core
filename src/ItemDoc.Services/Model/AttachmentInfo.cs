using System;
using ItemDoc.Framework.Repositories;

namespace ItemDoc.Services.Model
{
  /// <summary>
  /// 
  /// </summary>

  [SopTableName("Item_FileAttachment")]
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

    public string ServerUrlPath { get; set; }


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
    public AttachmentStatus Status { get; set; }
    public int DisplayOrder { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Ip { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime DateCreated { get; set; }


  }


  /// <summary>
  /// 附件状态
  /// </summary>
  public enum AttachmentStatus
  {
    /// <summary>
    /// 失败
    /// </summary>
    Fail = 1,
    /// <summary>
    /// 成功
    /// </summary>
    Success = 10,
    /// <summary>
    /// 提交
    /// </summary>
    Submit = 20,

    /// <summary>
    /// 删除
    /// </summary>
    Delete = 30,
  }
}