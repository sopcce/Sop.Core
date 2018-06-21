using System.Collections.Generic;
using ItemDoc.Framework.Repositories;
using ItemDoc.Services.Model;

namespace ItemDoc.Services.Repositories.Repository
{
  /// <summary>
  /// 
  /// </summary>
  public class FileServerRepository : PocoRepository<FileServerInfo>, IFileServerRepository
  {
    public FileServerInfo GetById(string primaryKey)
    {
      Sql sql = Sql.Builder
        .Select(SopTable.GetColumnStr<FileServerInfo>())
        .From(SopTable.Instance().GetTableName<FileServerInfo>())
        .Where("Id=@0", primaryKey);
      return Database().SingleOrDefault<FileServerInfo>(sql);
    }


    public IEnumerable<FileServerInfo> GetByServerId(string itemId)
    {
      Sql sql = Sql.Builder
        .Select(SopTable.GetColumnStr<FileServerInfo>())
        .From(SopTable.Instance().GetTableName<FileServerInfo>())
        .Where("itemId=@0", itemId);
      return Database().Query<FileServerInfo>(sql);
    }
  }
}
