using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicService.Data;
using MusicService.Models;

namespace MusicService.Pages.Admin.Song
{
    public class DetailsModel : PageModel
    {
        private readonly MusicService.Data.MusicContext _context;

        public DetailsModel(MusicService.Data.MusicContext context)
        {
            _context = context;
        }

        public Models.Song Song { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Song = await _context.Songs.FirstOrDefaultAsync(m => m.ID == id);

            if (Song == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
