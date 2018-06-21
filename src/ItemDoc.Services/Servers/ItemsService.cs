using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemDoc.Framework.Environment;
using ItemDoc.Framework.Repositories;
using ItemDoc.Framework.Utilities;
using ItemDoc.Services.Model;
using ItemDoc.Services.Repositories;

namespace ItemDoc.Services.Servers
{
  public class ItemsService : IItemsService
  {
    private readonly IItemsRepository _repository;
    public ItemsService(IItemsRepository repository)
    {
      _repository = repository;

    }


    public IEnumerable<ItemsInfo> GetAll()
    {
      return _repository.Table.AsEnumerable();
    }




    /// <summary>
    /// Gets the by userid.
    /// </summary>
    /// <param name="primaryKey">The primary key.</param>
    /// <returns></returns>
    public ItemsInfo GetById(string primaryKey)
    {

      if (primaryKey == "-1")
      {
        _repository.Execute("truncate table " + SopTable.Instance().GetTableName<ItemsInfo>());
        for (int i = 0; i < 10; i++)
        {
          //再一次验证验证码是否正确
          ItemsInfo info = new ItemsInfo();
          info.UserId = "123";
          info.Description = "我是项目描述：" + i;
          info.Name = "项目名称：" + i;
          info.DateCreated = DateTime.Now;
          info.LastUpdateTime = DateTime.Now;
          info.ChildCount = 0;
          info.Depth = 1;
          info.ParentId = 0;
          info.ParentIdList = "";
          info.Enabled = true;
          this.Insert(info);
        }
        
      }

      return _repository.GetById(primaryKey);
    }



    /// <summary>
    /// Inserts the specified information.
    /// </summary>
    /// <param name="info">The information.</param>
    public void Insert(ItemsInfo info)
    {
      _repository.Insert(info);
      info.DisplayOrder = info.Id;
      this.Update(info);
    }
    /// <summary>
    /// Updates the specified information.
    /// </summary>
    /// <param name="info">The information.</param>
    public void Update(ItemsInfo info)
    {
      _repository.Update(info);
    }

    public void Delete(ItemsInfo info)
    {
      _repository.Delete(info);
    }
  }

}
