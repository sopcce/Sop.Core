using System;
using Sop.Framework.Repositories;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Sop.Services.Model
{
    /// <summary>
    /// 用户登录实体
    /// </summary> 
    public abstract class ArticleInfo
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }
        public virtual string UserId { get; set; }

        public virtual string PassWord { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public virtual DateTime LastUpdateTime { get; set; } = DateTime.Now;



        public virtual int DisplayOrder { get; set; }
        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        public virtual DateTime DateCreated { get; set; } = DateTime.Now;
    }


  

}
