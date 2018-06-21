using System.Collections.Generic;
using ItemDoc.Framework.Repositories;
using ItemDoc.Services.Model;

namespace ItemDoc.Services.Repositories
{
  /// <summary>
  /// 
  /// </summary>
  public interface ICatalogRepository : IRepository<CatalogInfo>
  {
    /// <summary>
    /// Gets the by userid.
    /// </summary>
    /// <param name="primaryKey">The primary key.</param>
    /// <returns></returns>
    CatalogInfo GetById(string primaryKey);

    IEnumerable<CatalogInfo> GetByItemId(string itemId);

  }


}
