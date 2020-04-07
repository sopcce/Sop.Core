using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sop.Core.Utility;
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
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool SignIn(string username, string password)
        {
            var user = _userRepository.TableNoTracking.SingleOrDefault(
                x => x.UserName == username);

            // return null if user not found
            if (user == null)
                return false;
            //º”√‹ 
            var inputPass = EncryptionUtility.Sha512Encode(password + user.SecurityStamp);

            if (user.PassWord == inputPass)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        public bool Insert(RegisterVm register)
        {
            var securityStamp = Guid.NewGuid().ToString("N");

            var user = new User
            {
                UserId = Guid.NewGuid().ToString(),
                UserName = register.UserName,
                UrlToken = null,
                Email = register.Email,
                EmailConfirmed = 0,
                MobilePhone = register.MobilePhone,
                MobilePhoneConfirmed = 0,
                PassWord = EncryptionUtility.Sha512Encode(register.PassWord + securityStamp),
                SecurityStamp = securityStamp,
                TrueName = null,
                NickName = register.NickName,
                Status = 0,
                StatusNotes = null,
                CreatedIP = null,
                ActivityTime = DateTime.Now,
                LastActivityTime = DateTime.Now,
                ActivityIP = null,
                LastActivityIP = null,
                DateCreated = DateTime.Now,
                DisplayOrder = 0,
                LockoutEndDateUtc = DateTime.Now,
                LockoutEnabled = 0,
                AccessFailedCount = 0,
                TwoFactorEnabled = 0
            };

            _userRepository.Insert(user);
            _unitOfWork.SaveChanges();
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool ExistUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return false;
            }

            var info = _userRepository.TableNoTracking?.FirstOrDefault(user => user.UserName == userName);
            return info != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mobilePhone"></param>
        /// <returns></returns>
        public bool ExistMobilePhone(string mobilePhone)
        {
            if (string.IsNullOrWhiteSpace(mobilePhone))
            {
                return false;
            }
            var info = _userRepository.TableNoTracking?.FirstOrDefault(user => user.MobilePhone == mobilePhone);
            return info != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool ExistEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }
            var info = _userRepository.TableNoTracking?.FirstOrDefault(user => user.Email == email);
            return info != null;
        }


        /// <summary>
        ///     gets user list
        /// </summary>
        /// <returns></returns>
        public List<User> GetAll()
        {
            return _userRepository.TableNoTracking.ToList();
        }



    }
}