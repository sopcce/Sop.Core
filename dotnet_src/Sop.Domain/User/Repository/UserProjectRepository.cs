using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Sop.Data;
using Sop.Data.Repository;
using Sop.Domain.Entity;

namespace Sop.Domain.Repository
{
    public class UserProjectRepository : EfCoreRepository<UserProject>, IUserProjectRepository
    {
        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        public UserProjectRepository(DbContext context) : base(context)
        {
        }


        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPageList<UserProject> GetPageListByUserId(long userId, int pageIndex = 1,
                                                          int pageSize = 8)
        {
            var query = TableNoTracking.Where(n => n.UserId == userId);

            query = query.OrderByDescending(n => n.StartDate);
            var list = query.ToPagedList(pageIndex, pageSize);
            return list;
        }
    }
}