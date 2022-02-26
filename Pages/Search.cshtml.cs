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
    public class SearchModel : PageModel
    {
        private readonly MusicService.Data.MusicContext _context;

        public SearchModel(MusicService.Data.MusicContext context)
        {
            _context = context;
        }

        public IList<Song> Song { get;set; }

        public async Task OnGetAsync(string searchinput)
        {
            Song = await _context.Songs.Where(song => song.Name.ToLower().Contains(searchinput)).Include(song => song.Artists).ToListAsync();
        }
    }
}
