
/*------------------------------------------------
// File Name:sop_task_spider
// File Description:sop_task_spider DataBase Entity
// Author:sopcce.com
// Create Time:2019/05/05 14:51:05
//------------------------------------------------*/

using System;

namespace Sop.Core.Spider.Model
{
    /// <summary>
    /// sop_task_spider  
    /// </summary>
    [Serializable]
    public class TaskSpiderInfo
    {
        ///<Summary>
        /// ID
        ///</Summary>
        public virtual long Id { get; set; }
        ///<Summary>
        /// 域名,eg:baidu.com
        ///</Summary>
        public virtual string Domain { get; set; }
        ///<Summary>
        /// 请求连接
        ///</Summary>
        public virtual string RequestLink { get; set; }
        ///<Summary>
        /// tenant  Id 租户身份证ID,默认0
        ///</Summary>
        public virtual string TenantId { get; set; }
        ///<Summary>
        /// 父类ID
        ///</Summary>
        public virtual long ParentId { get; set; }
        ///<Summary>
        /// 父类列表
        ///</Summary>
        public virtual string ParentIdList { get; set; }
        ///<Summary>
        /// 子类数量
        ///</Summary>
        public virtual int ChildCount { get; set; }
        ///<Summary>
        /// 深度
        ///</Summary>
        public virtual int Depth { get; set; }
        ///<Summary>
        /// 请求类型
        ///</Summary>
        public virtual int RequestType { get; set; }
        ///<Summary>
        /// 是否启动
        ///</Summary>
        public virtual ushort Enabled { get; set; }
        ///<Summary>
        /// IP,多个使用,分割
        ///</Summary>
        public virtual string DomainIp { get; set; }
        ///<Summary>
        /// 页内连接
        ///</Summary>
        public virtual string PageLink { get; set; }
        ///<Summary>
        /// 页内连接数量
        ///</Summary>
        public virtual int PageLinkCount { get; set; }
        ///<Summary>
        /// 创建时间
        ///</Summary>
        public virtual DateTime CreateDate { get; set; }
        ///<Summary>
        /// 页面标题
        ///</Summary>
        public virtual string PageTitle { get; set; }
        ///<Summary>
        /// 备注详细
        ///</Summary>
        public virtual string Description { get; set; }
        ///<Summary>
        /// 页面内链接
        ///</Summary>
        public virtual string PageOnsiteLink { get; set; }
        ///<Summary>
        /// 内链接数量
        ///</Summary>
        public virtual int PageOnsiteCout { get; set; }
        ///<Summary>
        /// 页面外链接
        ///</Summary>
        public virtual string PageOffsiteLink { get; set; }
        ///<Summary>
        /// 外链接数量
        ///</Summary>
        public virtual int PageOffsiteCount { get; set; }
        ///<Summary>
        /// 处理时间
        ///</Summary>
        public virtual DateTime PageHandleTime { get; set; }
        ///<Summary>
        /// 页面内容
        ///</Summary>
        public virtual string PageContent { get; set; }
        ///<Summary>
        /// 页面html内容
        ///</Summary>
        public virtual string PageHtmlContent { get; set; }

    }
}
