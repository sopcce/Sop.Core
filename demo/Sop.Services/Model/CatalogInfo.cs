using Sop.Common.Treeview;
using System;
using System.ComponentModel.DataAnnotations;

namespace Sop.Services.Model
{
    /// <summary>
    /// 用户登录实体
    /// </summary> 
    public class CatalogInfo : NodeInfo
    {
        public override int Id { get; set; }
        public virtual string UserId { get; set; }
        public virtual int ItemId { get; set; }
        public virtual int TenantId { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public override string Name { get; set; }
        /// <summary>
        /// 栏目描述
        /// </summary>
        public override string Description { get; set; }
        /// <summary>
        /// 父id
        /// </summary>
        public override int ParentId { get; set; }
        /// <summary>
        /// 父id
        /// </summary>
        public override string ParentIdList { get; set; } = "";

        /// <summary>
        /// 子栏目数目
        /// </summary>
        public override int ChildCount { get; set; }

        /// <summary>
        /// 深度
        /// </summary>
        public override int Depth { get; set; } = 0;
        /// <summary>
        /// 是否启用
        /// </summary>
        public override bool Enabled { get; set; } = true;

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
        public override int DisplayOrder { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public override DateTime DateCreated { get; set; } = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        public override string Tags { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public override string Color { get; set; }
        public override string Url { get; set; }

        public override string BackColor { get; set; }

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
