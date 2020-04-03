using System.Linq;
using Sop.Data;
using Sop.Domain.Entity;
using Sop.Domain.Repository;

namespace Sop.Domain.Service
{
    /// <summary>
    /// </summary>
    public class UserAboutService : IUserAboutService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserAboutRepository _userAboutRepository;

        public UserAboutService(IUserAboutRepository userAboutRepository, IUnitOfWork unitOfWork)
        {
            _userAboutRepository = userAboutRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserAbout GetById(int id)
        {
            return _userAboutRepository.GetById(id);
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserAbout GetByUserId(int id)
        {
            return _userAboutRepository.TableNoTracking.FirstOrDefault(n => n.UserId == id);
        }
    }
}