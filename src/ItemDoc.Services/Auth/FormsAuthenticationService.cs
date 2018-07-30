using System;
using System.Text;
using System.Web;
using System.Web.Security;
using ItemDoc.Framework.Environment;
using ItemDoc.Framework.Utilities;
using ItemDoc.Framework.Utility;
using ItemDoc.Services.Model;
using ItemDoc.Services.Servers;

namespace ItemDoc.Core.Auth
{

  /// <summary>
  /// 
  /// </summary>
  /// <seealso cref="ItemDoc.Core.Auth.IAuthenticationService" />
  public class FormsAuthenticationService : IAuthenticationService
  {
    /// <summary>
    /// The current user cookie
    /// </summary>
    public static readonly string CurrentUserCookie = "_CurrentUser";
    /// <summary>
    /// The user information
    /// </summary>
    private UsersInfo _usersLoginInfo;


    /// <summary>
    /// Signs the in user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="userName">Name of the user.</param>
    /// <param name="rememberPassword">if set to <c>true</c> [remember password].</param>
    public void UserSignIn(string userId, string userName, bool rememberPassword)
    {
      FormsAuthentication.SetAuthCookie(userId, rememberPassword);
    }

    /// <summary>
    /// Represents an event that is raised when the sign-out operation is complete.
    /// </summary>
    public void UserSignOut()
    {
      FormsAuthentication.SignOut();
    }
    /// <summary>
    /// Gets the current account.
    /// </summary>
    /// <returns></returns>
    public UsersInfo GetCurrentAccount()
    {
      if (_usersLoginInfo != null)
        return _usersLoginInfo;

      var httpContext = HttpContext.Current;
      if (httpContext == null || httpContext.Request.IsAuthenticated)
        return null;
      //获取用户userid
      string primaryKey = httpContext.User.Identity.Name;
      //根据用户名获取当前登录用户

      IUsersService userService = DiContainer.Resolve<IUsersService>();

      _usersLoginInfo = userService.GetByUserId(primaryKey);
      return _usersLoginInfo;
    }
    /// <summary>
    /// Sets the current user.
    /// </summary>
    /// <param name="user">The user.</param>
    public void SetCurrentAccount(UsersInfo user)
    {
      if (HttpContext.Current == null || HttpContext.Current.Request.IsAuthenticated)
        return;
      _usersLoginInfo = user;
      var cookieValue = EncryptionUtility.AES_Encrypt(user.UserId);

      cookieValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(cookieValue));
      var cookie = new HttpCookie(CurrentUserCookie, cookieValue)
      {
        Expires = DateTime.Now.Add(FormsAuthentication.Timeout),
        HttpOnly = true
      };
      HttpContext.Current.Response.SetCookie(cookie);
    }


    /// <summary>
    /// Gets the current user.
    /// </summary>
    /// <returns></returns>
    public UsersInfo GetCurrentUser()
    {
      if (HttpContext.Current == null || !HttpContext.Current.Request.IsAuthenticated)
        return null;
      var cookie = HttpContext.Current.Request.Cookies.Get(CurrentUserCookie);
      if (cookie == null)
        return null;

      string cookieValue = Encoding.UTF8.GetString(Convert.FromBase64String(cookie.Value));
      cookieValue = EncryptionUtility.AES_Decrypt(cookieValue);
      cookie.Expires = DateTime.Now.Add(FormsAuthentication.Timeout);
      cookie.HttpOnly = true;
      HttpContext.Current.Response.SetCookie(cookie);

      IUsersService userService = DiContainer.Resolve<IUsersService>();

      var account = userService.GetByUserId(cookieValue);
      string theuserid = HttpContext.Current.User.Identity.Name;
      bool isok = !Convert.ToString(account.UserId).Equals(theuserid, StringComparison.OrdinalIgnoreCase);
      if (account == null || isok)
        return null;
      _usersLoginInfo = account;

      return _usersLoginInfo;
    }


  }


}
