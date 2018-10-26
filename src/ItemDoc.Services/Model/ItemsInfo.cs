using System;
using ItemDoc.Framework.Repositories;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace ItemDoc.Services.Model
{
    /// <summary>
    /// 用户登录实体
    /// </summary>

    public class ItemsInfo
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }
        public virtual string UserId { get; set; }

        public virtual string PassWord { get; set; }
        /// <summary>
        /// 父id
        /// </summary>
        public virtual int ParentId { get; set; }

        /// <summary>
        /// 所有父级CategoriyId
        /// </summary>
        public virtual string ParentIdList { get; set; }

        /// <summary>
        /// 子栏目数目
        /// </summary>
        public virtual int ChildCount { get; set; }

        /// <summary>
        /// 深度(从0开始)
        /// </summary>
        public virtual int Depth { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public virtual bool Enabled { get; set; }

        public virtual DateTime LastUpdateTime { get; set; } = DateTime.Now;


        public virtual int DisplayOrder { get; set; }
        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        public virtual DateTime DateCreated { get; set; } = DateTime.Now;



    }

   





}
