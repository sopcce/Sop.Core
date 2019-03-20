using Sop.Services.Auth.Identity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using Sop.Services.Model;

namespace Sop.Services.Auth.Model
{
    public sealed class User : UsersInfo, IUser<long>
    {
        private string _userName;

        #region Pr
        public User()
        {


            this.Roles = new List<string>();
            this.Claims = new List<UserClaim>();
            this.Logins = new List<UserLoginInfo>();
        }

        public User(string userName)
            : this()
        {
            this._userName = userName;
        }

        public User(string userId, string userName) : this()
        {

            this._userName = userName;
        }
        public IList<string> Roles { get; set; }

        public IList<UserClaim> Claims { get; set; }

        public IList<UserLoginInfo> Logins { get; set; }

        public override long Id { get; set; }

        public override string UserName { get; set; }



        #endregion

    }

}
