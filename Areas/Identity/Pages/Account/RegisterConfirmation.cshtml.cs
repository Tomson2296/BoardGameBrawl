#nullable disable

using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Services;

namespace BoardGameBrawl.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMailKitService _mailKitService;
        
        public RegisterConfirmationModel(SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager, IMailKitService mailKitService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mailKitService = mailKitService;
        }

        public string Email { get; set; }

        public bool DisplayConfirmAccountLink { get; set; }

        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToPage("/Index", new { area = "" });
            }

            if (email == null)
            {
                return RedirectToPage("/Index");
            }
            returnUrl = returnUrl ?? Url.Content("~/");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }
            Email = email;

            return Page();
        }
    }
}
