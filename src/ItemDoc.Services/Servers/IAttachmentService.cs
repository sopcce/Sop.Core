using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemDoc.Framework.Repositories;
using ItemDoc.Services.Model;
using ItemDoc.Services.ViewModel;

namespace ItemDoc.Services.Servers
{
  public interface IAttachmentService
  {
    /// <summary>
    /// 获取单条数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    AttachmentInfo Get(object id);
    /// <summary>
    /// 获取所有栏目
    /// </summary>
    /// <returns></returns>
    IEnumerable<AttachmentInfo> GetAll();

    //IEnumerable<AttachmentInfo> GetAllbyInfos();


    #region Create&&Update&&Delete
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="info"></param>
    int Create(AttachmentInfo info);
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    int Update(AttachmentInfo info);

    /// <summary>
    /// 删除
    /// </summary> 
    /// <param name="info"></param>
    void Delete(AttachmentInfo info);
 
     
    #endregion

  }
}
