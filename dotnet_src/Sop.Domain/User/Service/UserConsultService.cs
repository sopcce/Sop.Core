using System;
using Sop.Data;
using Sop.Domain.Entity;
using Sop.Domain.Repository;
using Sop.Domain.VModel;

namespace Sop.Domain.Service
{
    public class UserConsultService : IUserConsultService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserConsultRepository _userConsultRepository;

        public UserConsultService(IUserConsultRepository userConsultRepository, IUnitOfWork unitOfWork)
        {
            _userConsultRepository = userConsultRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        ///     /
        /// </summary>
        /// <param name="userConsult"></param>
        public void Edit(UserConsultVm userConsult)
        {
            var info = new UserConsult
            {
                UserId = 1,
                FirstName = userConsult.FirstName,
                LastName = userConsult.LastName,
                Email = userConsult.Email,
                Body = userConsult.Message,
                Subject = userConsult.Subject,
                CreateTime = DateTime.Now
            };
            _userConsultRepository.Insert(info);
            _unitOfWork.SaveChanges();
        }
    }
}