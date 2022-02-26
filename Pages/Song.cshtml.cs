using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicService.Data;
using MusicService.Models;

namespace MusicService.Pages
{
    public class SongModel : PageModel
    {
        private readonly MusicService.Data.MusicContext _context;

        public SongModel(MusicService.Data.MusicContext context)
        {
            _context = context;
        }

        public Song Song { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Song = await _context.Songs.Include(song => song.Artists).FirstOrDefaultAsync(m => m.ID == id);

            if (Song == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
