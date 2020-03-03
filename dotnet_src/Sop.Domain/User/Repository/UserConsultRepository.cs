using System;
using Microsoft.EntityFrameworkCore;
using Sop.Data.Repository;
using Sop.Domain.Entity;
using Sop.Domain.VModel;

namespace Sop.Domain.Repository
{
    public class UserConsultRepository : EfCoreRepository<UserConsult>, IUserConsultRepository
    {
        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        public UserConsultRepository(DbContext context) : base(context)
        {
           
        }

         
    }
}