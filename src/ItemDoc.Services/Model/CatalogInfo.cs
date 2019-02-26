using System;
using System.ComponentModel.DataAnnotations;
using ItemDoc.Framework.Repositories;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace ItemDoc.Services.Model
{
    /// <summary>
    /// 用户登录实体
    /// </summary> 
    public class CatalogInfo
    {



        public virtual int Id { get; set; }
        public virtual string UserId { get; set; }
        public virtual int ItemId { get; set; }
        public virtual int TenantId { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 栏目描述
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// 父id
        /// </summary>
        public virtual int ParentId { get; set; }
        /// <summary>
        /// 父id
        /// </summary>
        public virtual string ParentIdList { get; set; } = "";

        /// <summary>
        /// 子栏目数目
        /// </summary>
        public virtual int ChildCount { get; set; }

        /// <summary>
        /// 深度
        /// </summary>
        public virtual int Depth { get; set; } = 0;
        /// <summary>
        /// 是否启用
        /// </summary>
        public virtual bool Enabled { get; set; } = true;

        /// <summary>
        /// 内容计数
        /// </summary>
        public virtual int ContentCount { get; set; } = 0;

        /// <summary>
        /// 访问计数类型
        /// </summary>
        public virtual ViewCountTypeStatus ViewCountType { get; set; }
        /// <summary>
        /// 排序id
        /// </summary>
        public virtual int DisplayOrder { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime DateCreated { get; set; } = DateTime.Now;

        public virtual string Tags { get; set; }

        public virtual string Color { get; set; }
        public virtual string Url { get; set; }

        public virtual string BackColor { get; set; }

    }
    public enum ViewCountTypeStatus
    {
        /// <summary>
        /// 每次访问记录
        /// </summary>
        [Display(Name = "每次访问")]
        EveryView = 0,
        /// <summary>
        /// 每天访问记录1次
        /// </summary>、
        [Display(Name = "每天访问")]
        EveryDayView = 1,

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "访问一次")]
        EveryOneView = 2,
    }


   

}
