using ItemDoc.Framework.Repositories;
using ItemDoc.Services.Model;

namespace ItemDoc.Services.Repositories
{
  /// <summary>
  /// 
  /// </summary>
  public interface IUsersRepository : IRepository<UsersLoginInfo>
  {
    /// <summary>
    /// Gets the by userid.
    /// </summary>
    /// <param name="primaryKey">The primary key.</param>
    /// <returns></returns>
    UsersLoginInfo GetByUserName(string primaryKey);


    UsersLoginInfo GetByUserId(string primaryKey);
  }


}
