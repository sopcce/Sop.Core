using System;
using System.Linq;
using System.Linq.Expressions;
using Sop.Data;
using Sop.Domain.Entity;
using Sop.Domain.Repository;

namespace Sop.Domain.Service
{
    /// <summary>
    /// </summary>
    public class UserWorkService : IUserWorkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserWorkRepository _userWorkRepository;

        public UserWorkService(IUserWorkRepository userWorkRepository, IUnitOfWork unitOfWork)
        {
            _userWorkRepository = userWorkRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPageList<UserWork> GetPageList(Expression<Func<UserWork, bool>> predicate, int pageIndex = 1,
                                               int pageSize = 10)
        {
            var query = _userWorkRepository.TableNoTracking.Where(predicate);
            var userWorks = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return new PageList<UserWork>(userWorks, pageIndex, pageSize);
        }
    }
}