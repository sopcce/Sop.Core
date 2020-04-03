using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Sop.Data.Repository;
using Sop.Domain.Entity;

namespace Sop.Domain.Repository
{
    public class UserWorkRepository : EfCoreRepository<UserWork>, IUserWorkRepository
    {
        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        public UserWorkRepository(DbContext context) : base(context)
        {
        }

        public List<UserWork> GetsByUserId(long userId)
        {
            return TableNoTracking.Where(n => n.UserId == userId).ToList();
        }
    }
}