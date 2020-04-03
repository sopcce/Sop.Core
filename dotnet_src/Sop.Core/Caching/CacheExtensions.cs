using Sop.Framework.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sop.Core.Caching
{
    /// <summary>
    /// 
    /// </summary>
    public static class CacheExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheManager"></param>
        /// <param name="key"></param>
        /// <param name="acquire"></param>
        /// <returns></returns>
        public static T Get<T>(this ICacheManager cacheManager, string key, Func<T> acquire)
        {
            return cacheManager.Get(key, 60, acquire);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheManager"></param>
        /// <param name="key"></param>
        /// <param name="cacheTime"></param>
        /// <param name="acquire"></param>
        /// <returns></returns>
        public static T Get<T>(this ICacheManager cacheManager, string key, int cacheTime, Func<T> acquire)
        {
            if (cacheManager.IsSet(key))
            {
                return cacheManager.Get<T>(key);
            }

            var result = acquire();
            if (cacheTime > 0)
                cacheManager.Set(key, result, cacheTime);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheManager"></param>
        /// <param name="pattern"></param>
        /// <param name="keys"></param>
        public static void RemoveByPattern(this ICacheManager cacheManager, string pattern, IEnumerable<string> keys)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (var key in keys.Where(p => regex.IsMatch(p.ToString())).ToList())
                cacheManager.Remove(key);
        }
    }
}
