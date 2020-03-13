using Sop.Data;
using Sop.Domain.Entity;

namespace Sop.Domain.Repository
{
    public interface IUserProjectRepository : IRepository<UserProject>
    {
        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPageList<UserProject> GetPageListByUserId(long userId, int pageIndex = 1, int pageSize = 8);
    }
}