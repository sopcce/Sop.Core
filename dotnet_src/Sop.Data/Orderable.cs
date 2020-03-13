using System;
using System.Linq;
using System.Linq.Expressions;

namespace Sop.Data.Repository
{
    /// <summary>
    ///     排序规约类
    /// </summary>
    /// <typeparam name="T">要排序的集合项的类型</typeparam>
    public class OrderTable<T>
    {
        public OrderTable(IQueryable<T> enumerable)
        {
            Queryable = enumerable;
        }

        /// <summary>
        ///     已经被排序的IQueryable集合
        /// </summary>
        public IQueryable<T> Queryable { get; private set; }

        /// <summary>
        ///     升序排序
        /// </summary>
        /// <typeparam name="TKey">排序字段</typeparam>
        /// <param name="keySelector">返回TKey的委托</param>
        /// <returns> </returns>
        public OrderTable<T> Asc<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            Queryable = Queryable
               .OrderBy(keySelector);
            return this;
        }

        /// <summary>
        ///     升序排序
        /// </summary>
        /// <typeparam name="TKey1">排序字段1</typeparam>
        /// <typeparam name="TKey2">排序字段2</typeparam>
        /// <param name="keySelector1">返回TKey1的委托</param>
        /// <param name="keySelector2">返回TKey2的委托</param>
        /// <returns> </returns>
        public OrderTable<T> Asc<TKey1, TKey2>(Expression<Func<T, TKey1>> keySelector1,
                                               Expression<Func<T, TKey2>> keySelector2)
        {
            Queryable = Queryable
                       .OrderBy(keySelector1)
                       .ThenBy(keySelector2);
            return this;
        }

        /// <summary>
        ///     升序排序
        /// </summary>
        /// <typeparam name="TKey1">排序字段1</typeparam>
        /// <typeparam name="TKey2">排序字段2</typeparam>
        /// <typeparam name="TKey3">排序字段3</typeparam>
        /// <param name="keySelector1">返回TKey1的委托</param>
        /// <param name="keySelector2">返回TKey2的委托</param>
        /// <param name="keySelector3">返回TKey3的委托</param>
        /// <returns> </returns>
        public OrderTable<T> Asc<TKey1, TKey2, TKey3>(Expression<Func<T, TKey1>> keySelector1,
                                                      Expression<Func<T, TKey2>> keySelector2,
                                                      Expression<Func<T, TKey3>> keySelector3)
        {
            Queryable = Queryable
                       .OrderBy(keySelector1)
                       .ThenBy(keySelector2)
                       .ThenBy(keySelector3);
            return this;
        }

        /// <summary>
        ///     降序排序
        /// </summary>
        /// <typeparam name="TKey">排序字段</typeparam>
        /// <param name="keySelector">返回TKey的委托</param>
        /// <returns></returns>
        public OrderTable<T> Desc<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            Queryable = Queryable
               .OrderByDescending(keySelector);
            return this;
        }

        /// <summary>
        ///     降序排序
        /// </summary>
        /// <typeparam name="TKey1">排序字段1</typeparam>
        /// <typeparam name="TKey2">排序字段2</typeparam>
        /// <param name="keySelector1">返回TKey1的委托</param>
        /// <param name="keySelector2">返回TKey2的委托</param>
        /// <returns> </returns>
        public OrderTable<T> Desc<TKey1, TKey2>(Expression<Func<T, TKey1>> keySelector1,
                                                Expression<Func<T, TKey2>> keySelector2)
        {
            Queryable = Queryable
                       .OrderByDescending(keySelector1)
                       .ThenByDescending(keySelector2);
            return this;
        }

        /// <summary>
        ///     降序排序
        /// </summary>
        /// <typeparam name="TKey1">排序字段1</typeparam>
        /// <typeparam name="TKey2">排序字段2</typeparam>
        /// <typeparam name="TKey3">排序字段3</typeparam>
        /// <param name="keySelector1">返回TKey1的委托</param>
        /// <param name="keySelector2">返回TKey2的委托</param>
        /// <param name="keySelector3">返回TKey3的委托</param>
        /// <returns> </returns>
        public OrderTable<T> Desc<TKey1, TKey2, TKey3>(Expression<Func<T, TKey1>> keySelector1,
                                                       Expression<Func<T, TKey2>> keySelector2,
                                                       Expression<Func<T, TKey3>> keySelector3)
        {
            Queryable = Queryable
                       .OrderByDescending(keySelector1)
                       .ThenByDescending(keySelector2)
                       .ThenByDescending(keySelector3);
            return this;
        }
    }
}