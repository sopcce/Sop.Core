using System.Collections.Generic;

namespace Sop.Data
{
    /// <summary>
    ///     Paged list interface
    /// </summary>
    public interface IPageList<T> : IList<T>
    {
        /// <summary>
        ///     第几页
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        ///     显示条数
        /// </summary>
        int PageSize { get; }

        /// <summary>
        ///     总条数
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        ///     总页数
        /// </summary>
        int TotalPages { get; }

        /// <summary>
        ///     上一页
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        ///     下一页
        /// </summary>
        bool HasNextPage { get; }
    }
}