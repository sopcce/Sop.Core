using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace ItemDoc.Framework.Caching
{
  /// <summary>
  /// Represents a manager for caching between HTTP requests (long term caching)
  /// </summary>
  public partial class WebCacheManager : ICacheManager
  {

    protected System.Web.Caching.Cache Cache = System.Web.HttpRuntime.Cache;

    public virtual T Get<T>(string key)
    {
      return (T)Cache[key];
    }


    public virtual void Set(string key, object data, int cacheTime)
    {
      if (data == null)
        return;
      var policy = new CacheItemPolicy()
      {
        AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime)
      };
      Cache.Insert(key, data);
    }

    public virtual void Set(string key, object value, TimeSpan timeSpan)
    {
      if (value == null)
        return;
      Cache.Insert(key, value, null, DateTime.Now, timeSpan);
    }



    public virtual bool IsSet(string key)
    {
      return false;//(Cache.Get(key)! = null);
    }


    public virtual void Remove(string key)
    {
      Cache.Remove(key);
    }


    public virtual void RemoveByPattern(string pattern)
    {
      // this.RemoveByPattern(pattern, Cache.Get(pattern));
    }


    public virtual void Clear()
    {

      var CacheEnum = Cache.GetEnumerator();
      while (CacheEnum.MoveNext())
      {
        Cache.Remove(CacheEnum.Key.ToString());
      }
    }


    public virtual void Dispose()
    {
    }
  }
}
