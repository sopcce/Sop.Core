using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sop.FileUpload.Models;

namespace Sop.FileUpload.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Sop.FileUpload.Models.SopFileUploadContext _context;

        public IndexModel(Sop.FileUpload.Models.SopFileUploadContext context)
        {
            _context = context;
        }

        public IList<Fileserver> Fileserver { get;set; }

        public async Task OnGetAsync()
        {
            Fileserver = await _context.Fileserver.ToListAsync();
        }
    }
}
