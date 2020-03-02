using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sop.Core.Models;
using Sop.Data;
using Sop.FileUpload.Models;
using Sop.Services;

namespace Sop.FileUpload.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SopContext _context;

        public IndexModel(SopContext context)
        {
            _context = context;
        }

        public IList<FileServerInfo> Fileserver { get;set; }

        public async Task OnGetAsync()
        {
            Fileserver = await _context.FileServer.ToListAsync();
        }
    }
}
