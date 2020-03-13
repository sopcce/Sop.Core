using Sop.Domain.VModel;

namespace Sop.Domain.Service
{
    public interface IUserConsultService
    {
        /// <summary>
        /// </summary>
        /// <param name="userConsult"></param>
        void Edit(UserConsultVm userConsult);
    }
}