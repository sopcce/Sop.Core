using System.Collections.Generic;
using ItemDoc.Framework.Utility;

namespace ItemDoc.Core.Extensions
{
    /// <summary>
    /// 对IDictionary的扩展方法
    /// </summary>
    public static class IDictionaryExtensions
    {
        /// <summary>
        ///  尝试将键和值添加到字典中：如果不存在，才添加；存在，不添加也不抛导常
        /// </summary>
        /// <param name="dictionary">字典</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static IDictionary<TKey, TValue> TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (value != null && !dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            return dictionary;
        }

        /// <summary>
        /// 将键和值添加或替换到字典中：如果不存在，则添加；存在，则替换
        /// </summary>
        public static IDictionary<TKey, TValue> AddOrReplace<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            dictionary[key] = value;
            return dictionary;
        }

        /// <summary>
        /// 向字典中批量添加键值对
        /// </summary>
        public static IDictionary<TKey, TValue> AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<TKey, TValue>> values, bool replaceExisted)
        {
            foreach (var item in values)
            {
                if (dictionary.ContainsKey(item.Key) == false || replaceExisted)
                    dictionary[item.Key] = item.Value;
            }
            return dictionary;
        }

        /// <summary>
        /// 依据key获取字典的value，并转换为需要的类型
        /// </summary>
        /// <remarks>
        /// <para>常用于以下集合：</para>
        /// <list type="number">
        /// <item>ViewData</item>
        /// <item>NameValueCollection：HttpRequest.Form、HttpRequest.Request、HttpRequest.Params</item>
        /// </list>
        /// </remarks>
        /// <param name="dictionary">字典集合</param>
        /// <param name="key">key</param>
        /// <param name="defaultValue">如果未找到则返回该默认值</param>
        /// <returns>取得viewdata里的某个值,并且转换成指定的对象类型,如果不是该类型或如果是一个数组类型而元素为0个或没有此key都将返回空,</returns>
        public static T Get<T>(this IDictionary<string, object> dictionary, string key, T defaultValue)
        {
            if (dictionary.ContainsKey(key))
            {
                object value;
                dictionary.TryGetValue(key, out value);

                return ValueUtility.ChangeType<T>(value, defaultValue);
            }
            return defaultValue;
        }

        /// <summary>
        /// 依据key获取字典的value，并转换为需要的类型
        /// </summary>
        /// <remarks>
        /// <para>常用于以下集合：</para>
        /// <list type="number">
        /// <item>ViewData</item>
        /// <item>NameValueCollection：HttpRequest.Form、HttpRequest.Request、HttpRequest.Params</item>
        /// </list>
        /// </remarks>
        /// <param name="dictionary">字典集合</param>
        /// <param name="key">key</param>
        /// <returns>取得viewdata里的某个值,并且转换成指定的对象类型,如果不是该类型或如果是一个数组类型而元素为0个或没有此key都将返回空,</returns>
        public static T Get<T>(this IDictionary<string, object> dictionary, string key)
        {
            if (dictionary.ContainsKey(key))
            {
                object value;
                dictionary.TryGetValue(key, out value);

                return ValueUtility.ChangeType<T>(value);
            }
            return default(T);
        }
    }
}