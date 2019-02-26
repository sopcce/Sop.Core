using System;
using ItemDoc.Framework.Repositories;

namespace ItemDoc.Services.Model
{
    /// <summary>
    /// 用户登录实体
    /// </summary>
    public class PostInfo
    {
        public virtual int Id { get; set; }

        public virtual int CatalogId { get; set; }

        public virtual string UserId { get; set; }


        public virtual string Title { get; set; }

        public virtual string Description { get; set; }
        public virtual string Content { get; set; }
        public virtual string HtmlContentPath { get; set; }

        public virtual int ViewCount { get; set; }

        public virtual int DisplayOrder { get; set; }
        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        public virtual DateTime DateCreated { get; set; } = DateTime.Now;


        public virtual string CreatedIp { get; set; }

    }





}
