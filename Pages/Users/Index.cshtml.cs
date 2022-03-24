using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicService.Data;
using MusicService.Models;

namespace MusicService.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly MusicService.Data.MusicContext _context;

        public IndexModel(MusicService.Data.MusicContext context)
        {
            _context = context;
        }

        public IList<User> UserModel { get;set; }

        public async Task OnGetAsync()
        {
            UserModel = await _context.User.ToListAsync();
        }
    }
}
