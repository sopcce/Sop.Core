using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sop.Data;
using Sop.Domain.Entity;

namespace Sop.Domain.Repository
{
    public interface IUserSkillRepository : IRepository<UserSkill>
    {
        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<UserSkill> GetsByUserId(long userId);


        /// <summary>
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPageList<UserSkill> GetPageList(Expression<Func<UserSkill, bool>> predicate, int pageIndex = 1,
                                         int pageSize = 8);

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="predicate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPageList<UserSkill> GetPageListByUserId(long userId, Expression<Func<UserSkill, bool>> predicate = null,
                                                 int pageIndex = 1,
                                                 int pageSize = 16);
    }
}