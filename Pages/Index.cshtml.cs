using Microsoft.AspNetCore.Http;
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
        public User UserModel { get; set; }
        public string Username => UserModel != null ? UserModel.Username : "";

        public IndexModel(ILogger<IndexModel> logger, MusicContext context)
        {
            _logger = logger;
            _context = context;
        }

        public List<Song> NewSongs { get; set; }
        public List<Playlist> Playlists { get; set; }

        public void OnGet()
        {
            string username = HttpContext.Session.GetString("username");
            UserModel = _context.User.Where(u => u.Username.Equals(username)).FirstOrDefault();
            NewSongs = _context.Songs.OrderByDescending(song => song.ID).Include(song => song.Artists).ToList();
            Playlists = _context.Playlists.Where(p => p.User != null && p.User == UserModel).ToList();
        }

        
    }
}
