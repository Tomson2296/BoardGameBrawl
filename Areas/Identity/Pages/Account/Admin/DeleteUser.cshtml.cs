#nullable disable

using BoardGameBrawl.Data.Models;
using BoardGameBrawl.Data.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoardGameBrawl.Areas.Identity.Pages.Account.Admin
{
    [Authorize(Roles = "Administrator")]
    public class DeleteUserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteUserModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public ApplicationUser ApplicationUser;

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(Id))
            {
                return RedirectToPage("./ListUsers");
            }

            ApplicationUser = await _userManager.FindByIdAsync(Id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!string.IsNullOrEmpty(Id) && ModelState.IsValid)
            {
                ApplicationUser = await _userManager.FindByIdAsync(Id);
                IdentityResult result = await _userManager.DeleteAsync(ApplicationUser);

                if (result.Succeeded)
                {
                    return RedirectToPage("ManageUsers");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                return Page();
            }
            return Page();
        }
    }
}
