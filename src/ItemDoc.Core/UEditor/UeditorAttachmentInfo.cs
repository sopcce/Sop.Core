using ItemDoc.Framework.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ItemDoc.Core.UEditor
{
    /// <summary>
    /// 
    /// </summary>
    //[TableName("Sop_UeditorAttachment")]
    //[PrimaryKey("Id",AutoIncrement = true)]
    public class UeditorAttachmentInfo
    {


       

        /// <summary>
        /// 附件ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// UE内容ID
        /// </summary>
        public string UEContentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OwnerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SavePath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LocalPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Extension { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Size { get; set; }
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
        public string MediaType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public AttachmentStatus Status { get; set; }
  

        /// <summary>
        /// 
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime DateCreated { get; set; }


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