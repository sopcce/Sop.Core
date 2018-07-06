using ItemDoc.Framework.Repositories;
using ItemDoc.Framework.Utilities;
using ItemDoc.Services.Model;
using ItemDoc.Services.Repositories;
using ItemDoc.Services.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace ItemDoc.Services.Servers
{
  public class TestService : ITestService
    {
    private readonly IAttachmentRepository _repository;
    public AttachmentService(IAttachmentRepository repository)
    {
      _repository = repository;

    }
    
    /// <summary>
    /// 获取所有栏目
    /// </summary>
    /// <returns></returns>
    public IEnumerable<AttachmentInfo> GetAll()
    {
      //todo:这里栏目使用比较多，但是数据量总体不超过5000条 ，可以考虑加入缓存
      return _repository.Table;
    }
    /// <summary>
    /// 获取单条数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public AttachmentInfo Get(object id)
    {
      return _repository.GetById(id);
    }




    #region Create&&Update&&Delete
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="info"></param>
    public int Create(AttachmentInfo info)
    {
      _repository.Insert(info);
      //todo 不知道会不会出现问题 ，但是当前能够使用
      info.DisplayOrder = info.Id;
      _repository.Update(info);

      return info.Id;
    }
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public int Update(AttachmentInfo info)
    {
      _repository.Update(info);
      return info.Id;
    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="info"></param>
    public void Delete(AttachmentInfo info)
    {
      if (info == null)
        return;
      _repository.Delete(info);

    }

 
    #endregion

  }
}
