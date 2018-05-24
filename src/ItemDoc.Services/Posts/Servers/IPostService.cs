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
  public interface IPostService
  {
    /// <summary>
    /// 获取单条数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    PostInfo Get(object id);
    /// <summary>
    /// 获取所有栏目
    /// </summary>
    /// <returns></returns>
    IEnumerable<PostInfo> GetAll();

    #region Create&&Update&&Delete
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="info"></param>
    int Create(PostInfo info);
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    void Update(PostInfo info);

    /// <summary>
    /// 删除
    /// </summary> 
    /// <param name="info"></param>
    void Delete(PostInfo info);

    IPageList<PostViewModel> GetPostList(PostParameter parameter);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    //IPageList<PostViewModel> GetPostList1(PostParameter parameter);
    #endregion

  }
}
