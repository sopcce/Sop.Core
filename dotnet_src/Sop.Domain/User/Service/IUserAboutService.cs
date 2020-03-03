using Sop.Domain.Entity;

namespace Sop.Domain.Service
{
    /// <summary>
    /// </summary>
    public interface IUserAboutService
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserAbout GetById(int id);

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserAbout GetByUserId(int id);
    }
}