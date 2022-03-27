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
        public List<Playlist> Playlists { get; set; }

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

            string username = HttpContext.Session.GetString("username");
            var UserModel = _context.User.Where(u => u.Username.Equals(username)).FirstOrDefault();
            Playlists = _context.Playlists.Where(p => p.User != null && p.User == UserModel).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAddToPlaylistAsync(int playlistId, int songId)
        {
            var playlist = await _context.Playlists.Where(p => p.ID == playlistId).Include(p => p.Songs).FirstOrDefaultAsync();
            Song = await _context.Songs.Include(song => song.Artists).FirstOrDefaultAsync(m => m.ID == songId);

            string username = HttpContext.Session.GetString("username");
            var UserModel = _context.User.Where(u => u.Username.Equals(username)).FirstOrDefault();
            Playlists = _context.Playlists.Where(p => p.User != null && p.User == UserModel).ToList();

            if (playlist == null || Song == null)
            {
                return Page();
            }

            playlist.Songs.Add(Song);
            await _context.SaveChangesAsync();
            return Page();
        }
    }
}
