using System.Collections.Generic;
using System.Linq;
using Sop.Data;
using Sop.Domain.Entity;
using Sop.Domain.Repository;
using Sop.Domain.VModel;

namespace Sop.Domain.Service
{
    /// <summary>
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserAboutRepository _userAboutRepository;
        private readonly IUserFeaturesRepository _userFeaturesRepository;
        private readonly IUserSkillRepository _userSkillRepository;
        private readonly IUserProjectRepository _userProjectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserWorkRepository _userWorkRepository;
        

        public UserService(IUserRepository userRepository,
                           IUserAboutRepository userAboutRepository,
                           IUserWorkRepository userWorkRepository,
                           IUserFeaturesRepository userFeaturesRepository,
                           IUserSkillRepository userSkillRepository,
                           IUserProjectRepository userProjectRepository,
                           IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _userAboutRepository = userAboutRepository;
            _userWorkRepository = userWorkRepository;
            _userFeaturesRepository = userFeaturesRepository;
            _userSkillRepository = userSkillRepository;
            _userProjectRepository = userProjectRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetById(long id)
        {
            return _userRepository.GetById(id);
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ReadMeVm GetReadMeVmByUserId(long userId)
        { 
            
          
            var readMeVm = new ReadMeVm
            {
                User = _userRepository.GetById(userId),
                UserAbout = _userAboutRepository.GetsByUserId(userId),
                UserWorks = _userWorkRepository.GetsByUserId(userId),
                UserFeatures = _userFeaturesRepository.GetsByUserId(userId),
                UserSkills= _userSkillRepository.GetPageListByUserId(userId).ToList(),
                UserProjects = _userProjectRepository.GetPageListByUserId(userId).ToList(),
            };


            return readMeVm;
        }
    }
}