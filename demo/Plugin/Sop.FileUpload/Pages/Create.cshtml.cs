using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sop.Core.Models;
using Sop.Data;
using Sop.FileUpload.Models;
using Sop.Services;

namespace Sop.FileUpload.Pages
{
    public class CreateModel : PageModel
    {
        private readonly SopContext _context;

        public CreateModel(SopContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public FileServerInfo Fileserver { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.FileServer.Add(Fileserver);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}