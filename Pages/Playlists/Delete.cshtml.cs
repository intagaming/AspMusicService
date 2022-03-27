using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicService.Data;
using MusicService.Models;

namespace MusicService.Pages.Playlists
{
    public class DeleteModel : PageModel
    {
        private readonly MusicService.Data.MusicContext _context;

        public DeleteModel(MusicService.Data.MusicContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Playlist Playlist { get; set; }
        public List<Playlist> Playlists { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Playlist = await _context.Playlists.FirstOrDefaultAsync(m => m.ID == id);

            if (Playlist == null)
            {
                return NotFound();
            }

            string username = HttpContext.Session.GetString("username");
            var UserModel = _context.User.Where(u => u.Username.Equals(username)).FirstOrDefault();
            Playlists = _context.Playlists.Where(p => p.User != null && p.User == UserModel).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Playlist = await _context.Playlists.FindAsync(id);

            if (Playlist != null)
            {
                _context.Playlists.Remove(Playlist);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Index");
        }
    }
}
