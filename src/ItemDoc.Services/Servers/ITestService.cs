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
  public interface ITestService
    {
    /// <summary>
    /// 获取单条数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    TestInfo Get(object id);
    /// <summary>
    /// 获取所有栏目
    /// </summary>
    /// <returns></returns>
    IEnumerable<TestInfo> GetAll();

    //IEnumerable<TestInfo> GetAllbyInfos();


    #region Create&&Update&&Delete
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="info"></param>
    int Create(TestInfo info);
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    int Update(TestInfo info);

    /// <summary>
    /// 删除
    /// </summary> 
    /// <param name="info"></param>
    void Delete(TestInfo info);
 
     
    #endregion

  }
}
