using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicService.Data;
using MusicService.Models;

namespace MusicService.Pages.Users
{
    public class DetailsModel : PageModel
    {
        private readonly MusicService.Data.MusicContext _context;

        public DetailsModel(MusicService.Data.MusicContext context)
        {
            _context = context;
        }

        public User UserModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserModel = await _context.User.FirstOrDefaultAsync(m => m.Id == id);

            if (UserModel == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
