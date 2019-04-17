using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sop.Core.Models;
using Sop.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Sop.FileUpload.Pages
{
    public class EditModel : PageModel
    {
        private readonly SopContext _context;

        public EditModel(SopContext context)
        {
            _context = context;
        }

        [BindProperty]
        public FileServerInfo Fileserver { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Fileserver = await _context.FileServer.FirstOrDefaultAsync(m => m.Id == id);

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
            return _context.FileServer.Any(e => e.Id == id);
        }
    }
}
