using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicService.Data;
using MusicService.Models;

namespace MusicService.Pages.Admin.Song
{
    public class EditModel : PageModel
    {
        private readonly MusicService.Data.MusicContext _context;
        private readonly IWebHostEnvironment _env;

        public EditModel(MusicService.Data.MusicContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public Models.Song Song { get; set; }
        [BindProperty]
        public string Artist { get; set; }
        [BindProperty]
        public IFormFile MusicFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string role = HttpContext.Session.GetString("role");
            if (role == "user")
            {
                return RedirectToPage("/Login/Index");
            }
            if (id == null)
            {
                return NotFound();
            }

            Song = await _context.Songs.Include(s => s.Artists).FirstOrDefaultAsync(m => m.ID == id);

            if (Song == null)
            {
                return NotFound();
            }
            ICollection<Artist> Artists = Song.Artists;
            Artist firstArtist = Artists.FirstOrDefault();

            if(firstArtist != null)
            {
                Artist = firstArtist.Name;
            }
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
            var SongFromDb = _context.Songs.Where(s => s.ID == Song.ID).Include(a => a.Artists).FirstOrDefault();

            if (MusicFile != null)
            {
                var fileName = $"{Song.ID}-{MusicFile.FileName}";
                var path = Path.Combine(_env.ContentRootPath, "wwwroot/songs", fileName);
                var fileExt = Path.GetExtension(path);
                if (fileExt != ".mp3" && fileExt != ".mp4")
                {
                    return Page();
                }
                using var fs = new FileStream(path, FileMode.Create);
                await MusicFile.CopyToAsync(fs);
                SongFromDb.FileName = fileName;
            }
            SongFromDb.Artists.Clear();
            string artistName = Artist;
            var existedArtist = _context.Artists.Where(x => x.Name.ToLower() == artistName.ToLower().Trim()).FirstOrDefault();
            if (existedArtist != null)
            {
                SongFromDb.Artists.Add(existedArtist);
            }
            else
            {
                Artist newArtist = new Models.Artist { Name = artistName };
                _context.Artists.Add(newArtist);
                await _context.SaveChangesAsync();
                SongFromDb.Artists.Add(newArtist);
            }

            //_context.Attach(Song).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SongExists(Song.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SongExists(int id)
        {
            return _context.Songs.Any(e => e.ID == id);
        }
    }
}
