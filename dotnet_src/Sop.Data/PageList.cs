using System.Collections.Generic;
using System.Linq;
using Sop.Data;

namespace System
{
    /// <summary>
    ///     Paged list
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    [Serializable]
    public class PageList<T> : List<T>, IPageList<T>
    {
        /// <summary>
        ///     Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PageList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var total = source.Count();
            TotalCount = total;
            TotalPages = total / pageSize;

            if (total % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            AddRange(source.Skip(pageIndex * pageSize).Take(pageSize).AsQueryable());
        }

        /// <summary>
        ///     Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PageList(IList<T> source, int pageIndex, int pageSize)
        {
            TotalCount = source.Count();
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            AddRange(source.Skip(pageIndex * pageSize).Take(pageSize).ToList());
        }

        /// <summary>
        ///     Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="totalCount">Total count</param>
        public PageList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            TotalCount = totalCount;
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            AddRange(source);
        }

        /// <summary>
        /// </summary>
        public int PageIndex { get; private set; }

        /// <summary>
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        /// </summary>
        public int TotalPages { get; private set; }

        /// <summary>
        /// </summary>
        public bool HasPreviousPage => PageIndex > 0;

        /// <summary>
        /// </summary>
        public bool HasNextPage => PageIndex + 1 < TotalPages;
    }
}