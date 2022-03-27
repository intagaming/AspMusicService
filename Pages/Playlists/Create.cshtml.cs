using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicService.Data;
using MusicService.Models;

namespace MusicService.Pages.Playlists
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
            string username = HttpContext.Session.GetString("username");
            var UserModel = _context.User.Where(u => u.Username.Equals(username)).FirstOrDefault();
            Playlists = _context.Playlists.Where(p =>  p.User != null && p.User == UserModel).ToList();
            return Page();
        }

        [BindProperty]
        public Playlist Playlist { get; set; }
        public List<Playlist> Playlists { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string username = HttpContext.Session.GetString("username");
            var user = _context.User.Where(u => u.Username.Equals(username)).FirstOrDefault();

            Playlist.User = user;

            _context.Playlists.Add(Playlist);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { id = Playlist.ID });
        }
    }
}
