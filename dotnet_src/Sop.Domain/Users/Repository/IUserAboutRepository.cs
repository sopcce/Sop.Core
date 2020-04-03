using Sop.Data;
using Sop.Domain.Entity;

namespace Sop.Domain.Repository
{
    /// <summary>
    /// </summary>
    public interface IUserAboutRepository : IRepository<UserAbout>
    {
        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserAbout GetsByUserId(long userId);
    }
}