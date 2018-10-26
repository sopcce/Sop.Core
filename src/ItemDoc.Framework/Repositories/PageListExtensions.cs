using System.Collections.Generic;
using System.Linq;

namespace ItemDoc.Framework.Repositories
{
  /// <summary>
  /// 该类包含两个ToPagedList扩展方法，
  /// 用于将泛型IQueryable或泛型IEnumerable对象转换为泛型PagedList对象；
  /// </summary>
  public static class PageListExtensions
  {

    /// <summary>
    /// 根据当前页索引pageIndex及每页记录数pageSize获取要分页的数据对象。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="allItems">包含所有要分页数据的IQueryable对象</param>
    /// <param name="pageIndex">当前页索引</param>
    /// <param name="pageSize">每页显示的记录数</param>
    /// <returns></returns>
    public static PageList<T> ToPagedList<T>(this IQueryable<T> allItems, int pageIndex, int pageSize)
    {
      if (pageIndex < 1)
        pageIndex = 1;
      var itemIndex = (pageIndex - 1) * pageSize;
      var totalItemCount = allItems.Count();
      while (totalItemCount <= itemIndex && pageIndex > 1)
      {
        itemIndex = (--pageIndex - 1) * pageSize;
      }
      var pageOfItems = allItems.Skip(itemIndex).Take(pageSize);
      return new PageList<T>(pageOfItems, pageIndex, pageSize, totalItemCount);
    }
    /// <summary>
    /// 根据当前页索引pageIndex及每页记录数pageSize获取要分页的数据对象。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="allItems">包含所有要分页数据的IEnumerable对象</param>
    /// <param name="pageIndex">当前页索引</param>
    /// <param name="pageSize">每页显示的记录数</param>
    /// <returns></returns>
    public static PageList<T> ToPagedList<T>(this IEnumerable<T> allItems, int pageIndex, int pageSize)
    {
      return allItems.AsQueryable().ToPagedList(pageIndex, pageSize);
    }

 
   


  }
}
