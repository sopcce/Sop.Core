using System.Collections.Generic;
using ItemDoc.Framework.Repositories;
using ItemDoc.Services.Model;

namespace ItemDoc.Services.Repositories
{
  /// <summary>
  /// 
  /// </summary>
  public interface IFileServerRepository : IRepository<FileServerInfo>
  {
    /// <summary>
    /// Gets the by userid.
    /// </summary>
    /// <param name="primaryKey">The primary key.</param>
    /// <returns></returns>
    FileServerInfo GetById(string primaryKey);

    IEnumerable<FileServerInfo> GetByServerId(string itemId);

  }


}
