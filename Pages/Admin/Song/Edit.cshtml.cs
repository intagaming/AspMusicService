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
        public IFormFile MusicFile { get; set; }

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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (MusicFile != null)
            {
                var fileName = $"{Song.ID}-{MusicFile.FileName}";
                var path = Path.Combine(_env.ContentRootPath, "wwwroot/songs", fileName);
                using var fs = new FileStream(path, FileMode.Create);
                await MusicFile.CopyToAsync(fs);
                Song.FileName = fileName;
            }

            _context.Attach(Song).State = EntityState.Modified;

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
