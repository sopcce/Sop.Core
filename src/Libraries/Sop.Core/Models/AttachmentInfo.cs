using System;
using Sop.Core.Enum;

namespace Sop.Core.Models
{
    /// <summary>
    /// 
    /// </summary> 
    public abstract class AttachmentInfo
    {

        /// <summary>
        /// 附件ID
        /// </summary>
        public virtual int Id { get; set; }
        /// <summary>
        /// UE内容ID
        /// </summary>
        public virtual string AttachmentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string OwnerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string ServerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string ServerUrlPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string Path { get; set; }
        /// <summary>
        /// 
        /// </summary>        
        public virtual string Extension { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual int Size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string FileNames { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string UploadFileName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string MimeType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual AttachmentStatusEnum Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual int DisplayOrder { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Ip { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime DateCreated { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual bool HasThumbnail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual bool HtmlPreview { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual int PageCount { get; set; }


    } 
}
