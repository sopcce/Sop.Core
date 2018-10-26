using System;

namespace ItemDoc.Services.Model
{
    /// <summary>
    /// 更改日志实体
    /// </summary>
    public class ChangeLogInfo
    {
        public virtual int Id { get; set; }
        /// <summary>
        /// 时间事件
        /// </summary>
        public virtual DateTime DataDate { get; set; }
        /// <summary>
        /// 导航标题
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public virtual string Summary { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public virtual string Body { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public virtual bool Selected { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public virtual bool Enabled { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime DateCreated { get; set; }


    }



}
