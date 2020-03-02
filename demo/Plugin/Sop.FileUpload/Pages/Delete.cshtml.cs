using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sop.Core.Models;
using Sop.Data;
using System.Threading.Tasks;

namespace Sop.FileUpload.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly SopContext _context;

        public DeleteModel(SopContext context)
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Fileserver = await _context.FileServer.FindAsync(id);

            if (Fileserver != null)
            {
                _context.FileServer.Remove(Fileserver);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
