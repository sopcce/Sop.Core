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
        /// 
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
        bool SignIn(string username, string password);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<User> GetAll();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        bool Insert(RegisterVm register);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool ExistUserName(string userName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mobilePhone"></param>
        /// <returns></returns>
        bool ExistMobilePhone(string mobilePhone);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        bool ExistEmail(string email);
    }
}