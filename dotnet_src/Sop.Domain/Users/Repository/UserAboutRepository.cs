using System.Linq;
using Microsoft.EntityFrameworkCore;
using Sop.Data.Repository;
using Sop.Domain.Entity;

namespace Sop.Domain.Repository
{
    /// <summary>
    /// </summary>
    public class UserAboutRepository : EfCoreRepository<UserAbout>, IUserAboutRepository
    {
        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        public UserAboutRepository(DbContext context) : base(context)
        {
        }


        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserAbout GetsByUserId(long userId)
        {
            return TableNoTracking.FirstOrDefault(n => n.UserId == userId);
        }
    }
}