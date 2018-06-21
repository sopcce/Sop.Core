using System.Collections.Generic;
using ItemDoc.Framework.Repositories;
using ItemDoc.Services.Model;

namespace ItemDoc.Services.Repositories
{
  /// <summary>
  /// 
  /// </summary>
  public class CatalogRepository : PocoRepository<CatalogInfo>, ICatalogRepository
  {
    public CatalogInfo GetById(string primaryKey)
    {
      Sql sql = Sql.Builder
        .Select(SopTable.GetColumnStr<CatalogInfo>())
        .From(SopTable.Instance().GetTableName<CatalogInfo>())
        .Where("Id=@0", primaryKey);
      return Database().SingleOrDefault<CatalogInfo>(sql);
    }


    public IEnumerable<CatalogInfo> GetByItemId(string itemId)
    {
      Sql sql = Sql.Builder
        .Select(SopTable.GetColumnStr<CatalogInfo>())
        .From(SopTable.Instance().GetTableName<CatalogInfo>())
        .Where("itemId=@0", itemId);
      return Database().Query<CatalogInfo>(sql);
    }
  }
}
