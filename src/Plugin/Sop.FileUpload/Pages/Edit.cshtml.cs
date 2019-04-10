using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sop.FileUpload.Models;

namespace Sop.FileUpload.Pages
{
    public class EditModel : PageModel
    {
        private readonly Sop.FileUpload.Models.SopFileUploadContext _context;

        public EditModel(Sop.FileUpload.Models.SopFileUploadContext context)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Fileserver).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FileserverExists(Fileserver.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool FileserverExists(int id)
        {
            return _context.Fileserver.Any(e => e.Id == id);
        }
    }
}
