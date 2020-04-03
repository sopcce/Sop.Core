using System.Collections.Generic;
using Sop.Data;
using Sop.Domain.Entity;

namespace Sop.Domain.Repository
{
    public interface IUserFeaturesRepository : IRepository<UserFeatures>
    {
        List<UserFeatures> GetsByUserId(long userId);
    }
}