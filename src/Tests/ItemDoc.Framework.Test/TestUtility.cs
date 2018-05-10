using Autofac;
using ItemDoc.Framework.Caching;
using ItemDoc.Framework.Environment;
using ItemDoc.Framework.Repositories;
using System;

namespace ItemDoc.Framework.Test
{
  public static class TestUtility
  {

    private static readonly bool RedisCache = Config.AppSettings<bool>("RedisCaching.Enabled", true);
    private static readonly string RedisServer = Config.AppSettings<string>("RedisServerConnectionString", "127.0.0.1:6379");

    /// <summary>
    /// 清除指定数据表的数据
    /// </summary>
    public static void CleanupTable(string table)
    {
      ////truncate table tb

      IRepository<string> resp = new PocoRepository<string>();


      string deleteSql = "TRUNCATE TABLE " + table;
      resp.Execute(deleteSql, null);
    }



    public static void Init()
    {

      var builder = new ContainerBuilder();

      builder.Register(c => new MemoryCacheManager()).As<ICacheManager>().SingleInstance().PropertiesAutowired();
      if (RedisCache)
      {
        try
        {
          builder.Register(c => new RedisCacheManager(RedisServer)).As<ICacheManager>().SingleInstance().PropertiesAutowired();
        }
        catch (Exception ex)
        {
          //todo: 写入日志文件 

        }
      }
      Autofac.IContainer container = builder.Build();
      DiContainer.RegisterContainer(container);

    }
  }
}
