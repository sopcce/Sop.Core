using Sop.Data;
using Sop.Domain.Repository;

namespace Sop.Domain.Service
{
    public class UserResumeService : IUserResumeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IUserResumeRepository _userResumeRepository;

        public UserResumeService(IUserRepository userRepository,
                                 IUserResumeRepository userResumeRepository,
                                 IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _userResumeRepository = userResumeRepository;
            _unitOfWork = unitOfWork;
        }
    }
}