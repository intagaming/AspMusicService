using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicService.Data;
using MusicService.Models;

namespace MusicService.Pages.Playlists
{
    public class EditModel : PageModel
    {
        private readonly MusicService.Data.MusicContext _context;

        public EditModel(MusicService.Data.MusicContext context)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Playlist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaylistExists(Playlist.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index", new { id = Playlist.ID });
        }

        private bool PlaylistExists(int id)
        {
            return _context.Playlists.Any(e => e.ID == id);
        }
    }
}
