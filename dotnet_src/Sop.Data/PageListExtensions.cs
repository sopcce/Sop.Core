using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sop.Data;

namespace System
{
    /// <summary>
    ///     该类包含两个ToPagedList扩展方法，
    ///     用于将泛型IQueryable或泛型IEnumerable对象转换为泛型PagedList对象；
    /// </summary>
    public static class PageListExtensions
    {
        /// <summary>
        ///     根据当前页索引pageIndex及每页记录数pageSize获取要分页的数据对象。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="allItems">包含所有要分页数据的IQueryable对象</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示的记录数</param>
        /// <returns></returns>
        public static PageList<T> ToPagedList<T>(this IQueryable<T> allItems, int pageIndex, int pageSize)
        {
            if (pageIndex < 1)
                pageIndex = 1;
            var itemIndex = (pageIndex - 1) * pageSize;
            var totalItemCount = allItems.Count();
            while (totalItemCount <= itemIndex && pageIndex > 1) itemIndex = (--pageIndex - 1) * pageSize;
            var pageOfItems = allItems.Skip(itemIndex).Take(pageSize);
            return new PageList<T>(pageOfItems, pageIndex, pageSize, totalItemCount);
        }

        /// <summary>
        ///     根据当前页索引pageIndex及每页记录数pageSize获取要分页的数据对象。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="allItems">包含所有要分页数据的IEnumerable对象</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示的记录数</param>
        /// <returns></returns>
        public static PageList<T> ToPagedList<T>(this IEnumerable<T> allItems, int pageIndex, int pageSize)
        {
            return allItems.AsQueryable().ToPagedList(pageIndex, pageSize);
        }


        /// <summary>
        ///     PagedList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="pageIndex">1为起始页</param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<IPageList<T>> ToPagedListAsync<T>(
            this IQueryable<T> query,
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            if (pageIndex < 1)
                pageIndex = 1;
            var itemIndex = (pageIndex - 1) * pageSize;
            var totalItemCount = await query.CountAsync(cancellationToken).ConfigureAwait(false);
            while (totalItemCount <= itemIndex && pageIndex > 1) itemIndex = (--pageIndex - 1) * pageSize;
            var items = await query.Skip(itemIndex)
                                   .Take(pageSize).ToListAsync(cancellationToken).ConfigureAwait(false);

            return new PageList<T>(items, pageIndex, pageSize, totalItemCount);
        }
    }
}