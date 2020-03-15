using Microsoft.EntityFrameworkCore;
using Sop.Data.Repository;
using Sop.Domain.Entity;

namespace Sop.Domain.Repository
{
    /// <summary>
    /// </summary>
    public class UserRepository : EfCoreRepository<User>, IUserRepository
    {
        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        public UserRepository(DbContext context) : base(context)
        {
        }
    }
}