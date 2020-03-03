using Sop.Domain.Entity;
using Sop.Domain.VModel;

namespace Sop.Domain.Service
{
    public interface IUserService
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User GetById(long id);

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        ReadMeVm GetReadMeVmByUserId(long userId);
    }
}