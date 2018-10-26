using ItemDoc.Services.Auth.Identity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
namespace ItemDoc.Services.Model
{
    /// <summary>
    /// 用户登录实体
    /// </summary>
    public class UsersExternalLoginInfo
    { 
        public virtual int Id { get; set; }

        public virtual long UserId { get; set; }

        public virtual string LoginProvider { get; set; }

        public virtual string ProviderKey { get; set; }
         
        public virtual DateTime DateCreated { get; set; }

    }




   
    



}
