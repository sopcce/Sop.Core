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
        public SopContext (DbContextOptions<SopContext> options)
            : base(options)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<FileServerInfo> FileServer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<CitysInfo> CitysServer { get; set; }
    }
}
