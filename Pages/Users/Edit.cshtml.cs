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

namespace MusicService.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly MusicService.Data.MusicContext _context;

        public EditModel(MusicService.Data.MusicContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User UserModel { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                return NotFound();
            }
            UserModel = await _context.User.FirstOrDefaultAsync(m => m.Username == username);

            if (UserModel == null)
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

            _context.Attach(UserModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(UserModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Page();
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }

        public IActionResult OnPostChangePassword(string newPass, string retypePass)
        {
            if (newPass != retypePass) return RedirectToPage();
            UserModel.Password = newPass;
            _context.Attach(UserModel).State = EntityState.Modified;
            _context.SaveChanges();
            return Page();
        }
    }
}
