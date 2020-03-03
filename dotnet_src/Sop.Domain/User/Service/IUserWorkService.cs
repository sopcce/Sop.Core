using System;
using System.Linq.Expressions;
using Sop.Data;
using Sop.Domain.Entity;

namespace Sop.Domain.Service
{
    /// <summary>
    /// </summary>
    public interface IUserWorkService
    {
        /// <summary>
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPageList<UserWork> GetPageList(Expression<Func<UserWork, bool>> predicate, int pageIndex = 1,
                                        int pageSize = 10);
    }
}