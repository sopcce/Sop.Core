using ItemDoc.Services.Auth.Identity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
namespace ItemDoc.Services.Model
{
    /// <summary>
    /// 用户登录实体
    /// </summary>
    public class UsersInfo
    {


        /// <summary>
        /// auto_increment
        /// </summary>		

        public virtual long Id { get; set; }

        public virtual string UserId { get; set; }

        /// <summary>
        /// UserName
        /// </summary>		

        public virtual string UserName { get; set; }
        /// <summary>
        /// UrlToken
        /// </summary>		 
        public virtual string UrlToken { get; set; }

        /// <summary>
        /// Email
        /// </summary>		

        public virtual string Email { get; set; }
        
        /// <summary>
        /// EmailConfirmed
        /// </summary>		

        public virtual bool EmailConfirmed { get; set; }

        /// <summary>
        /// PhoneNumber
        /// </summary>		

        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// PhoneNumberConfirmed
        /// </summary>		

        public virtual bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// AccountIDcard
        /// </summary>		

        public virtual string AccountIDcard { get; set; }



        /// <summary>
        /// PassWord
        /// </summary>		

        public virtual string PassWord { get; set; }

 

        /// <summary>
        /// SecurityStamp
        /// </summary>
        public virtual string SecurityStamp { get; set; }

        /// <summary>
        /// TrueName
        /// </summary>		

        public virtual string TrueName { get; set; }

        /// <summary>
        /// NickName
        /// </summary>		

        public virtual string NickName { get; set; }

        /// <summary>
        /// Status
        /// </summary>		

        public virtual Status Status { get; set; }

        /// <summary>
        /// StatusNotes
        /// </summary>		

        public virtual string StatusNotes { get; set; }

        /// <summary>
        /// CreatedIP
        /// </summary>		

        public virtual string CreatedIP { get; set; }

        /// <summary>
        /// on update CURRENT_TIMESTAMP
        /// </summary>		

        public virtual DateTime ActivityTime { get; set; }

        /// <summary>
        /// LastActivityTime
        /// </summary>		

        public virtual DateTime LastActivityTime { get; set; }

        /// <summary>
        /// ActivityIP
        /// </summary>		

        public virtual string ActivityIP { get; set; }

        /// <summary>
        /// LastActivityIP
        /// </summary>		

        public virtual string LastActivityIP { get; set; }

        /// <summary>
        /// DateCreated
        /// </summary>		

        public virtual DateTime DateCreated { get; set; }

        /// <summary>
        /// DisplayOrder
        /// </summary>		

        public virtual int DisplayOrder { get; set; }

        /// <summary>
        /// LockoutEndDateUtc
        /// </summary>		

        public virtual DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        /// LockoutEnabled
        /// </summary>		

        public virtual bool LockoutEnabled { get; set; }

        /// <summary>
        /// AccessFailedCount
        /// </summary>		

        public virtual int AccessFailedCount { get; set; }

        /// <summary>
        /// TwoFactorEnabled
        /// </summary>		

        public virtual bool TwoFactorEnabled { get; set; }


    }




    public enum PassWordEncryptionType
    {
        /// <summary>
        /// The none
        /// </summary>
        None = 0,
        /// <summary>
        /// The MD5 upper
        /// </summary>
        Md5Upper = 1,
        /// <summary>
        /// The sha256
        /// </summary>
        Sha256 = 2,
        /// <summary>
        /// The sha512
        /// </summary>
        Sha512 = 3,
        /// <summary>
        /// The aes
        /// </summary>
        Aes = 4,

    }
    public enum Status
    {

        /// <summary>
        /// 未激活
        /// </summary>
        NoActivated = 1,

        /// <summary>
        /// 已激活
        /// </summary>
        Activated = 2,

        /// <summary>
        /// 已封禁
        /// </summary>
        Banned = 3
    }



}
