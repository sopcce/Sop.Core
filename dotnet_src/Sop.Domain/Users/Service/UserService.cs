using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IUserProjectRepository _userProjectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserSkillRepository _userSkillRepository;
        private readonly IUserWorkRepository _userWorkRepository;


        public UserService(IUserRepository userRepository,
                           IUserAboutRepository userAboutRepository,
                           IUserWorkRepository userWorkRepository,
                           IUserFeaturesRepository userFeaturesRepository,
                           IUserSkillRepository userSkillRepository,
                           IUserProjectRepository userProjectRepository,
                           IUnitOfWork unitOfWork
                           )
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
                UserSkills = _userSkillRepository.GetPageListByUserId(userId).ToList(),
                UserProjects = _userProjectRepository.GetPageListByUserId(userId).ToList()
            };


            return readMeVm;
        }

        /// <summary>
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public AuthenticateVm Authenticate(string username, string password)
        {
            var user = _userRepository.TableNoTracking.SingleOrDefault(
                x => x.UserName == username && x.PassWord == password);

            // return null if user not found
            if (user == null)
                return null;

            var userModel = new AuthenticateVm();
        


          
            return userModel;
        }

        /// <summary>
        ///     gets user list
        /// </summary>
        /// <returns></returns>
        public List<User> GetAll()
        {
            return _userRepository.TableNoTracking.ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Authenticate(string username, object password)
        {
            throw new NotImplementedException();
        }

        public AuthenticateVm PasswordSignIn(string username, string password, bool RememberMe)
        {
            throw new NotImplementedException();
        }
    }
}