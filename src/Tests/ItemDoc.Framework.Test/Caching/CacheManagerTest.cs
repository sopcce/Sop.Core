using System;
using ItemDoc.Framework.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ItemDoc.Framework.Test.Caching
{
  [TestClass]
    public class CacheManagerTest
    {
        private static ICacheManager _memoryCache;
        private static ICacheManager _redisCache;

        /// <summary>
        /// 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        /// </summary>
        /// <param name="testContext"></param>
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            _memoryCache = new MemoryCacheManager();

            //var con = ConfigurationManager.AppSettings["RedisServerConnectionString"].ToString();
            var con= "127.0.0.1:6379";
            con = "localhost";
            _redisCache = new RedisCacheManager(con);
        }
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        [TestCleanup()]
        public void MyTestCleanup()
        {
            _memoryCache.Clear();
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void RedisCacheManager_Get_Set_IsSet()
        {

            const string keyStirng = "sop-key-001";
            const string valueStirng = "sopcce.com";
            _redisCache.Clear();
            _redisCache.Set(keyStirng, valueStirng, new TimeSpan(0, 0, 10));
            var value = _redisCache.Get<string>(keyStirng);
            Assert.AreEqual(value, valueStirng);

            System.Threading.Thread.Sleep(10 * 1000);

            var isSet = _redisCache.IsSet(keyStirng);

            Assert.AreEqual(isSet, false);
            _redisCache.Remove(keyStirng);
            _redisCache.Set(keyStirng, valueStirng, new TimeSpan(0, 0, 60));
        }


        [TestMethod]
        public void MemoryCacheManager_Get_Set_IsSet()
        {

            const string keyStirng = "sop-key-001";
            const string valueStirng = "sopcce.com";
            _memoryCache.Clear();
            _memoryCache.Set(keyStirng, valueStirng, new TimeSpan(0, 0, 10));
            var value = _memoryCache.Get<string>(keyStirng);
            Assert.AreEqual(value, valueStirng);

            System.Threading.Thread.Sleep(10 * 1000);

            var isSet = _memoryCache.IsSet(keyStirng);

            Assert.AreEqual(isSet, false);
        }

    }
}
