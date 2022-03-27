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
using MusicService.Data;
using MusicService.Models;

namespace MusicService.Pages.Admin.Song
{
    public class CreateModel : PageModel
    {
        private readonly MusicService.Data.MusicContext _context;
        private readonly IWebHostEnvironment _env;

        public CreateModel(MusicService.Data.MusicContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult OnGet()
        {
            string role = HttpContext.Session.GetString("role");
            if (role == "user")
            {
                return RedirectToPage("/Login/Index");
            }
            return Page();
        }

        [BindProperty]
        public Models.Song Song { get; set; }
        [BindProperty]
        public string Artist { get; set; }
        [BindProperty]
        public IFormFile MusicFile { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var fileName = $"{Song.ID}-{MusicFile.FileName}";
            var path = Path.Combine(_env.ContentRootPath, "wwwroot/songs", fileName);
            var fileExt = Path.GetExtension(path);
            if (fileExt != ".mp3" && fileExt != ".mp4")
            {
                return Page();
            }
            using var fs = new FileStream(path, FileMode.Create);
            await MusicFile.CopyToAsync(fs);
            Song.FileName = fileName;
            Song.Artists = new HashSet<Artist>();
            string artistName = Artist;
            var existedArtist = _context.Artists.Where(x => x.Name.ToLower() == artistName.ToLower().Trim()).FirstOrDefault();
            if(existedArtist != null)
            {
                Song.Artists.Add(existedArtist);
            }
            else
            {
                Artist newArtist = new Models.Artist { Name = artistName };
                _context.Artists.Add(newArtist);
                await _context.SaveChangesAsync();
                Song.Artists.Add(newArtist);
            }

            _context.Songs.Add(Song);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
