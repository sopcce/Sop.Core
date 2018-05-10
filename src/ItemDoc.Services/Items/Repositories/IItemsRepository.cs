using ItemDoc.Framework.Repositories;
using ItemDoc.Services.Model;

namespace ItemDoc.Services.Repositories
{
  /// <summary>
  /// 
  /// </summary>
  public interface IItemsRepository : IRepository<ItemsInfo>
  {
    /// <summary>
    /// Gets the by userid.
    /// </summary>
    /// <param name="primaryKey">The primary key.</param>
    /// <returns></returns>
    ItemsInfo GetById(string primaryKey);

    
  
  }


}
