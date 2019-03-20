using System;
using System.Security.Claims;

using Sop.Framework.Caching;
using Sop.Framework.Environment;
using Sop.Services.Model;
using Sop.Services.Servers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Sop.Services.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public class OwinAuthenticationService : IAuthenticationService
    {
        public static readonly string CurrentUserCookie = "_CurrentUser";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                var ii = HttpContext.Current.GetOwinContext().Authentication;

                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }

        public void UserSignIn(string userId, string userName, bool rememberPassword)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.NameIdentifier, ClaimTypes.Role);
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, userName));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "User"));

            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, claimsIdentity);


            var usersService = DiContainer.Resolve<UsersService>();
            var loginInfo = usersService.GetByUserId(userId);
            var cacheManager = DiContainer.Resolve<ICacheManager>();

            string key = $"{CurrentUserCookie}:{userId}";
            if (!cacheManager.IsSet(key))
            {
                cacheManager.Remove(key);
            }
            cacheManager.Set(key, loginInfo, TimeSpan.FromDays(7));

            //
            var aaa = AuthenticationManager.User.Identity.GetUserId();
            var asdaa = cacheManager.Get<UsersInfo>(key);

        }
        public UsersInfo GetCurrentUser()
        {
            string userId = AuthenticationManager.User.Identity.GetUserId();
            if (!long.TryParse(userId, out long uId))
            {
                return null;
            } 
            
            var cacheManager = DiContainer.Resolve<ICacheManager>();
            string key = $"{CurrentUserCookie}:{userId}";
            var info = cacheManager.Get<UsersInfo>(key);

            if (info == null)
            {
                var usersService = DiContainer.Resolve<UsersService>();
                var loginInfo = usersService.Get(uId);
                if (loginInfo != null)
                {
                    return loginInfo;
                }
                return new UsersInfo() { UserId = userId };
            }
            return info;
        }

        public void UserSignOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }




















        public UsersInfo GetCurrentAccount()
        {
            throw new NotImplementedException();
        }

        public void SetCurrentAccount(UsersInfo user)
        {
            throw new NotImplementedException();
        }


    }
}
