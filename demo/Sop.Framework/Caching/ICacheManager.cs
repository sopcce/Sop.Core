using System;

namespace Sop.Framework.Caching
{
    /// <summary>
    /// 缓存管理接口
    /// </summary>
    public interface ICacheManager : IDisposable
  {

    T Get<T>(string key);

    void Set(string key, object value, int cacheTime);


    void Set(string key, object value, TimeSpan timeSpan);


    bool IsSet(string key);

    void Remove(string key);


    void RemoveByPattern(string pattern);

    void Clear();
  }
}
