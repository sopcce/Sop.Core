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
    public class DeleteModel : PageModel
    {
        private readonly Sop.FileUpload.Models.SopFileUploadContext _context;

        public DeleteModel(Sop.FileUpload.Models.SopFileUploadContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Fileserver = await _context.Fileserver.FindAsync(id);

            if (Fileserver != null)
            {
                _context.Fileserver.Remove(Fileserver);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
