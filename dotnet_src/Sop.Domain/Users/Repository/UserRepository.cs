using Microsoft.EntityFrameworkCore;
using Sop.Data;
using Sop.Data.Repository;
using Sop.Domain.Entity;

namespace Sop.Domain.Repository
{
    /// <summary>
    /// </summary>
    public class UserRepository : EfCoreRepository<User>, IUserRepository
    {
        private readonly IUnitOfWork _unitOfWork; 
        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="unitOfWork"></param>
        public UserRepository(DbContext context, IUnitOfWork unitOfWork) : base(context)
        {
            _unitOfWork = unitOfWork;
        }

        public void Demo()
        {
            _unitOfWork.BeginTransaction();
        }

    }
}