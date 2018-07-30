using ItemDoc.Services.Model;

namespace ItemDoc.Core.Auth
{
  /// <summary>
  /// 
  /// </summary>
  public interface IAuthenticationService
  {

    /// <summary>
    /// Signs the in user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="userName">Name of the user.</param>
    /// <param name="rememberPassword">if set to <c>true</c> [remember password].</param>
    void UserSignIn(string userId, string userName, bool rememberPassword);


    /// <summary>
    /// Represents an event that is raised when the sign-out operation is complete.
    /// </summary>
    void UserSignOut();
    /// <summary>
    /// Gets the current user.
    /// </summary>
    /// <returns></returns>
    UsersInfo GetCurrentUser();

    /// <summary>
    /// Gets the current account.
    /// </summary>
    /// <returns></returns>
    UsersInfo GetCurrentAccount();
    /// <summary>
    /// Sets the current user.
    /// </summary>
    /// <param name="user">The user.</param>
    void SetCurrentAccount(UsersInfo user);
  

  }
}
