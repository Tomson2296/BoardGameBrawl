#nullable disable

using BoardGameBrawl;
using BoardGameBrawl.Data.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoardGameBrawl.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LockoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LockoutModel(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult OnGet()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToPage("/Index", new { area = "" });
            }
            return Page();
        }
    }
}
