using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Web;
using NHibernate.Caches.Redis;
using StackExchange.Redis;

namespace NHibernate.Caches.Redis
{
  /// <summary>
  /// Allow NHibernate not to continue to timeout for every operation when Redis server is unavailable
  /// https://github.com/TheCloudlessSky/NHibernate.Caches.Redis
  /// </summary>
  public class RequestRecoveryRedisCache : RedisCache
  {
    public const string SkipNHibernateCacheKey = "__SkipNHibernateCache__";

    //public RequestRecoveryRedisCache(string regionName, IDictionary<string, string> properties, RedisCacheElement element, ConnectionMultiplexer connectionMultiplexer, RedisCacheProviderOptions options)
    //    : base(regionName, properties, element, connectionMultiplexer, options)
    //{

    //}

    public RequestRecoveryRedisCache(RedisCacheConfiguration configuration,
      IDictionary<string, string> properties,
      ConnectionMultiplexer connectionMultiplexer,
      RedisCacheProviderOptions options)
      : base(configuration, connectionMultiplexer, options)
    {
      if (HttpContext.Current != null)
      {
        HttpContext.Current.Items[RequestRecoveryRedisCache.SkipNHibernateCacheKey] = true;
      }
      else
      {
        CallContext.SetData(RequestRecoveryRedisCache.SkipNHibernateCacheKey, true);
      }

    }




    public override object Get(object key)
    {
      if (HasFailedForThisHttpRequest()) return null;
      return base.Get(key);
    }

    public override void Put(object key, object value)
    {
      if (HasFailedForThisHttpRequest()) return;
      base.Put(key, value);
    }

    public override void Remove(object key)
    {
      if (HasFailedForThisHttpRequest()) return;
      base.Remove(key);
    }

    public override void Clear()
    {
      if (HasFailedForThisHttpRequest()) return;
      base.Clear();
    }

    public override void Destroy()
    {
      if (HasFailedForThisHttpRequest()) return;
      base.Destroy();
    }

    public override void Lock(object key)
    {
      if (HasFailedForThisHttpRequest()) return;
      base.Lock(key);
    }

    public override void Unlock(object key)
    {
      if (HasFailedForThisHttpRequest()) return;
      base.Unlock(key);
    }

    private bool HasFailedForThisHttpRequest()
    {
      if (HttpContext.Current != null)
      {
        return HttpContext.Current.Items.Contains(SkipNHibernateCacheKey);
      }
      else
      {
        return CallContext.GetData(SkipNHibernateCacheKey) != null;
      }




    }
  }

}
