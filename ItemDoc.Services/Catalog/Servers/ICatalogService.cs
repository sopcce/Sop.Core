using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemDoc.Services.Model;

namespace ItemDoc.Services.Servers
{
  public interface ICatalogService
  {
    /// <summary>
    /// 获取单条数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    CatalogInfo Get(object id);

    IEnumerable<CatalogInfo> GetByItemId(string id);
    /// <summary>
    /// 获取所有栏目
    /// </summary>
    /// <returns></returns>
    IEnumerable<CatalogInfo> GetAll(); 
     


    /// <summary>
    /// 获取顶级栏目
    /// </summary>
    /// <returns></returns>
    IEnumerable<CatalogInfo> GetRootCatalogs();
    /// <summary>
    /// 获取所有栏目，排序
    /// </summary>
    /// <returns></returns>
    IEnumerable<CatalogInfo> GetAllByDisplayOrder();

    /// <summary>
    /// 获取子栏目
    /// </summary>
    /// <param name="parentId">parentId</param>
    /// <returns></returns>
    IEnumerable<CatalogInfo> GetChildren(int parentId);
    /// <summary>
    /// 获取子栏目(不包括父栏目自己)
    /// </summary>
    /// <param name="categoryId">栏目ID</param>
    /// <returns></returns>
    IEnumerable<CatalogInfo> GetAllDescendants(int categoryId);
    /// <summary>
    /// 获取子栏目
    /// </summary>
    /// <param name="categoryId">栏目ID</param>
    /// <returns></returns>
    Dictionary<int, string> GetChildrenDictionary(int categoryId = 0);

    /// <summary>
    /// 获取一级栏目
    /// </summary>
    /// <param name="exceptCategoryId">要去除的栏目ID</param>
    /// <returns></returns>
    Dictionary<int, string> GetRootFolderDictionary(int exceptCategoryId = -1);



    /// <summary>
    /// 把fromCategoryId合并到toCategoryId
    /// </summary>
    /// <remarks>
    /// 例如：将栏目fromCategoryId合并到栏目toCategoryId，那么fromCategoryId栏目下的所有子栏目和ContentItem全部归到toCategoryId栏目，同时删除fromCategoryId栏目
    /// </remarks>
    /// <param name="fromCategoryId">被合并的栏目ID</param>
    /// <param name="toCategoryId">合并到的栏目ID</param>
    void Merge(int fromCategoryId, int toCategoryId);


    /// <summary>
    /// 把fromCategoryId移动到toCategoryId，作为toCategoryId的子栏目
    /// </summary>
    /// <remarks>
    /// 例如：将栏目fromCategoryId合并到栏目toCategoryId，那么fromCategoryId栏目下的所有子栏目和ContentItem全部归到toCategoryId栏目，同时删除fromCategoryId栏目
    /// </remarks>
    /// <param name="fromCategoryId">被移动的栏目ID</param>
    /// <param name="toCategoryId">移动到的栏目ID</param>
    void Move(int fromCategoryId, int toCategoryId);


    #region Create&&Update&&Delete
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="info"></param>
    void Create(CatalogInfo info);
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    void Update(CatalogInfo info);

    /// <summary>
    /// 删除
    /// </summary> 
    /// <param name="info"></param>
    void Delete(CatalogInfo info);
    #endregion

  }
}
