using ItemDoc.Framework.Repositories;
using ItemDoc.Services.Model;

namespace ItemDoc.Services.Repositories
{
  /// <summary>
  /// 
  /// </summary>
  public class ItemsRepository : PocoRepository<ItemsInfo>, IItemsRepository
  {
    public ItemsInfo GetById(string primaryKey)
    {
      Sql sql = Sql.Builder
        .Select(SopTable.GetColumnStr<ItemsInfo>())
        .From(SopTable.Instance().GetTableName<ItemsInfo>())
        .Where("Id=@0", primaryKey);
      return Database().SingleOrDefault<ItemsInfo>(sql);
    }
     

  }
}
