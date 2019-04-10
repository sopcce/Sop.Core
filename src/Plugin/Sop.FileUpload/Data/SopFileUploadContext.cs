using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sop.FileUpload.Models
{
    public class SopFileUploadContext : DbContext
    {
        public SopFileUploadContext (DbContextOptions<SopFileUploadContext> options)
            : base(options)
        {
        }

        public DbSet<Sop.FileUpload.Models.Fileserver> Fileserver { get; set; }
    }
}
