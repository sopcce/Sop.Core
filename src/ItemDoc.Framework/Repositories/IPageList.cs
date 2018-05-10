using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemDoc.Framework.Repositories
{
  /// <summary>
  /// Paged list interface
  /// </summary>
  public interface IPageList<out T> : IEnumerable<T>, IPageList
  { 
  }
 
  /// <summary>
  /// 
  /// </summary>
  public interface IPageList : IEnumerable
  {
    /// <summary>
    /// 当前页数
    /// </summary>
    int PageIndex { get; set; }

    /// <summary>
    /// 每页显示记录数
    /// </summary>
    int PageSize { get; set; }
    /// <summary>
    /// 总记录数
    /// </summary>
    int TotalCount { get; }
    /// <summary>
    /// 总页数
    /// </summary>
    int TotalPages { get; }


  }
}
