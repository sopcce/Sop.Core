using Microsoft.EntityFrameworkCore;
using Sop.Data.Repository;

namespace WebApi
{

    /// <summary>
    /// 
    /// </summary>
    public class EfDbBaseDbContext : BaseDbContext
    {
        /// <summary>
        /// </summary>
        /// <param name="options"></param>
        public EfDbBaseDbContext(DbContextOptions options) : base(options)
        {
            SetOnModelCreatingType(OnModelCreatingType.UseEntityMap);
        }

        /// <summary>
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}