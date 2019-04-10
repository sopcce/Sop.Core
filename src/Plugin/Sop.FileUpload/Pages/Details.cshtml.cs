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
    public class DetailsModel : PageModel
    {
        private readonly Sop.FileUpload.Models.SopFileUploadContext _context;

        public DetailsModel(Sop.FileUpload.Models.SopFileUploadContext context)
        {
            _context = context;
        }

        public Fileserver Fileserver { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Fileserver = await _context.Fileserver.FirstOrDefaultAsync(m => m.Id == id);

            if (Fileserver == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
