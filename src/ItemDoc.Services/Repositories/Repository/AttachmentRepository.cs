using System.Collections.Generic;
using ItemDoc.Framework.Repositories;
using ItemDoc.Services.Model;

namespace ItemDoc.Services.Repositories
{
  /// <summary>
  /// 
  /// </summary>
  public class AttachmentRepository : PocoRepository<AttachmentInfo>, IAttachmentRepository
  {
    public AttachmentInfo GetById(string primaryKey)
    {
      Sql sql = Sql.Builder
        .Select(SopTable.GetColumnStr<AttachmentInfo>())
        .From(SopTable.Instance().GetTableName<AttachmentInfo>())
        .Where("Id=@0", primaryKey);
      return Database().SingleOrDefault<AttachmentInfo>(sql);
    }


    public IEnumerable<AttachmentInfo> GetByServerId(string itemId)
    {
      Sql sql = Sql.Builder
        .Select(SopTable.GetColumnStr<AttachmentInfo>())
        .From(SopTable.Instance().GetTableName<AttachmentInfo>())
        .Where("itemId=@0", itemId);
      return Database().Query<AttachmentInfo>(sql);
    }
  }
}
