using ItemDoc.Framework.Repositories;
using ItemDoc.Framework.Utilities;
using ItemDoc.Services.Model;
using ItemDoc.Services.Repositories;
using ItemDoc.Services.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace ItemDoc.Services.Servers
{
  public class PostService : IPostService
  {
    private readonly IPostRepository _repository;
    public PostService(IPostRepository repository)
    {
      _repository = repository;

    }





    /// <summary>
    /// 获取所有栏目
    /// </summary>
    /// <returns></returns>
    public IEnumerable<PostInfo> GetAll()
    {
      //todo:这里栏目使用比较多，但是数据量总体不超过5000条 ，可以考虑加入缓存
      return _repository.Table;
    }
    /// <summary>
    /// 获取单条数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public PostInfo Get(object id)
    {
      return _repository.GetById(id);
    }




    #region Create&&Update&&Delete
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="info"></param>
    public void Create(PostInfo info)
    {
      _repository.Insert(info);
      //todo 不知道会不会出现问题 ，但是当前能够使用

      info.DisplayOrder = info.Id;
      info.Title = info.Title + info.Id;
      _repository.Update(info);
    }
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public void Update(PostInfo info)
    {
      _repository.Update(info);
    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="info"></param>
    public void Delete(PostInfo info)
    {
      if (info == null)
        return;
      _repository.Delete(info);

    }

    public IPageList<PostViewModel> GetPostList(PostParameter parameter)
    {
      //int cataLog, int pageSize, int pageIndex, string keyword, string sortOrder, string sortName 
      //TODO 特殊处理page 数
      parameter.pageIndex = (parameter.pageIndex / parameter.pageSize) + 1;



      return new PageList<PostViewModel>();


      // return _repository.GetPostList(parameter);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public IPageList<PostViewModel> GetPostList1(PostParameter parameter)
    {
      //int cataLog, int pageSize, int pageIndex, string keyword, string sortOrder, string sortName 
      //TODO 特殊处理page 数
      parameter.pageIndex = (parameter.pageIndex / parameter.pageSize) + 1;

      return _repository.GetPostList1(parameter);
    }
    #endregion

  }
}
