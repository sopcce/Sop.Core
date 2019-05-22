using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;

namespace Sop.Common.Helper.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<T> Search<T>(this IQueryable<T> model, string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                var where = model.GetType().GetGenericArguments()[0].GetProperties().Where(item => item.PropertyType == typeof(string)).Aggregate("1!=1 ", (current, item) => current + " or " + item.Name + ".Contains(@0)");

                int intKeyword;
                if (int.TryParse(keyword, out intKeyword))
                {
                    where = model.GetType().GetGenericArguments()[0].GetProperties().Where(item => item.PropertyType == typeof(int)).Aggregate(where, (current, item) => current + " or " + item.Name + "==" + intKeyword);
                }

                decimal decimalKeyword;
                if (decimal.TryParse(keyword, out decimalKeyword))
                {
                    where = model.GetType().GetGenericArguments()[0].GetProperties().Where(item => item.PropertyType == typeof(decimal)).Aggregate(where, (current, item) => current + " or " + item.Name + "==" + decimalKeyword);
                }

                bool boolKeyword;
                if (bool.TryParse(keyword, out boolKeyword))
                {
                    where = model.GetType().GetGenericArguments()[0].GetProperties().Where(item => item.PropertyType == typeof(bool)).Aggregate(where, (current, item) => current + " or " + item.Name + "==" + boolKeyword);
                }

               // model = model.Where(where, keyword);
           
            }

            return model;
        }

        public static IEnumerable<T> Cache<T>(this IQueryable<T> model) where T : class
        {

            var cacheId = model.ToTraceString();

            var item = (IEnumerable<T>)MemoryCache.Default.Get(cacheId);

            if (item == null)
            {
                item = model.ToList();

                var policy = new CacheItemPolicy
                {
                    //该缓存指定时间内未使用 自动移除
                    SlidingExpiration = TimeSpan.FromHours(1)
                };

               // policy.ChangeMonitors.Add(MemoryCache.Default.CreateCacheEntryChangeMonitor(new[] { cacheId }));

                MemoryCache.Default.Add(cacheId, item, policy);

            }
            else
            {
                Trace.WriteLine("从缓存读取：" + cacheId);
            }

            return item;
        }

        private static string ToTraceString<T>(this IQueryable<T> query)
        {
            var internalQueryField = query.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).First(f => f.Name.Equals("_internalQuery"));

            var internalQuery = internalQueryField.GetValue(query);

            var objectQueryField = internalQuery.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).First(f => f.Name.Equals("_objectQuery"));

            var objectQuery = objectQueryField.GetValue(internalQuery) as ObjectQuery<T>;

            return objectQuery.ToTraceStringWithParameters();
        }

        private static string ToTraceStringWithParameters<T>(this ObjectQuery<T> query)
        {
            var traceString = query.ToTraceString() + "\n";

            traceString = query.Parameters.Aggregate(traceString, (current, parameter) => current + ("-- @" + parameter.Name + ": '" + parameter.Value + "' (Type =" + parameter.ParameterType.Name + ")\n"));

            return traceString;
        }

        public static CacheEntryChangeMonitor policyChangeMonitors { get; set; }
    }

    public static class DistinctExtensions
    {
        public static IEnumerable<T> Distinct<T, TV>(this IEnumerable<T> source, Func<T, TV> keySelector)
        {
            return source.Distinct(new CommonEqualityComparer<T, TV>(keySelector));
        }

        public static IEnumerable<T> Distinct<T, TV>(this IEnumerable<T> source, Func<T, TV> keySelector, IEqualityComparer<TV> comparer)
        {
            return source.Distinct(new CommonEqualityComparer<T, TV>(keySelector, comparer));
        }
    }

    // Distinct(p => p.Name, StringComparer.CurrentCultureIgnoreCase)

    public class CommonEqualityComparer<T, TV> : IEqualityComparer<T>
    {
        private readonly Func<T, TV> _keySelector;
        private readonly IEqualityComparer<TV> _comparer;

        public CommonEqualityComparer(Func<T, TV> keySelector, IEqualityComparer<TV> comparer)
        {
            _keySelector = keySelector;
            _comparer = comparer;
        }

        public CommonEqualityComparer(Func<T, TV> keySelector)
            : this(keySelector, EqualityComparer<TV>.Default)
        { }

        public bool Equals(T x, T y)
        {
            return _comparer.Equals(_keySelector(x), _keySelector(y));
        }

        public int GetHashCode(T obj)
        {
            return _comparer.GetHashCode(_keySelector(obj));
        }
    }
}
