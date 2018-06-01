using ItemDoc.Core.Auth;
using ItemDoc.Framework.Environment;
using ItemDoc.Services.Model;
using Microsoft.AspNet.Identity;
using System.Web;
using ItemDoc.Framework.Caching;


namespace ItemDoc.Core
{
  /// <summary>
  /// 用户上下文
  /// </summary>
  public class UserContext
  {
    public static readonly string AdminUserCookie = "__AdminUser";

    private static IAuthenticationService AuthenticationService()
    {
      return DiContainer.ResolvePerHttpRequest<IAuthenticationService>();
    }

    /// <summary>
    /// Gets the get user identifier.
    /// </summary>
    /// <returns></returns>
    public static string GetGetUserId()
    {

      var user = AuthenticationService().GetCurrentUser();
      if (user == null)
        return null;
      return user.UserId;

    } 
    /// <summary>
    /// Gets the name of the get user.
    /// </summary>
    /// <returns>
    /// The name of the get user.
    /// </returns>
    public static string GetUserName()
    {
      var user = AuthenticationService().GetCurrentUser();
      if (user == null)
        return null;
      return user.UserName;
    }


    public static UsersLoginInfo GetUsersLoginInfo()
    {
      var user = AuthenticationService().GetCurrentUser();
      if (user == null)
        return null;
      return user;
    }





  }
}
