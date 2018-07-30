using ItemDoc.Core.Auth;
using ItemDoc.Framework.Environment;
using ItemDoc.Services.Model;

namespace ItemDoc.Services
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
      return user?.UserId;

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
      return user?.UserName;
    }


    public static UsersInfo GetUsersLoginInfo()
    {
      var user = AuthenticationService().GetCurrentUser();
      return user;
    }





  }
}
