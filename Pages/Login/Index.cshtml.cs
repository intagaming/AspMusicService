using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicService.Models;
using MusicService.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace MusicService.Pages
{
    public class LoginModel : PageModel
    {
        public MusicContext _context;

        public LoginModel(MusicContext context)
        {
            _context = context;
        }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync(string username, string password, string rememberMe)
        {
            var user = await _context.User.Where(u => u.Username == username.Trim() && u.Password == password).FirstOrDefaultAsync();
            if (user == null)
            {
                return Page();
            }
            HttpContext.Session.SetString("username", username);
            HttpContext.Session.SetString("role", user.IsAdmin ?"admin":"user");

            if (user.IsAdmin) return RedirectToPage("/Admin/Song/Index");
            return RedirectToPage("../Index");
        }
    }
}
