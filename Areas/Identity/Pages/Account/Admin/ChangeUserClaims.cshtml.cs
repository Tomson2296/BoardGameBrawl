#nullable disable
using BoardGameBrawl.Data.Models;
using BoardGameBrawl.Data.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace BoardGameBrawl.Areas.Identity.Pages.Account.Admin
{
    [Authorize(Roles = "Administrator")]
    public class ChangeUserClaimsModel : PageModel
    {
        public UserManager<ApplicationUser> _userManager;
        public SignInManager<ApplicationUser> _singInManager;

        public ChangeUserClaimsModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> singInManager)
        {
            _userManager = userManager;
            _singInManager = singInManager;
        }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public IList<Claim> Claims = new List<Claim>();

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(Id))
            {
                return RedirectToPage("ManageUsers");
            }
            ApplicationUser user = await _userManager.FindByIdAsync(Id);
            Claims = await _userManager.GetClaimsAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostEditClaim([Required] string type,
            [Required] string value, [Required] string oldValue)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(Id);
            if (ModelState.IsValid)
            {
                var claimNew = new Claim(type, value);
                var claimOld = new Claim(type, oldValue);
                var result = await _userManager.ReplaceClaimAsync(user, claimOld, claimNew);

                if (result.Succeeded)
                {
                    StatusMessage = "Claim has been successfully edited.";
                    Claims = await _userManager.GetClaimsAsync(user);
                    return RedirectToPage();
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        StatusMessage = "Error - Claim has not been edited. Try Again.";
                        ModelState.AddModelError(string.Empty, error.Description);
                        return Page();
                    }
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteClaim([Required] string type,
            [Required] string value)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(Id);
            if (ModelState.IsValid)
            {
                var claim = new Claim(type, value);
                var result = await _userManager.RemoveClaimAsync(user, claim);

                if (result.Succeeded)
                {
                    StatusMessage = "Claim has been successfully deleted.";
                    Claims = await _userManager.GetClaimsAsync(user);
                    return RedirectToPage();
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        StatusMessage = "Error - Claim has not been deleted. Try Again.";
                        ModelState.AddModelError(string.Empty, error.Description);
                        return Page();
                    }
                }
            }
            return Page();
        }
    }
}
