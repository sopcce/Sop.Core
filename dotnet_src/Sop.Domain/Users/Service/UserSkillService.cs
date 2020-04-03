using Sop.Data;
using Sop.Domain.Repository;

namespace Sop.Domain.Service
{
    public class UserSkillService : IUserSkillService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserSkillRepository _userSkillRepository;


        public UserSkillService(IUserSkillRepository userSkillRepository,
                                IUnitOfWork unitOfWork)
        {
            _userSkillRepository = userSkillRepository;
            _unitOfWork = unitOfWork;
        }
    }
}