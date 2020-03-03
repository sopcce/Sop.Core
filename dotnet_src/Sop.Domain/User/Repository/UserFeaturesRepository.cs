using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Sop.Data.Repository;
using Sop.Domain.Entity;

namespace Sop.Domain.Repository
{
    public class UserFeaturesRepository : EfCoreRepository<UserFeatures>, IUserFeaturesRepository
    {
        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        public UserFeaturesRepository(DbContext context) : base(context)
        {
        }

        public List<UserFeatures> GetsByUserId(long userId)
        {
            return TableNoTracking.Where(n => n.UserId == userId).ToList();
        }
    }
}