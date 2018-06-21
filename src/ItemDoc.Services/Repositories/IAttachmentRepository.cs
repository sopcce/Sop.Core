using System.Collections.Generic;
using ItemDoc.Framework.Repositories;
using ItemDoc.Services.Model;

namespace ItemDoc.Services.Repositories
{
  /// <summary>
  /// 
  /// </summary>
  public interface IAttachmentRepository : IRepository<AttachmentInfo>
  {
    /// <summary>
    /// Gets the by userid.
    /// </summary>
    /// <param name="primaryKey">The primary key.</param>
    /// <returns></returns>
    AttachmentInfo GetById(string primaryKey);

    IEnumerable<AttachmentInfo> GetByServerId(string itemId);

  }


}
