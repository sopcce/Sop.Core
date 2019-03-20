using Sop.Framework.Repositories;
using Sop.Services.Model;
using System.Collections.Generic;

namespace Sop.Services.Servers
{
  public class FileServerService  
  {
    public IRepository<FileServerInfo> _fileRepository { get; set; }

    /// <summary>
    /// 获取所有栏目
    /// </summary>
    /// <returns></returns>
    public IEnumerable<FileServerInfo> GetAll()
    { 
      return _fileRepository.Table;
    }
    /// <summary>
    /// 获取单条数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public FileServerInfo Get(object id)
    {
      return _fileRepository.Get(id);
    }




    #region Create&&Update&&Delete
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="info"></param>
    public int Create(FileServerInfo info)
    {
      _fileRepository.Create(info); 
      info.DisplayOrder = info.Id;
      _fileRepository.Update(info);

      return info.Id;
    }
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public int Update(FileServerInfo info)
    {
      _fileRepository.Update(info);
      return info.Id;
    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="info"></param>
    public void Delete(FileServerInfo info)
    {
      if (info == null)
        return;
      _fileRepository.Delete(info);

    }

 
    #endregion

  }
}
