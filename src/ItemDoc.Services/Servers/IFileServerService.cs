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
  public interface IFileServerService
  {
    /// <summary>
    /// 获取单条数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    FileServerInfo Get(object id);
    /// <summary>
    /// 获取所有栏目
    /// </summary>
    /// <returns></returns>
    IEnumerable<FileServerInfo> GetAll();

    //IEnumerable<FileServerInfo> GetAllbyInfos();


    #region Create&&Update&&Delete
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="info"></param>
    int Create(FileServerInfo info);
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    int Update(FileServerInfo info);

    /// <summary>
    /// 删除
    /// </summary> 
    /// <param name="info"></param>
    void Delete(FileServerInfo info);
 
     
    #endregion

  }
}
