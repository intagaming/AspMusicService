using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MusicService.Data;
using MusicService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicService.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly MusicContext _context;

        public IndexModel(ILogger<IndexModel> logger, MusicContext context)
        {
            _logger = logger;
            _context = context;
        }

        public List<Song> NewSongs { get; set; }

        public void OnGet()
        {
            NewSongs = _context.Songs.OrderByDescending(song => song.ID).Include(song => song.Artists).Take(5).ToList();
        }
    }
}
