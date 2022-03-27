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
    public class DetailsModel : PageModel
    {
        private readonly MusicService.Data.MusicContext _context;

        public DetailsModel(MusicService.Data.MusicContext context)
        {
            _context = context;
        }

        public Playlist Playlist { get; set; }
        public List<Playlist> Playlists { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Playlist = await _context.Playlists.Include(p => p.Songs).ThenInclude(s => s.Artists).FirstOrDefaultAsync(m => m.ID == id);

            if (Playlist == null)
            {
                return NotFound();
            }

            string username = HttpContext.Session.GetString("username");
            var UserModel = _context.User.Where(u => u.Username.Equals(username)).FirstOrDefault();
            Playlists = _context.Playlists.Where(p => p.User != null && p.User == UserModel).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveFromPlaylistAsync(int playlistId, int songId)
        {
            Playlist = await _context.Playlists.Include(p => p.Songs).ThenInclude(s => s.Artists).FirstOrDefaultAsync(m => m.ID == playlistId);

            if (Playlist == null)
            {
                return NotFound();
            }

            string username = HttpContext.Session.GetString("username");
            if (Playlist.User.Name != username)
            {
                return NotFound();
            }

            var song = Playlist.Songs.Where(s => s.ID == songId).FirstOrDefault();
            if (song == null)
            {
                return NotFound();
            }

            Playlist.Songs.Remove(song);
            _context.SaveChanges();

            var UserModel = _context.User.Where(u => u.Username.Equals(username)).FirstOrDefault();
            Playlists = _context.Playlists.Where(p => p.User != null && p.User == UserModel).ToList();

            return Page();
        }
    }
}
