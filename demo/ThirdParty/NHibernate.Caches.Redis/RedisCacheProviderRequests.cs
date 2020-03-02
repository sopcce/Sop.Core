using System;
using System.Collections.Generic;
using NHibernate.Caches.Redis;
using System.Web;
using System.Runtime.Remoting.Messaging;
using StackExchange.Redis;

namespace NHibernate.Caches.Redis
{
  public class RedisCacheProviderRequests : RedisCacheProvider
  {


    protected override RedisCache BuildCache(RedisCacheConfiguration configuration,
      IDictionary<string, string> properties,
      ConnectionMultiplexer connectionMultiplexer,
      RedisCacheProviderOptions options)
    {
      //options.OnException = (e) =>
      //{
      //  if (HttpContext.Current != null)
      //  {
      //    HttpContext.Current.Items[RequestRecoveryRedisCache.SkipNHibernateCacheKey] = true;
      //  }
      //  else
      //  {
      //    CallContext.SetData(RequestRecoveryRedisCache.SkipNHibernateCacheKey, true);
      //  }

      //};
      //var evtArg = new ExceptionEventArgs(configuration.RegionName, RedisCacheMethod.Clear, e);
      //options.OnException(this, evtArg);
      options.OnException(null, new ExceptionEventArgs(configuration.RegionName, RedisCacheMethod.Unknown, new Exception()));

      return new RequestRecoveryRedisCache(configuration, properties, connectionMultiplexer, options);
    }
   
  }
}
