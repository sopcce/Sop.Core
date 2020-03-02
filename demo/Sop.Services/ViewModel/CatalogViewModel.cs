using System;
using Sop.Services.Model;

namespace Sop.Services.ViewModel
{

    public class CatalogViewModel
    {

        public int Id { get; set; }
        public string UserId { get; set; }
        /// <summary>
        /// 租户ID
        /// </summary>
        public  int TenantId { get; set; }
        public int ItemId { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 栏目描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 父id
        /// </summary>
        public int ParentId { get; set; }

        public string ParentName { get; set; }
        /// <summary>
        /// 父id
        /// </summary>
        public string ParentIdList { get; set; }
        /// <summary>
        /// 子栏目数目
        /// </summary>
        public int ChildCount { get; set; }
        /// <summary>
        /// 深度
        /// </summary>
        public int Depth { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 内容计数
        /// </summary>
        public int ContentCount { get; set; }

        /// <summary>
        /// 访问计数类型
        /// </summary>
        public ViewCountTypeStatus ViewCountType { get; set; }
        /// <summary>
        /// 排序id
        /// </summary>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public string Tags { get; set; }

        public string Color { get; set; }
        public string Url { get; set; }

        public string BackColor { get; set; }

    }






}
