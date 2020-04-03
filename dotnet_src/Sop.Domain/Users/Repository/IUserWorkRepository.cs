using System.Collections.Generic;
using Sop.Data;
using Sop.Domain.Entity;

namespace Sop.Domain.Repository
{
    /// <summary>
    /// </summary>
    public interface IUserWorkRepository : IRepository<UserWork>
    {
        List<UserWork> GetsByUserId(long userId);
    }
}