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
  public interface IItemsService
  {
    /// <summary>
    /// Inserts the specified information.
    /// </summary>
    /// <param name="info">The information.</param>
    void Insert(ItemsInfo info);

    void Update(ItemsInfo info);

    void Delete(ItemsInfo info);
    /// <summary>
    /// Gets the by userid.
    /// </summary>
    /// <param name="primaryKey">The primary key.</param>
    /// <returns></returns>
    ItemsInfo GetById(string primaryKey);

    /// <summary>
    /// Gets all.
    /// </summary>
    /// <returns></returns>
    IEnumerable<ItemsInfo> GetAll();
  }
}
