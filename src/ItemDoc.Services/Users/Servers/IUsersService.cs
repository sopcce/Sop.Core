using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemDoc.Framework.Repositories;
using ItemDoc.Services.Model;

namespace ItemDoc.Services.Servers
{
  /// <summary>
  /// 
  /// </summary>
  public interface IUsersService
  {
    /// <summary>
    /// Inserts the specified information.
    /// </summary>
    /// <param name="info">The information.</param>
    void Insert(UsersLoginInfo info);
    /// <summary>
    /// Gets the by userid.
    /// </summary>
    /// <param name="primaryKey">The primary key.</param>
    /// <returns></returns>
    UsersLoginInfo GetByUserId(string primaryKey);
    /// <summary>
    /// Logins the specified u name.
    /// </summary>
    /// <param name="uName">Name of the u.</param>
    /// <param name="passWord">The pass word.</param>
    /// <param name="isExsit">if set to <c>true</c> [is exsit].</param>
    /// <returns></returns>
    UsersLoginInfo Login(string uName, string passWord, out bool isExsit);

    /// <summary>
    /// Determines whether [is account exsit] [the specified u name].
    /// </summary>
    /// <param name="uName">Name of the u.</param>
    /// <returns>
    ///   <c>true</c> if [is account exsit] [the specified u name]; otherwise, <c>false</c>.
    /// </returns>
    bool IsAccountExsit(string uName);
  }
}
