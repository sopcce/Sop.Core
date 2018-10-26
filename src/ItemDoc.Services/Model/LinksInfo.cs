using System;
using ItemDoc.Framework.Repositories;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace ItemDoc.Services.Model
{
    /// <summary>
    /// 链接实体
    /// </summary> 
    public class LinksInfo
    {
        public virtual string Id { get; set; }

        /// <summary>
        /// 拥有者ID 
        /// </summary>
        public virtual string OwnerId { get; set; }
        /// <summary>
        /// 链接名字
        /// </summary>
        public virtual string LinkName { get; set; }
        /// <summary>
        ///链接地址
        /// </summary>
        public virtual string LinkUrl { get; set; }
        /// <summary>
        /// 链接title提示文字为空时取linkName
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// 链接属性
        /// </summary>
        public virtual ATarget Target { get; set; }

        /// <summary>
        /// 图片链接地址
        /// </summary>
        public virtual string ImageUrl { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public virtual bool IsEnabled { get; set; }
        /// <summary>
        /// 排序ID，默认与主键相同
        /// </summary>
        public virtual int DisplayOrder { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime DateCreated { get; set; }
        /// <summary>
        /// 最后一次修改时间
        /// </summary>
        public virtual DateTime LastModified { get; set; }


    }
   

    /// <summary>
    /// 打开方式
    /// </summary>
    public enum ATarget
    {
        /// <summary>
        /// 在新窗口中打开
        /// </summary>
        Blank = 0,
        /// <summary>
        /// 默认、在相同的框架中打开
        /// </summary>
        Self = 1,
        /// <summary>
        /// 在父框架集中打开
        /// </summary>
        Parent = 2,
        /// <summary>
        /// 在整个窗口中打开
        /// </summary>
        Top = 3
    }
}
