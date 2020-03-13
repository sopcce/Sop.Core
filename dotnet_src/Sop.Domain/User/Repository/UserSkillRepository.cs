using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Sop.Data;
using Sop.Data.Repository;
using Sop.Domain.Entity;

namespace Sop.Domain.Repository
{
    public class UserSkillRepository : EfCoreRepository<UserSkill>, IUserSkillRepository
    {
        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        public UserSkillRepository(DbContext context) : base(context)
        {
        }


        public List<UserSkill> GetsByUserId(long userId)
        {
            return TableNoTracking.Where(n => n.UserId == userId).ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPageList<UserSkill> GetPageList(Expression<Func<UserSkill, bool>> predicate, int pageIndex = 1,
                                                int pageSize = 8)
        {
            var query = TableNoTracking.Where(predicate);
            var pages = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return new PageList<UserSkill>(pages, pageIndex, pageSize);
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="predicate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPageList<UserSkill> GetPageListByUserId(long userId, Expression<Func<UserSkill, bool>> predicate = null,
                                                        int pageIndex = 1,
                                                        int pageSize = 15)
        {
            var query = TableNoTracking.Where(n => n.UserId == userId);
            if (predicate != null) query = query.Where(predicate);
            query = query.OrderByDescending(n => n.CreateTime);
            var list = query.ToPagedList(pageIndex, pageSize);
            return list;
        }
    }
}