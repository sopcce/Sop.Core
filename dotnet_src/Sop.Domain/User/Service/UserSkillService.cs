using System;
using System.Linq;
using System.Linq.Expressions;
using Sop.Data;
using Sop.Domain.Entity;
using Sop.Domain.Repository;

namespace Sop.Domain.Service
{
    public class UserSkillService : IUserSkillService
    {
        private readonly IUserSkillRepository _userSkillRepository;
        private readonly IUnitOfWork _unitOfWork;


        public UserSkillService(IUserSkillRepository userSkillRepository,
                                IUnitOfWork unitOfWork)
        {
            _userSkillRepository = userSkillRepository;
            _unitOfWork = unitOfWork;
        }

      
    }
}