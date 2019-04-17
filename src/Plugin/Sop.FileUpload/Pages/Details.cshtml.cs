using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sop.Core.Models;
using Sop.Data;
using System.Threading.Tasks;

namespace Sop.FileUpload.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly SopContext _context;

        public DetailsModel(SopContext context)
        {
            _context = context;
        }

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
    }
}
