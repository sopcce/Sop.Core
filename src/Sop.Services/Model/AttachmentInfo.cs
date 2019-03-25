﻿using System;
using Sop.Framework.Repositories;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Sop.Services.Model
{
    /// <summary>
    /// 
    /// </summary> 
    public class AttachmentInfo
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
        public virtual AttachmentStatus Status { get; set; }
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

        /// <summary>
        /// 未知
        /// </summary>
        None = 0,
    }



}