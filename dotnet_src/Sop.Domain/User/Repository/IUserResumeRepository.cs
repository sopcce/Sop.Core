using System.Collections.Generic;
using Sop.Data;
using Sop.Domain.Entity;

namespace Sop.Domain.Repository
{
    public interface IUserResumeRepository : IRepository<UserResume>
    {
        List<int> GetResumeListByTop();
    }
}