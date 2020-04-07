using System;
using Sop.Data;

namespace Sop.Domain.Entity
{
    /// <summary>
    ///     sop_user
    /// </summary>
    [Serializable]
    public class User : IEntity
    {
        /// <Summary>
        ///     ID
        /// </Summary>
        public long Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserId { get;  set; }
        /// <Summary>
        ///     用户名称
        /// </Summary>
        public string UserName { get; set; }

        /// <Summary>
        ///     授权URL
        /// </Summary>
        public string UrlToken { get; set; }

        /// <Summary>
        ///     电子邮箱
        /// </Summary>
        public string Email { get; set; }

        /// <Summary>
        ///     电子邮箱是否认证
        /// </Summary>
        public int EmailConfirmed { get; set; }

        /// <Summary>
        ///     电话号码
        /// </Summary>
        public string MobilePhone { get; set; }

        /// <Summary>
        ///     电话号码是否认证
        /// </Summary>
        public int MobilePhoneConfirmed { get; set; } 
        /// <Summary>
        ///     密码
        /// </Summary>
        public string PassWord { get; set; }

        /// <Summary>
        ///     安全戳
        ///     
        /// </Summary>
        public string SecurityStamp { get; set; }

        /// <Summary>
        ///     真实姓名
        /// </Summary>
        public string TrueName { get; set; }

        /// <Summary>
        ///     用户昵称
        /// </Summary>
        public string NickName { get; set; }

        /// <Summary>
        ///     状态
        /// </Summary>
        public int Status { get; set; }

        /// <Summary>
        ///     状态备注
        /// </Summary>
        public string StatusNotes { get; set; }

        /// <Summary>
        ///     创建IP
        /// </Summary>
        public string CreatedIP { get; set; }

        /// <Summary>
        ///     登录时间
        /// </Summary>
        public DateTime ActivityTime { get; set; }

        /// <Summary>
        ///     最后登录时间
        /// </Summary>
        public DateTime LastActivityTime { get; set; }

        /// <Summary>
        ///     活跃IP
        /// </Summary>
        public string ActivityIP { get; set; }

        /// <Summary>
        ///     最后登录IP
        /// </Summary>
        public string LastActivityIP { get; set; }

        /// <Summary>
        ///     创建时间
        /// </Summary>
        public DateTime DateCreated { get; set; }

        /// <Summary>
        ///     排序ID
        /// </Summary>
        public int DisplayOrder { get; set; }

        /// <Summary>
        ///     锁定时间
        /// </Summary>
        public DateTime LockoutEndDateUtc { get; set; }

        /// <Summary>
        ///     锁定状态
        /// </Summary>
        public int LockoutEnabled { get; set; }

        /// <Summary>
        ///     账号登录失败次数
        /// </Summary>
        public int AccessFailedCount { get; set; }

        /// <Summary>
        ///     二次授权启用
        /// </Summary>
        public int TwoFactorEnabled { get; set; }
       
    }
}