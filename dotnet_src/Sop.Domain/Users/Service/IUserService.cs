using System.Collections.Generic;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="RememberMe"></param>
        /// <returns></returns>
        AuthenticateVm PasswordSignIn(string username, string password, bool RememberMe);

        /// <summary>
        /// </summary>
        /// <returns></returns>
        List<User> GetAll();
    }
}