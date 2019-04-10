using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sop.FileUpload.Models;

namespace Sop.FileUpload.Pages
{
    public class CreateModel : PageModel
    {
        private readonly Sop.FileUpload.Models.SopFileUploadContext _context;

        public CreateModel(Sop.FileUpload.Models.SopFileUploadContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Fileserver Fileserver { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Fileserver.Add(Fileserver);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}