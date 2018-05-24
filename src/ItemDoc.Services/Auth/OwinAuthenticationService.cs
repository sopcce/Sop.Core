using ItemDoc.Framework.Caching;
using ItemDoc.Framework.Environment;
using ItemDoc.Services.Model;
using ItemDoc.Services.Servers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace ItemDoc.Core.Auth
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


      var usersService = DiContainer.Resolve<IUsersService>();
      var loginInfo = usersService.GetByUserId(userId);
      var cacheManager = DiContainer.Resolve<ICacheManager>();

      string key = string.Format("{0}:{1}", CurrentUserCookie, userId);
      if (!cacheManager.IsSet(key))
      {
        cacheManager.Remove(key);
      }
      cacheManager.Set(key, loginInfo, TimeSpan.FromDays(7));

      //
      var aaa = AuthenticationManager.User.Identity.GetUserId();
      var asdaa = cacheManager.Get<UsersLoginInfo>(key);

    }
    public UsersLoginInfo GetCurrentUser()
    {
      string userId = AuthenticationManager.User.Identity.GetUserId();
      if (string.IsNullOrWhiteSpace(userId))
      {
        return null;
      }
      var cacheManager = DiContainer.Resolve<ICacheManager>();
      string key = string.Format("{0}:{1}", CurrentUserCookie, userId);
      var info = cacheManager.Get<UsersLoginInfo>(key);

      if (info == null)
      {
        var usersService = DiContainer.Resolve<IUsersService>();
        var loginInfo = usersService.GetByUserId(userId);
        if (loginInfo != null)
        {
          return loginInfo;
        }
        return new UsersLoginInfo() { UserId = userId };
      }
      return info;
    }

    public void UserSignOut()
    {
      AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
    }




















    public UsersLoginInfo GetCurrentAccount()
    {
      throw new NotImplementedException();
    }

    public void SetCurrentAccount(UsersLoginInfo user)
    {
      throw new NotImplementedException();
    }


  }
}
