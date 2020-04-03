using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Sop.Data.Repository;
using Sop.Domain.Entity;

namespace Sop.Domain.Repository
{
    /// <summary>
    ///     /
    /// </summary>
    public class UserResumeRepository : EfCoreRepository<UserResume>, IUserResumeRepository
    {
        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        public UserResumeRepository(DbContext context) : base(context)
        {
        }

        public List<int> GetResumeListByTop()
        {
            throw new NotImplementedException();
        }
    }
}