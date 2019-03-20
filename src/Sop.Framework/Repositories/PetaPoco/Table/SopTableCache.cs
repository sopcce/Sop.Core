using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemDoc.Framework.Repositories
{
  [AttributeUsage(AttributeTargets.Class)]
  public class SopTableCacheAttribute : Attribute
  {
    /// <summary>
    ///     The column name.
    /// </summary>
    /// <returns>
    ///     The column name.
    /// </returns>
    public bool IsCache { get; private set; } = false;


    public TimeSpan CacheTime { get; set; }


    public SopTableCacheAttribute(bool isCache)
    {

      IsCache = isCache;
    }




  }
}
