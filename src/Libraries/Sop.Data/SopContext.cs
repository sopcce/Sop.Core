using Microsoft.EntityFrameworkCore;
using Sop.Core.Models;

namespace Sop.Data
{
    public class SopContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public SopContext(DbContextOptions<SopContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<FileServerInfo>();
            builder.Entity<CitysInfo>();

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<FileServerInfo> FileServer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<CitysInfo> CitysServer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<ChangelogInfo> ChangelogServer { get; set; }
    }
}
 