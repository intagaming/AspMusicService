using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicService.Data;
using MusicService.Models;

namespace MusicService.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly MusicService.Data.MusicContext _context;

        public CreateModel(MusicService.Data.MusicContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public User UserModel { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.User.Add(UserModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Login/Index");
        }
    }
}
