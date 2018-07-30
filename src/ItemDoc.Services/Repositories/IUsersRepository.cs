using ItemDoc.Framework.Repositories;
using ItemDoc.Services.Model;

namespace ItemDoc.Services.Repositories
{
  /// <summary>
  /// 
  /// </summary>
  public interface IUsersRepository : IRepository<UsersInfo>
  {
    /// <summary>
    /// Gets the by userid.
    /// </summary>
    /// <param name="userName">The primary key.</param>
    /// <returns></returns>
    UsersInfo GetByUserName(string userName);


    UsersInfo GetByUserId(string primaryKey);
  }


}
